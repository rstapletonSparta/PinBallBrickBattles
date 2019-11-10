using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

enum MovementStates
{
    Normal, Dash, LongKnockBack, ShortKnockBack
}

namespace PinBallBattles
{
    class Player
    {
        private bool test = false;
        private Texture2D texture;
        private Vector2 position;
        private MovementStates moveState = MovementStates.Normal;
        private int radius;

        private float sideSpeed = 150;
        private float dashSpeed = 1500;
        private float normalRepel = 900;
        private int knockBackDuration = 500;
        private double cooldowntime = 0;
        
        public Texture2D Texture { get => texture; }
        public Vector2 Position { get => position; set => position = value; }
        public MovementStates MoveState { get => moveState; set => moveState = value; }
        public int Radius { get => radius; }

        public Player(Vector2 position, int radius, bool test)
        {
            this.position = position;
            this.radius = radius;
            this.test = test;
        }

        public Vector2 PlayerMovementController
            (GameTime gameTime, float playersDistance, bool playersCollision, Player other)
        {
            Vector2 movePos = new Vector2();
            Vector2 normal = PlayerToPLayerNormal
                (gameTime, playersDistance, other);
            
            
            if (playersCollision)
            {
                moveState = CollisionStateChange(other);
                //if (moveState == MovementStates.Normal)
                //{
                //    movePos -= normal * normalRepel;
                //}
            }

            if (moveState == MovementStates.Normal)
            {
                movePos += PlayerInputUpdate(gameTime, sideSpeed);
            }
            else if (moveState == MovementStates.Dash)
            {
                movePos += PlayerInputUpdate(gameTime, dashSpeed);
            }
            else if (moveState == MovementStates.LongKnockBack)
            {
                // need something that will save collision direction
                // instead of normal
                movePos -= normal * normalRepel;

                if (!CooldownTimer(gameTime, knockBackDuration))
                {
                    moveState = MovementStates.Normal;
                }
            }
            else if (moveState == MovementStates.ShortKnockBack)
            {
                movePos -= normal * 200;
                
                if (!CooldownTimer(gameTime, 50))
                {
                    moveState = MovementStates.Normal;
                }
            }
            Console.WriteLine(moveState);
            
            return movePos * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        private Vector2 PlayerInputUpdate(GameTime gameTime, float speed)
        {
            Vector2 inputPos = new Vector2();

            if (test)
            {
                var keyboard = Keyboard.GetState();

                if (keyboard.IsKeyDown(Keys.A))
                {
                    inputPos.X -= speed;
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    inputPos.X += speed;
                }

                if (keyboard.IsKeyDown(Keys.Space))
                {
                    moveState = MovementStates.Dash;
                }
                else
                {
                    moveState = MovementStates.Normal;
                }
            }

            return inputPos;
        }

        public MovementStates CollisionStateChange(Player other)
        {
            if (moveState == MovementStates.Dash
                && other.moveState == MovementStates.Dash)
            {
                return MovementStates.ShortKnockBack;
            }
            else if (moveState == MovementStates.Dash
                && other.moveState == MovementStates.Normal)
            {
                return MovementStates.ShortKnockBack;
            }
            else if (moveState == MovementStates.Normal
                && other.moveState == MovementStates.Dash)
            {
                return MovementStates.LongKnockBack;
            }
            else
            {
                return MovementStates.Normal;
            }
        }

        public bool CooldownTimer(GameTime gameTime, double duration)
        {
            cooldowntime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (cooldowntime >= duration)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public Vector2 PlayerToPLayerNormal(GameTime gameTime, float dis, Player other)
        {
            Vector2 pushDir = new Vector2();

            pushDir.X = VectorDirection(other).X / dis;
            pushDir.Y = VectorDirection(other).Y / dis;
            
            return pushDir;
        }

        public Vector2 VectorDirection(Player other)
        {
            return other.position - position;
        }
        
        public void createCircleText(GraphicsDevice graphics)
        {
            // this guy has got radius and diamiter backwards

            texture = new Texture2D(graphics, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            texture.SetData(colorData);
        }
    }
    
}
