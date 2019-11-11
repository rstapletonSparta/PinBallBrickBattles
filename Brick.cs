using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PinBallBattles
{
    class Brick : Obstacle
    {
        private Texture2D texture;

        public Texture2D Texture { get => texture; }

        public Brick(Vector2 position, int width, int height) : base(position, width, height)
        {

        }

        public Vector2 ClosestEdge(Player p)
        {
            Vector2 r = new Vector2(p.Position.X, p.Position.Y);

            if (p.Position.X < Position.X - Width / 2)
            {
                r.X = Position.X - Width / 2;
            }
            else if (p.Position.X > Position.X + Width / 2)
            {
                r.X = Position.X + Width / 2;
            }
            if (p.Position.Y < Position.Y - Height / 2)
            {
                r.Y = Position.Y - Height / 2;
            }
            else if (p.Position.Y > Position.Y + Height / 2)
            {
                r.Y = Position.Y + Height / 2;
            }
            return r;
        }

        public void CreateSquareText(GraphicsDevice graphics)
        {
            // this guy has got radius and diamiter backwards
            texture = new Texture2D(graphics, Width, Height);
            Color[] colorData = new Color[Width * Height];
            int index = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    colorData[index] = Color.White;
                    index++;
                }
            }
            texture.SetData(colorData);
        }
    }
}
