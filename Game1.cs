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
        Brick[] bricks = new Brick[2];
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Vector2 p0Pos = new Vector2(200, 410);
            Player p1 = new Player(p0Pos, 60, true);
            p1.createCircleText(GraphicsDevice);
            players[0] = p1;

            Vector2 p1Pos = new Vector2(400, 400);
            Player p2 = new Player(p1Pos, 60, false);
            p2.createCircleText(GraphicsDevice);
            players[1] = p2;

            Vector2 brick1Pos = new Vector2(15, 450);
            Brick b1 = new Brick(brick1Pos, 30, 900);
            b1.CreateSquareText(GraphicsDevice);
            bricks[0] = b1;

            Vector2 brickpos = new Vector2(585, 450);
            Brick b = new Brick(brickpos, 30, 900);
            b.CreateSquareText(GraphicsDevice);
            bricks[1] = b;
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
        bool wallCol;
        private void MovePlayers(GameTime gameTime)
        {
            float playerToPlayerDistance = ToPlayerDistance(players[0], players[1].Position);
            bool playerToPlayerCollision = PlayerToPlayerCollision
                (playerToPlayerDistance, players[0]);

            if (playerToPlayerCollision)
            {
                CollisionStateChange(playerToPlayerDistance);
            }

            foreach(Player p in players)
            {
                Player other;
                if (p != players[0])
                    other = players[0];
                else
                    other = players[1];

                foreach(Brick b in bricks)
                {
                    Vector2 t = b.ClosestEdge(p);
                    float dis = ToPlayerDistance(p, t);
                    wallCol = PlayerToPlayerCollision(dis, p);
                }
                Console.WriteLine(wallCol);

                Vector2 movePlayer = p.PlayerMovementController
                    (gameTime, playerToPlayerDistance, playerToPlayerCollision, other, graphics);
                if (!wallCol)
                {
                    p.Position += movePlayer;

                }
            }
        }

        private void CollisionStateChange(float playDistance)
        {
            if (players[0].MoveState == MovementStates.Dash
                && players[1].MoveState == MovementStates.Dash)
            {
                players[0].MoveState = MovementStates.ShortKnockBack;
                players[1].MoveState = MovementStates.ShortKnockBack;
                players[0].SetKnockbackDirection(playDistance, players[1]);
                players[1].SetKnockbackDirection(playDistance, players[0]);
            }
            else if (players[0].MoveState == MovementStates.Dash
                && players[1].MoveState == MovementStates.Normal)
            {
                players[0].MoveState = MovementStates.ShortKnockBack;
                players[1].MoveState = MovementStates.LongKnockBack;
                players[0].SetKnockbackDirection(playDistance, players[1]);
                players[1].SetKnockbackDirection(playDistance, players[0]);
            }
            else if (players[0].MoveState == MovementStates.Normal
                && players[1].MoveState == MovementStates.Dash)
            {
                players[0].MoveState = MovementStates.LongKnockBack;
                players[1].MoveState = MovementStates.ShortKnockBack;
                players[0].SetKnockbackDirection(playDistance, players[1]);
                players[1].SetKnockbackDirection(playDistance, players[0]);
            }
            else
            {
                players[0].MoveState = MovementStates.Normal;
                players[1].MoveState = MovementStates.Normal;
            }
        }

        private bool PlayerToPlayerCollision(double dis, Player player)
        {
            if (dis <= player.Radius / 2)
            {
                return true;
            }
            return false;
        }

        private float ToPlayerDistance(Player player, Vector2 test)
        {
            float distX = player.Position.X - test.X;
            float distY = player.Position.Y - test.Y;

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

            foreach (Brick b in bricks)
            {
                spriteBatch.Draw(b.Texture, b.Position, null, Color.AntiqueWhite, 0f,
                        new Vector2(b.Texture.Width / 2, b.Texture.Height / 2),
                        Vector2.One, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
