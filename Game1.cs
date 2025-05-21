using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Monogame___FINAL_PROJECT
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        // enemies, inventory, health, CLEAR GOAL - kill all monsters (have counter on screen)
        // Intro, tutorial, main, end screens


        KeyboardState keyboardState;

        Player player;
        List<Rectangle> healthRects;
        List<Texture2D> healthTextures;

        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, testRectTexture;
        Rectangle playerDrawRect, playerCollisionRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            healthRects = new List<Rectangle>();
            healthTextures = new List<Texture2D>();

            playerCollisionRect = new Rectangle(32,28,25,45);
            playerDrawRect = new Rectangle(20,20,50,65);
           
            for (int x = 0; x < 125; x += 25)
            {
                healthRects.Add(new Rectangle(x, 0, 20, 20));
            }

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, testRectTexture);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < healthRects.Count; i++)
            {
                healthTextures.Add(Content.Load<Texture2D>("Images/heart"));
            }

            playerIdleTexture = Content.Load<Texture2D>("Images/characterIdle");
            playerAttackTexture = Content.Load<Texture2D>("Images/characterAttack");
            playerWalkTexture = Content.Load<Texture2D>("Images/characterWalk");
            testRectTexture = Content.Load<Texture2D>("Images/rectangle");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            player.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            player.Update(keyboardState);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            for (int i = 0; i < healthRects.Count; i++)
            {
                _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
