using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PinBallBattles
{
    class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private int radius;

        public Texture2D Texture { get => texture; }
        public Vector2 Position { get => position; set => position = value; }
        public int Radius { get => radius; }

        public Player(Vector2 position, int radius)
        {
            this.position = position;
            this.radius = radius;
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
