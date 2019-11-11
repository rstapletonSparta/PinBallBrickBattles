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
        private float longRepel = 1300;
        private float shortRepel = 1300;
        private int longKnockBack = 500;
        private int shortKnockBack = 200;
        private double cooldowntime = 0;
        private Vector2 KnockbackDir;
        
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
            (GameTime gameTime, float playersDistance, bool playersCollision, Player other, GraphicsDeviceManager graphics)
        {
            Vector2 movePos = new Vector2();
            Vector2 normal = PlayerToPLayerNormal
                (playersDistance, other);

            switch (moveState)
            {
                case MovementStates.Normal:
                    movePos += PlayerInputUpdate(sideSpeed);
                    if (playersCollision)
                    {
                        movePos -= normal * normalRepel;
                    }
                break;
                case MovementStates.Dash:
                    movePos += PlayerInputUpdate(dashSpeed);
                break;
                case MovementStates.LongKnockBack:
                    movePos -= normal * longRepel;
                    if (CooldownTimer(gameTime, longKnockBack))
                    {
                        moveState = MovementStates.Normal;
                        cooldowntime = 0;
                    }
                break;
                case MovementStates.ShortKnockBack:
                    movePos -= normal * shortRepel;
                    if (CooldownTimer(gameTime, shortKnockBack))
                    {
                        moveState = MovementStates.Normal;
                        cooldowntime = 0;
                    }
                break;
            }

            //Console.WriteLine(moveState);
            
            return movePos * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void ObjectCollision()
        {

        }







        public void SetKnockbackDirection(float playersDistance, Player other)
        {
            KnockbackDir = PlayerToPLayerNormal
                (playersDistance, other);
        }
        
        private Vector2 PlayerInputUpdate(float speed)
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
        
        public bool CooldownTimer(GameTime gameTime, double duration)
        {
            cooldowntime += gameTime.ElapsedGameTime.TotalMilliseconds;
            //Console.WriteLine(cooldowntime);
            if (cooldowntime >= duration)
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public Vector2 PlayerToPLayerNormal(float dis, Player other)
        {
            Vector2 pushDir = new Vector2();

            pushDir = VectorDirection(other) / dis;
            
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
