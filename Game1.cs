using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame___FINAL_PROJECT
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Vague idea:
        // Dungeon game -  top-down RPG 
        // enemies, inventory?, health, CLEAR GOAL - kill all monsters (have counter on screen)
        // Intro, tutorial, main, end screens

        // TO LEARN: Enemy Field of view - Angular motion tutorial has resources; Inventory - learn on own/video tutorials; 
        // Would screen have to move?

        KeyboardState keyboardState;

        Player player;
        //float time;
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
            playerCollisionRect = new Rectangle(32,28,25,45);
            playerDrawRect = new Rectangle(20,20,50,65);
           

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, testRectTexture);

            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
