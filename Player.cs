using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace PinBallBattles
{
    class Player
    {
        private bool test = false;

        private Texture2D texture;
        private Vector2 position;
        private int radius;
        private bool dash = false;
        private float sideSpeed = 150;
        private float dashSpeed = 1500;
        private float normalRepel = 900;
        private float dashRepel = 500;

        public Texture2D Texture { get => texture; }
        public Vector2 Position { get => position; set => position = value; }
        public int Radius { get => radius; }
        public bool Dash { get => dash; set => dash = value; }

        public Player(Vector2 position, int radius, bool test)
        {
            this.position = position;
            this.radius = radius;
            this.test = test;
        }

        public Vector2 PlayerMovement
            (GameTime gameTime, float playersDistance, bool playersCollision, Player other)
        {
            Vector2 movePos = new Vector2();
            Vector2 normal = PlayerToPLayerNormal
                (gameTime, playersDistance, other);
            
            movePos += PlayerInputUpdate(gameTime);
            
            if (playersCollision)
            {
                movePos -= normal * normalRepel
                    * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            return movePos;
        }

        public Vector2 KnockBack()
        {


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

        private Vector2 PlayerInputUpdate(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            Vector2 inputPos = new Vector2();
            if (test)
            {
                if (keyboard.IsKeyDown(Keys.A))
                {
                    if (dash)
                    {
                        inputPos.X -= dashSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        inputPos.X -= sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                if (keyboard.IsKeyDown(Keys.D))
                {
                    if (dash)
                    {
                        inputPos.X += dashSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        inputPos.X += sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (keyboard.IsKeyDown(Keys.Space))
                {
                    dash = true;
                }
                else
                {
                    dash = false;
                }
            }

            return inputPos;
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
