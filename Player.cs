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
        private int diam = 0;

        private float sideSpeed = 300;
        private float sideReSpeed = 300;

        private float dashSpeed = 1000;
        private float normalRepel = 600;
        private float longRepel = 1100;
        private float shortRepel = 800;
        private int longKnockBack = 1000;
        private int shortKnockBack = 500;
        private double cooldowntime = 0;
        private Vector2 velocity;
        private Brick lastBrick;
        
        public Texture2D Texture { get => texture; }
        public Vector2 Position { get => position; set => position = value; }
        public MovementStates MoveState { get => moveState; set => moveState = value; }
        public int Diam { get => diam; }

        public Player(Vector2 position, int diam, bool test)
        {
            this.position = position;
            this.diam = diam;
            this.test = test;
        }

        public Vector2 PlayerMovementController
            (GameTime gameTime, float playersDistance, bool playersCollision, Player other, 
            GraphicsDeviceManager graphics, Brick[] objs)
        {
            Vector2 movePos = new Vector2();

            Vector2 normal = PlayerToPLayerNormal
                (playersDistance, other);

            if (!test)
            {
                Console.WriteLine(velocity);
                Console.WriteLine(moveState);
            }

            Brick newBrick = ObjectCollision(objs);
            if (newBrick != null)
            {
                Vector2 n = GetVelocityToBrick(newBrick);
                ReverseVelocity(n);
                lastBrick = newBrick;
            }

            switch (moveState)
            {
                case MovementStates.Normal:
                    movePos += PlayerInputUpdate(sideSpeed);
                    if (newBrick != null && newBrick == lastBrick)
                    {
                        Vector2 n = GetVelocityToBrick(newBrick);
                        movePos += n * 400;
                    }
                    break;
                case MovementStates.Dash:
                    movePos += PlayerInputUpdate(dashSpeed);
                    if (newBrick != null && newBrick == lastBrick)
                    {
                        Vector2 n = GetVelocityToBrick(newBrick);
                        movePos += n * longRepel;
                    }
                    break;
                case MovementStates.LongKnockBack:
                    movePos -= velocity * longRepel;
                    
                    if (CooldownTimer(gameTime, longKnockBack))
                    {
                        moveState = MovementStates.Normal;
                        cooldowntime = 0;
                    }
                break;
                case MovementStates.ShortKnockBack:
                    movePos -= velocity * shortRepel;
                    if (CooldownTimer(gameTime, shortKnockBack))
                    {
                        moveState = MovementStates.Normal;
                        cooldowntime = 0;
                    }
                break;
            }
            
            return movePos * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void ReverseVelocity(Vector2 newDir)
        {
            if (newDir.X > 0)
            {
                velocity.X = -velocity.X;
            }
            else if (newDir.X < 0)
            {
                velocity.X = Math.Abs(velocity.X);
            }
            if (newDir.Y > 0)
            {
                velocity.Y = -velocity.Y;
            }
            else if (newDir.Y < 0)
            {
                velocity.Y = Math.Abs(velocity.Y);
            }
        }

        #region ObjectCollision

        public Brick ObjectCollision(Brick[] bricks)
        {
            foreach (Brick b in bricks)
            {
                Vector2 closestTest = b.ClosestEdge(position);
                float check = ToPlayerDistance(closestTest);

                if (PlayerToObjCollision(check))
                {
                    return b;
                }
            }
            return null;
        }

        public Vector2 GetVelocityToBrick(Brick closeTest)
        {
            Vector2 n = closeTest.ClosestEdge(position);
            Vector2 repel = closeTest.GetPlayerDir(position);

            return repel;
        }

        public void SetVelocityToPlayer(float playersDistance, Player other)
        {
            velocity = PlayerToPLayerNormal
                (playersDistance, other);
        }

        public bool PlayerToObjCollision(float distance)
        {
            if (distance < this.Diam / 2)
            {
                return true;
            }
            return false;
        }

        #endregion


        public float ToPlayerDistance(Vector2 test)
        {
            float distX = Position.X - test.X;
            float distY = Position.Y - test.Y;

            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            return distance;
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
                if (keyboard.IsKeyDown(Keys.S))
                {
                    inputPos.Y += speed;
                }
                if (keyboard.IsKeyDown(Keys.W))
                {
                    inputPos.Y -= speed;
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

            texture = new Texture2D(graphics, Diam, Diam);
            Color[] colorData = new Color[Diam * Diam];

            float radius = diam / 2f;
            float diamsq = radius * radius;

            for (int x = 0; x < diam; x++)
            {
                for (int y = 0; y < diam; y++)
                {
                    int index = x * diam + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
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
