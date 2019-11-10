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
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 p0Pos = new Vector2(200, 400);
            Player p1 = new Player(p0Pos, 60, true);
            p1.createCircleText(GraphicsDevice);
            players[0] = p1;

            Vector2 p1Pos = new Vector2(400, 400);
            Player p2 = new Player(p1Pos, 60, false);
            p2.createCircleText(GraphicsDevice);
            players[1] = p2;
        }
        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // GAME LOOP happens 1 per frame
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            MovePlayers(gameTime);

            base.Update(gameTime);
        }
        
        private void MovePlayers(GameTime gameTime)
        {
            float playerToPlayerDistance = PlayerToPlayerDistance();
            bool playerToPlayerCollision = PlayerToPlayerCollision
                (playerToPlayerDistance);
            
            Vector2 movePlayerZero = players[0].PlayerMovementController
                (gameTime, playerToPlayerDistance, playerToPlayerCollision, players[1]);
            Vector2 movePlayerOne = players[1].PlayerMovementController
                (gameTime, playerToPlayerDistance, playerToPlayerCollision, players[0]);

            players[0].Position += movePlayerZero;
            players[1].Position += movePlayerOne;
        }

        private bool PlayerToPlayerCollision(double dis)
        {
            if (dis <= players[0].Radius)
            {
                return true;
            }
            return false;
        }

        public float PlayerToPlayerDistance()
        {
            float distX = players[1].Position.X - players[0].Position.X;
            float distY = players[1].Position.Y - players[0].Position.Y;

            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            return distance;
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach(Player p in players)
            {
                spriteBatch.Draw(p.Texture,p.Position, null, Color.AntiqueWhite, 0f, 
                    new Vector2(p.Texture.Width / 2, p.Texture.Height / 2),
                    Vector2.One, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
