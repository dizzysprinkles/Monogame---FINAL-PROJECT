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
        // enemies,  health, CLEAR GOAL - kill all monsters (have counter on screen)
        // NO INVENTORY or POWERUPS - no enough time...
        // Intro, tutorial, main, end screens
        // detection - turn centres of each to points, find magnitude between them, if => radius then attack/move; should be built into vector class

        //TODO: Screens, deal with health stuff, background, movement, levels?, etc


        KeyboardState keyboardState;
        Player player;
        Slime slime;
        List<Rectangle> healthRects;
        List<Texture2D> healthTextures;

        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, rectangleTexture, slimeAttackTexture, slimeWalkTexture, slimeDeathTexture;
        Texture2D plantWalkTexture, plantAttackTexture, plantDeathTexture;
        Rectangle playerDrawRect, playerCollisionRect, slimeDrawRect, slimeCollisionRect, playerSwordRect;
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

            playerCollisionRect = new Rectangle(32,30,25,45);
            playerDrawRect = new Rectangle(20,20,50,65);
            slimeCollisionRect = new Rectangle(75, 72, 25, 25); 
            slimeDrawRect = new Rectangle(40, 40, 75, 75);
            playerSwordRect = new Rectangle(28, 45, 10, 35 );
           
            for (int x = 0; x < 125; x += 25)
            {
                healthRects.Add(new Rectangle(x, 0, 20, 20));
            }

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, rectangleTexture, playerSwordRect);
            slime = new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeCollisionRect, slimeDrawRect);
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
            rectangleTexture = Content.Load<Texture2D>("Images/rectangle");
            slimeAttackTexture = Content.Load<Texture2D>("Images/slimeAttacking");
            slimeWalkTexture = Content.Load<Texture2D>("Images/slimeWalks"); // Still have to fix again.....
            slimeDeathTexture = Content.Load<Texture2D>("Images/slimeDying");
            plantAttackTexture = Content.Load<Texture2D>("Images/plantAttack");
            plantDeathTexture = Content.Load<Texture2D>("Images/plantDying");
            plantWalkTexture = Content.Load<Texture2D>("Images/plantWalking");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            player.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            slime.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            player.Update(keyboardState);
            slime.Update();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            player.Draw(_spriteBatch);
            slime.Draw(_spriteBatch);
            for (int i = 0; i < healthRects.Count; i++)
            {
                _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
