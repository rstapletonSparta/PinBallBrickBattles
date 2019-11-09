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
        float normalRepel = 900;
        float dashRepel = 150;

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

            Vector2 p0Pos = new Vector2(200, 400);
            Player p1 = new Player(p0Pos, 60);
            p1.createCircleText(GraphicsDevice);
            players[0] = p1;

            Vector2 p1Pos = new Vector2(400, 400);
            Player p2 = new Player(p1Pos, 60);
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
                Exit();

            PlayerMovement(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// 
        ///         PLAYER MOVEMENT
        /// 
        /// </summary>
        /// <param name="gameTime"></param>

        private void PlayerMovement(GameTime gameTime)
        {
            // should handle both player movement here as they
            // interact with each other
            Vector2 movePlayerZero = new Vector2();
            Vector2 movePlayerOne = new Vector2();

            movePlayerOne += PlayerInputUpdate(gameTime);

            float playerToPlayerDistance = PlayerToPlayerDistance();
            bool playerToPlayerCollision = PlayerToPlayerCollision
                (playerToPlayerDistance);


            Vector2 playerZeroNormal = players[0].PlayerToPLayerNormal
                (gameTime, playerToPlayerDistance, players[1]);
            Vector2 playerOneNormal = players[1].PlayerToPLayerNormal
                (gameTime, playerToPlayerDistance, players[0]);

            if (playerToPlayerCollision)
            {
                movePlayerZero -= playerZeroNormal * normalRepel
                    * (float)gameTime.ElapsedGameTime.TotalSeconds;
                movePlayerOne -= playerOneNormal * normalRepel
                    * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

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

        private Vector2 PlayerInputUpdate(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            Vector2 inputPos = new Vector2(); 

            if (keyboard.IsKeyDown(Keys.A))
            {
                inputPos.X -= sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                inputPos.X += sideSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                // this will be for boost
            }

            return inputPos;
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
