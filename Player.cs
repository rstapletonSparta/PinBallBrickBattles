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

        public void createCircleText(GraphicsDevice graphics)
        {
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
