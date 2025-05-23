﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        // detection - turn centres of each to points, find magnitude between them, if => radius then attack/move; should be built into vector class

        //TODO: Screens, deal with health stuff, background, movement, levels?, etc

        Screen screen;
        KeyboardState keyboardState;
        Player player;
        Slime slime;
        Plant plant;
        Orc orc;
        List<Rectangle> healthRects;
        List<Texture2D> healthTextures;

        MouseState mouseState;

        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, rectangleTexture, slimeAttackTexture, slimeWalkTexture, slimeDeathTexture;
        Texture2D plantWalkTexture, plantAttackTexture, plantDeathTexture, orcAttackTexture, orcWalkTexture, orcDeathTexture;
        Rectangle playerDrawRect, playerCollisionRect, slimeDrawRect, slimeCollisionRect, playerSwordRect, plantDrawRect, plantCollisionRect, orcDrawRect, orcCollisionRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Title;
            healthRects = new List<Rectangle>();
            healthTextures = new List<Texture2D>();

            playerCollisionRect = new Rectangle(32,30,25,45);
            playerDrawRect = new Rectangle(20,20,50,65);
            slimeCollisionRect = new Rectangle(62, 60, 32, 30); 
            slimeDrawRect = new Rectangle(40, 40, 75, 75);
            playerSwordRect = new Rectangle(28, 45, 10, 35 );

            plantDrawRect = new Rectangle(100, 100, 75, 75);
            plantCollisionRect = new Rectangle(115, 110, 40, 50);

            orcCollisionRect = new Rectangle(215,212,45,45);
            orcDrawRect = new Rectangle(200,200,80,80);

           
            for (int x = 0; x < 125; x += 25)
            {
                healthRects.Add(new Rectangle(x, 0, 20, 20));
            }

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, rectangleTexture, playerSwordRect);
            slime = new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeCollisionRect, slimeDrawRect);
            plant = new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantCollisionRect, plantDrawRect);
            orc = new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcCollisionRect, orcDrawRect);
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
            slimeWalkTexture = Content.Load<Texture2D>("Images/slimeWalks"); 
            slimeDeathTexture = Content.Load<Texture2D>("Images/slimeDying");

            plantAttackTexture = Content.Load<Texture2D>("Images/plantAttack");
            plantDeathTexture = Content.Load<Texture2D>("Images/plantDying");
            plantWalkTexture = Content.Load<Texture2D>("Images/plantWalking");

            orcAttackTexture = Content.Load<Texture2D>("Images/orcAttack");
            orcDeathTexture = Content.Load<Texture2D>("Images/orcDeath");
            orcWalkTexture = Content.Load<Texture2D>("Images/orcWalk");
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

            }
            else if (screen == Screen.Tutorial)
            {



            }
            else if (screen == Screen.Main)
            {
                player.Update(keyboardState, mouseState, healthTextures, healthRects);
                slime.Update();
                plant.Update();
                orc.Update();
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

            }
            else if (screen == Screen.Tutorial)
            {

            }

            else if (screen == Screen.Main)
            {
                slime.Draw(_spriteBatch);

                plant.Draw(_spriteBatch);

                orc.Draw(_spriteBatch);

                player.Draw(_spriteBatch);
                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White);
                }

            }
            else
            {





            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
