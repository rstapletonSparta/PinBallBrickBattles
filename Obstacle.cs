using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PinBallBattles
{
    class Obstacle : IStaticObject
    {
        private Vector2 position;
        private int width;
        private int height;

        public Vector2 Position { get => position; set => position = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public Obstacle(Vector2 position, int width, int height)
        {
            this.position = position;
            this.height = height;
            this.width = width;
        }
    }
}
