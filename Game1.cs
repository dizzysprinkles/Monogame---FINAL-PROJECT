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
        First,
        Second,
        Win,
        Lose
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //TODO: collision detection - player attacks, enemy attacks; lists of enemies per level; Text for tutorial to guide the player 
        //TODO: if statement - if done death spritesheet, stop drawing the enemy to the screen;

        //DONE: player hitboxes, enemy spritesheets, enemy hitboxes, title screen, levle 1 walls, enemy movement and detection, title name, enemy cooldown, health opacity

    

        Screen screen;
        KeyboardState keyboardState;
        Player player;
        Slime slime;
        Plant plant;
        Orc orc;
        int monsterCountMax, monstersKilled;
        List<Rectangle> healthRects, tutorialBarriers, firstLevelBarriers, secondLevelBarriers;
        List<Texture2D> healthTextures;
        List<float> healthOpacity;

        SpriteFont titleFont, instructionFont;

        Rectangle window;

        MouseState mouseState;

        Texture2D tutorialMapTexture, firstMapTexture, secondMapTexture;
        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, rectangleTexture, slimeAttackTexture, slimeWalkTexture, slimeDeathTexture, signTexture, slimeIdleTexture;
        Texture2D plantWalkTexture, plantAttackTexture, plantDeathTexture, orcAttackTexture, orcWalkTexture, orcDeathTexture, titleBackgroundTexture, plantIdleTexture, orcIdleTexture;
        Rectangle playerDrawRect, playerCollisionRect, slimeDrawRect, slimeCollisionRect, playerSwordRect, plantDrawRect, plantCollisionRect, orcDrawRect, orcCollisionRect;
        Rectangle signRect, gameButtonRect, tutorialButtonRect, tutorialBackgroundRect, descentRect, slimeWalkRect, plantWalkRect, orcWalkRect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = Screen.Title;
            Window.Title = "Dungeon Mayhem: Main Menu";
            healthRects = new List<Rectangle>();
            healthTextures = new List<Texture2D>();
            tutorialBarriers = new List<Rectangle>();
            healthOpacity = new List<float>();
            firstLevelBarriers = new List<Rectangle>();
            secondLevelBarriers= new List<Rectangle>(); // Still need to add a bunch of rectangles

            tutorialBarriers.Add(new Rectangle(35, 35, 45, 500));
            tutorialBarriers.Add(new Rectangle(35, 35, 725, 30));
            tutorialBarriers.Add(new Rectangle(712, 35, 47, 500));
            tutorialBarriers.Add(new Rectangle(35, 495, 725, 30));

            firstLevelBarriers.Add(new Rectangle(0, 195, 28, 400));
            firstLevelBarriers.Add(new Rectangle(772, 195, 28, 400));
            firstLevelBarriers.Add(new Rectangle(65, 568, 725, 28));
            firstLevelBarriers.Add(new Rectangle(28, 538, 60, 50));
            firstLevelBarriers.Add(new Rectangle(210, 5, 325, 15));
            firstLevelBarriers.Add(new Rectangle(28, 195, 525, 10));
            firstLevelBarriers.Add(new Rectangle(63, 442, 375, 10));
            firstLevelBarriers.Add(new Rectangle(210, 380, 215, 68));
            firstLevelBarriers.Add(new Rectangle(205, 10, 28, 185));
            firstLevelBarriers.Add(new Rectangle(500, 10, 28, 93));
            firstLevelBarriers.Add(new Rectangle(500, 162, 28, 40));
            firstLevelBarriers.Add(new Rectangle(415, 302, 28, 280));
            firstLevelBarriers.Add(new Rectangle(413, 200, 28, 40));
            firstLevelBarriers.Add(new Rectangle(28, 378, 148, 10));
            firstLevelBarriers.Add(new Rectangle(63, 442, 27, 30));
            firstLevelBarriers.Add(new Rectangle(625, 195, 200, 10));
            firstLevelBarriers.Add(new Rectangle(530, 70, 200, 10));
            firstLevelBarriers.Add(new Rectangle(680, 70, 28, 130));

            playerCollisionRect = new Rectangle(200,260,25,45); 
            playerDrawRect = new Rectangle(20,20,50,65);
            playerSwordRect = new Rectangle(196, 275, 10, 30);

            slimeCollisionRect = new Rectangle(62, 260, 32, 30); 
            slimeDrawRect = new Rectangle(40, 40, 75, 75);
            slimeWalkRect = new Rectangle(70, 260, 16, 30);

            plantDrawRect = new Rectangle(100, 100, 75, 75);
            plantCollisionRect = new Rectangle(115, 290, 40, 50);
            plantWalkRect = new Rectangle(127, 290, 16, 50);

            orcCollisionRect = new Rectangle(220,300,45,45); 
            orcDrawRect = new Rectangle(212,288,65,80);
            orcWalkRect = new Rectangle(226, 300,17, 45 );

            descentRect = new Rectangle(320, 207, 53, 60);

            window = new Rectangle(0, 0, 800, 600);

            signRect = new Rectangle(70,205,200,175);

            monsterCountMax = 1;
            monstersKilled = 0;

            tutorialButtonRect = new Rectangle(100, 320, 140, 35);
            tutorialBackgroundRect = new Rectangle(30,30, 750, 550);
            gameButtonRect = new Rectangle(100, 270, 140, 35);

            _graphics.PreferredBackBufferHeight = window.Height;
            _graphics.PreferredBackBufferWidth = window.Width;
            _graphics.ApplyChanges();

            for (int x = 0; x < 125; x += 25)
            {
                healthRects.Add(new Rectangle(x, 0, 20, 20));
            }

            for (int i = 0; i < healthRects.Count; i++)
            {
                healthOpacity.Add(1f);
            }

            base.Initialize();
            player = new Player(playerIdleTexture, playerWalkTexture, playerAttackTexture, playerCollisionRect, playerDrawRect, rectangleTexture, playerSwordRect);
            slime = new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture);
            plant = new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantCollisionRect, plantDrawRect, player, plantWalkRect, plantIdleTexture);
            orc = new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcCollisionRect, orcDrawRect, player, orcIdleTexture, orcWalkRect);
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
            slimeIdleTexture = Content.Load<Texture2D>("Images/slimeIdle");

            plantAttackTexture = Content.Load<Texture2D>("Images/plantAttack");
            plantDeathTexture = Content.Load<Texture2D>("Images/plantDying");
            plantWalkTexture = Content.Load<Texture2D>("Images/plantWalk");
            plantIdleTexture = Content.Load<Texture2D>("Images/plantIdle");

            orcAttackTexture = Content.Load<Texture2D>("Images/orcAttacking");
            orcDeathTexture = Content.Load<Texture2D>("Images/orcDeath");
            orcWalkTexture = Content.Load<Texture2D>("Images/orcWalk");
            orcIdleTexture = Content.Load<Texture2D>("Images/orcIdle");

            titleBackgroundTexture = Content.Load<Texture2D>("Images/titleBackground");
            titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");

            tutorialMapTexture = Content.Load<Texture2D>("Images/Map Tutorial");
            firstMapTexture = Content.Load<Texture2D>("Images/firstMap");
            secondMapTexture = Content.Load<Texture2D>("Images/secondMap");

            signTexture = Content.Load<Texture2D>("Images/signTitle");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            player.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            slime.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            slime.AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            plant.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            plant.AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            orc.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            orc.AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (screen == Screen.Title)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (tutorialButtonRect.Contains(mouseState.Position))
                    {
                        Window.Title = "Dungeon Mayhem: Tutorial";
                        screen = Screen.Tutorial;
                    }
                    else if (gameButtonRect.Contains(mouseState.Position))
                    {
                        monstersKilled = 0;
                        monsterCountMax = 3;
                        descentRect = new Rectangle(440, 160, 35, 35);
                        Window.Title = "Dungeon Mayhem: Level One";
                        orc.AttackCooldown = 0.65f;
                        slime.AttackCooldown = orc.AttackCooldown;
                        plant.AttackCooldown = orc.AttackCooldown;
                        screen = Screen.First;
                    }
                }
            }
            else if (screen == Screen.Tutorial)
            {
                player.Update(keyboardState, mouseState, healthTextures, healthRects, firstLevelBarriers, healthOpacity, orc, plant, slime);
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        monsterCountMax = 3;
                        descentRect = new Rectangle(440, 160, 35, 35);
                        monstersKilled = 0;
                        Window.Title = "Dungeon Mayhem: Level One";
                        player.Health = 10;
                        orc.AttackCooldown = 0.65f;
                        slime.AttackCooldown = orc.AttackCooldown;
                        plant.AttackCooldown = orc.AttackCooldown;
                        screen = Screen.First;
                    }
                }
            }
            else if (screen == Screen.First)
            {
                //slime.Update(player, firstLevelBarriers);
                plant.Update(player, firstLevelBarriers);
                //orc.Update(player, firstLevelBarriers);
                player.Update(keyboardState, mouseState, healthTextures, healthRects, firstLevelBarriers, healthOpacity, orc, plant, slime);

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        monsterCountMax = 6;
                        monstersKilled = 0;
                        descentRect = new Rectangle(665, 35, 35, 35);
                        Window.Title = "Dungeon Mayhem: Level Two";
                        player.Health = 10;
                        orc.AttackCooldown = 0.45f;
                        slime.AttackCooldown = orc.AttackCooldown;
                        plant.AttackCooldown = orc.AttackCooldown;
                        screen = Screen.Second;
                    }
                }
                if (player.Health <= 0)
                {
                    Window.Title = "Dungeon Mayhem: You Lose";
                    screen = Screen.Lose;
                }

            }
            else if (screen == Screen.Second)
            {
                slime.Update(player, secondLevelBarriers);
                plant.Update(player, secondLevelBarriers);
                orc.Update(player, secondLevelBarriers);
                player.Update(keyboardState, mouseState, healthTextures, healthRects, secondLevelBarriers, healthOpacity, orc, plant, slime);

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        Window.Title = "Dungeon Mayhem: You Win";
                        screen = Screen.Win;
                    }
                }

                if (player.Health <= 0)
                {
                    Window.Title = "Dungeon Mayhem: You Lose";
                    screen = Screen.Lose;
                }

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
            GraphicsDevice.Clear(Color.Black);

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
                _spriteBatch.Draw(tutorialMapTexture, tutorialBackgroundRect, Color.White);
               
                player.Draw(_spriteBatch);

            }
            else if (screen == Screen.First)
            {
                _spriteBatch.Draw(firstMapTexture, window, Color.White);
                slime.Draw(_spriteBatch);

                plant.Draw(_spriteBatch);

                orc.Draw(_spriteBatch);

                player.Draw(_spriteBatch);
               

                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White * healthOpacity[i]);
                }

            }
            else if(screen == Screen.Second) 
            {
                _spriteBatch.Draw(secondMapTexture, window, Color.White);
                slime.Draw(_spriteBatch);

                plant.Draw(_spriteBatch);

                orc.Draw(_spriteBatch);

                player.Draw(_spriteBatch);


                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White * healthOpacity[i]);
                }

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
