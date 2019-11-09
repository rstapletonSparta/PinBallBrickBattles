using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PinBallBattles
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player[] players = new Player[2];

        float sideSpeed = 150;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 p1Pos = new Vector2(300, 400);
            Player p1 = new Player(p1Pos, 60);
            p1.createCircleText(GraphicsDevice);
            players[0] = p1;

            Vector2 p2Pos = new Vector2(100, 400);
            Player p2 = new Player(p2Pos, 60);
            p2.createCircleText(GraphicsDevice);
            players[1] = p2;
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputUpdate(gameTime);
            bool playerToPlayerCollision = PlayerToPlayerCollision();
            if (playerToPlayerCollision)
            {
                Console.WriteLine("Hello");
            }
            base.Update(gameTime);
        }

        private bool PlayerToPlayerCollision()
        {
            float distX = players[0].Position.X - players[1].Position.X;
            float distY = players[0].Position.Y - players[1].Position.Y;
            double distance = Math.Sqrt((distX * distX) + (distY * distY));
            
            // if the distance is less than the sum of the circle's
            // radii, the circles are touching!
            Console.WriteLine(distance);
            if (distance <= players[0].Radius)
            {
                return true;
            }
            return false;
        }

        private void InputUpdate(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
                Exit();

            Vector2 player1Pos = new Vector2(); 
            if (keyboard.IsKeyDown(Keys.A))
            {
                player1Pos.X -= sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                player1Pos.X += sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            players[0].Position += player1Pos;

            if (keyboard.IsKeyDown(Keys.Space))
            {

            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach(Player p in players)
            {
                spriteBatch.Draw(p.Texture,p.Position, null, Color.Blue,
                    0f, Vector2.Zero,//new Vector2(map.LoadedChunks.Texture.Width / 2, map.LoadedChunks.Texture.Height / 2),
                    Vector2.One, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
