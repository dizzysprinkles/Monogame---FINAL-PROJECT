using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Monogame___FINAL_PROJECT
{
    enum Screen
    {
        Title,
        Tutorial,
        Main,
        End
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //TODO: Screens except title, deal with health stuff, background, levels, etc
        //TODO: collision detection - player attacks, enemy attacks
        //TODO: if statement - if done death spritesheet, stop drawing the enemy to the screen; Whats the game name? I need a title
        //ALMOST DONE: Player detection. Now just have to make the enemies move and add a slight delay between attacks so the player doesn't die straight away.

        //DONE: player hitboxes, enemy spritesheets, enemy hitboxes, title screen

        Screen screen;
        KeyboardState keyboardState;
        Player player;
        Slime slime;
        Plant plant;
        Orc orc;
        List<Rectangle> healthRects;
        List<Texture2D> healthTextures;

        SpriteFont titleFont, instructionFont;

        Rectangle window;

        MouseState mouseState;

        Texture2D tutorialMapTexture, firstMapTexture, secondMapTexture, thirdMapTexture, fourthMapTexture;
        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, rectangleTexture, slimeAttackTexture, slimeWalkTexture, slimeDeathTexture, signTexture;
        Texture2D plantWalkTexture, plantAttackTexture, plantDeathTexture, orcAttackTexture, orcWalkTexture, orcDeathTexture, titleBackgroundTexture;
        Rectangle playerDrawRect, playerCollisionRect, slimeDrawRect, slimeCollisionRect, playerSwordRect, plantDrawRect, plantCollisionRect, orcDrawRect, orcCollisionRect, slimeAttackRect;
        Rectangle plantAttackRect, orcAttackRect, signRect, gameRect, tutorialRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Main;
            Window.Title = "Game Title Here: Main Menu";
            healthRects = new List<Rectangle>();
            healthTextures = new List<Texture2D>();

            playerCollisionRect = new Rectangle(32,30,25,45);
            playerDrawRect = new Rectangle(20,20,50,65);
            playerSwordRect = new Rectangle(28, 45, 10, 30);

            slimeCollisionRect = new Rectangle(62, 60, 32, 30); 
            slimeDrawRect = new Rectangle(40, 40, 75, 75);
            slimeAttackRect = new Rectangle(70, 60, 23, 32); 

            plantDrawRect = new Rectangle(100, 100, 75, 75);
            plantCollisionRect = new Rectangle(115, 110, 40, 50);
            plantAttackRect = new Rectangle(125, 120, 30, 40);

            orcCollisionRect = new Rectangle(215,212,45,45);
            orcDrawRect = new Rectangle(200,200,80,80);
            orcAttackRect = new Rectangle(200, 200, 65, 55);

            window = new Rectangle(0, 0, 800, 600);

            signRect = new Rectangle(70,205,200,175);

            tutorialRect = new Rectangle(100, 320, 140, 35);
            gameRect = new Rectangle(100, 270, 140, 35);

            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            for (int x = 0; x < 125; x += 25)
            {
                healthRects.Add(new Rectangle(x, 0, 20, 20));
            }

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, rectangleTexture, playerSwordRect);
            slime = new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeCollisionRect, slimeDrawRect, slimeAttackRect, player);
            plant = new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantCollisionRect, plantDrawRect, plantAttackRect, player);
            orc = new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcCollisionRect, orcDrawRect, orcAttackRect, player);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < healthRects.Count; i++)
            {
                healthTextures.Add(Content.Load<Texture2D>("Images/heart"));
            }

            instructionFont = Content.Load<SpriteFont>("Fonts/InstructionFont");

            playerIdleTexture = Content.Load<Texture2D>("Images/characterIdle");
            playerAttackTexture = Content.Load<Texture2D>("Images/characterAttack");
            playerWalkTexture = Content.Load<Texture2D>("Images/characterWalk");

            rectangleTexture = Content.Load<Texture2D>("Images/rectangle");

            slimeAttackTexture = Content.Load<Texture2D>("Images/slimeAttacking");
            slimeWalkTexture = Content.Load<Texture2D>("Images/slimeWalk"); 
            slimeDeathTexture = Content.Load<Texture2D>("Images/slimeDying");

            plantAttackTexture = Content.Load<Texture2D>("Images/plantAttack");
            plantDeathTexture = Content.Load<Texture2D>("Images/plantDying");
            plantWalkTexture = Content.Load<Texture2D>("Images/plantWalk");

            orcAttackTexture = Content.Load<Texture2D>("Images/orcAttacking");
            orcDeathTexture = Content.Load<Texture2D>("Images/orcDeath");
            orcWalkTexture = Content.Load<Texture2D>("Images/orcWalk");

            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            tutorialMapTexture = Content.Load<Texture2D>("Images/Map Tutorial");
            firstMapTexture = Content.Load<Texture2D>("Images/Map 1");
            secondMapTexture = Content.Load<Texture2D>("Images/Map 2");
            thirdMapTexture = Content.Load<Texture2D>("Images/Map 3");
            fourthMapTexture = Content.Load<Texture2D>("Images/Map 4");

            signTexture = Content.Load<Texture2D>("Images/signTitle");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            player.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            slime.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            plant.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            orc.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (screen == Screen.Title)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (tutorialRect.Contains(mouseState.Position))
                    {
                        screen = Screen.Tutorial;
                    }
                    else if (gameRect.Contains(mouseState.Position))
                    {
                        screen = Screen.Main;
                    }
                }
            }
            else if (screen == Screen.Tutorial)
            {

            }
            else if (screen == Screen.Main)
            {
                player.Update(keyboardState, mouseState, healthTextures, healthRects);
                slime.Update(player);
                plant.Update(player);
                orc.Update(player);
            }
            else
            { 
            
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (screen == Screen.Title)
            {
                _spriteBatch.Draw(titleBackgroundTexture, window, Color.White);
                _spriteBatch.Draw(signTexture, signRect, Color.White);
                _spriteBatch.DrawString(titleFont, "TITLE HERE", new Vector2(10,0), Color.White);
                _spriteBatch.DrawString(instructionFont, "Tutorial", new Vector2(100, 330), Color.White);
                _spriteBatch.DrawString(instructionFont, "Main Game", new Vector2(100, 285), Color.White);

            }
            else if (screen == Screen.Tutorial)
            {

            }
            else if (screen == Screen.Main)
            {
                slime.Draw(_spriteBatch);

                plant.Draw(_spriteBatch, instructionFont);

                orc.Draw(_spriteBatch);

                player.Draw(_spriteBatch);
                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White);
                }

                _spriteBatch.DrawString(titleFont, $"{player.Health}", new Vector2(10, 500), Color.White);

            }
            else
            {

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
