using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Encodings.Web;

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
        //TODO: Overhaul sword collision box and just switch it to bix hitbox for easier detection

        //TODO: End screen buttons, health potion, instruction/dialogue in levels?, SOUND EFFECTSSSS

        Screen screen;
        KeyboardState keyboardState;
        Player player;
        int monsterCountMax, monstersKilled, clickCounter;
        List<Rectangle> healthRects, tutorialBarriers, firstLevelBarriers, secondLevelBarriers;
        List<Texture2D> healthTextures;
        List<float> healthOpacity;
        List<Slime> slimes;
        List<Orc> orcs;
        List<Plant> plants;

        string instructionText;

        SpriteFont titleFont, instructionFont, counterFont;
        Song titleSong, tutorialSong, firstLevelSong, secondLevelSong, deadSong, surviveSong;

        Rectangle window;

        MouseState mouseState, prevMouseState;

        Texture2D tutorialMapTexture, firstMapTexture, secondMapTexture, winBackgroundTexture, loseBackgroundTexture, instructionBoxTexture;
        Texture2D playerIdleTexture, playerWalkTexture, playerAttackTexture, rectangleTexture, slimeAttackTexture, slimeWalkTexture, slimeDeathTexture, signTexture, slimeIdleTexture;
        Texture2D plantWalkTexture, plantAttackTexture, plantDeathTexture, orcAttackTexture, orcWalkTexture, orcDeathTexture, titleBackgroundTexture, plantIdleTexture, orcIdleTexture;
        Rectangle playerDrawRect, playerCollisionRect, slimeDrawRect, slimeTutorialCollisionRect, playerSwordRect, plantDrawRect, plantFirstCollisionRect, orcDrawRect, orcFirstCollisionRect;
        Rectangle signRect, gameButtonRect, tutorialButtonRect, tutorialBackgroundRect, descentRect, slimeWalkRect, plantWalkRect, orcWalkRect, instructionBoxRect;
        Rectangle orcSecondCollisionRect, plantSecondCollisionRect, slimeFirstCollisionRect, slimeSecondCollisionRect, orcThirdCollisionRect, plantThirdCollisionRect, slimeThirdCollisionRect;
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

            instructionText = "Welcome to the Dungeon! \n\nYour goal is to kill every monster on each floor and \n\ndescend until you reach the bottom!";

            orcs = new List<Orc>();
            slimes = new List<Slime>();
            plants = new List<Plant>();

            healthRects = new List<Rectangle>();
            healthTextures = new List<Texture2D>();
            tutorialBarriers = new List<Rectangle>();
            healthOpacity = new List<float>();
            firstLevelBarriers = new List<Rectangle>();
            secondLevelBarriers= new List<Rectangle>(); 

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

            secondLevelBarriers.Add(new Rectangle(7, 45, 28, 400));
            secondLevelBarriers.Add(new Rectangle(765, 0, 28, 475));
            secondLevelBarriers.Add(new Rectangle(198, 257, 26, 400));
            secondLevelBarriers.Add(new Rectangle(450, 45, 24, 345));
            secondLevelBarriers.Add(new Rectangle(513, 10, 24, 150));
            secondLevelBarriers.Add(new Rectangle(513, 215, 24, 115));
            secondLevelBarriers.Add(new Rectangle(513, 382, 24, 133));
            secondLevelBarriers.Add(new Rectangle(575, 257, 24, 200 ));
            secondLevelBarriers.Add(new Rectangle(635, 445, 24, 150));
            secondLevelBarriers.Add(new Rectangle(422, 510, 24, 100));
            secondLevelBarriers.Add(new Rectangle(70, 255, 28, 400));
            secondLevelBarriers.Add(new Rectangle(38, 255, 32, 400));
            secondLevelBarriers.Add(new Rectangle(475, 0, 40, 142));
            secondLevelBarriers.Add(new Rectangle(535, 255, 40, 77));
            secondLevelBarriers.Add(new Rectangle(0, 52, 450, 28));
            secondLevelBarriers.Add(new Rectangle(330, 0, 450, 16));
            secondLevelBarriers.Add(new Rectangle(600, 445, 450, 13));
            secondLevelBarriers.Add(new Rectangle(158, 255, 200, 12));
            secondLevelBarriers.Add(new Rectangle(417, 255, 58, 12));
            secondLevelBarriers.Add(new Rectangle(595, 255, 68, 12));
            secondLevelBarriers.Add(new Rectangle(724, 255, 68, 12));
            secondLevelBarriers.Add(new Rectangle(200, 382, 117, 12));
            secondLevelBarriers.Add(new Rectangle(381, 382, 135, 12));
            secondLevelBarriers.Add(new Rectangle(512, 508, 87, 12));
            secondLevelBarriers.Add(new Rectangle(0, 510, 447, 28));
            secondLevelBarriers.Add(new Rectangle(0, 572, 800, 28));

            playerCollisionRect = new Rectangle(200,260,25,45); 
            playerDrawRect = new Rectangle(20,20,50,65);
            playerSwordRect = new Rectangle(196, 275, 10, 30);

            slimeTutorialCollisionRect = new Rectangle(562, 260, 32, 30);
            slimeFirstCollisionRect = new Rectangle(600, 450, 32, 30);
            slimeSecondCollisionRect = new Rectangle(300, 450, 32, 30);
            slimeThirdCollisionRect = new Rectangle(100, 100, 32, 30);
            slimeDrawRect = new Rectangle(40, 40, 75, 75);
            slimeWalkRect = new Rectangle(70, 260, 16, 30);

            plantDrawRect = new Rectangle(100, 100, 75, 75);
            plantFirstCollisionRect = new Rectangle(215, 490, 40, 50);
            plantSecondCollisionRect = new Rectangle(600, 100, 40, 50);
            plantThirdCollisionRect = new Rectangle(350, 310, 40, 50);
            plantWalkRect = new Rectangle(127, 290, 16, 50);

            orcFirstCollisionRect = new Rectangle(350,100,45,45);
            orcSecondCollisionRect = new Rectangle(115, 360, 45,45);
            orcThirdCollisionRect = new Rectangle(675, 310, 45, 45);
            orcDrawRect = new Rectangle(212,288,65,80);
            orcWalkRect = new Rectangle(226, 300,17, 45 );

            descentRect = new Rectangle(320, 207, 53, 60);

            instructionBoxRect = new Rectangle(0, 450, 800, 150);

            window = new Rectangle(0, 0, 800, 600);

            signRect = new Rectangle(70,205,200,175);

            clickCounter = 0;

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
            slimes.Add(new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeTutorialCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture));
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
            counterFont = Content.Load<SpriteFont>("Fonts/CounterFont");

            tutorialMapTexture = Content.Load<Texture2D>("Images/Map Tutorial");
            firstMapTexture = Content.Load<Texture2D>("Images/firstMap");
            secondMapTexture = Content.Load<Texture2D>("Images/secondMap");
            winBackgroundTexture = Content.Load<Texture2D>("Images/winBackground");
            loseBackgroundTexture = Content.Load<Texture2D>("Images/loseBackground");

            signTexture = Content.Load<Texture2D>("Images/signTitle");
            instructionBoxTexture = Content.Load<Texture2D>("Images/instructionBox");

            titleSong = Content.Load<Song>("SoundFX/title");
            tutorialSong = Content.Load<Song>("SoundFX/tutorial");
            firstLevelSong = Content.Load<Song>("SoundFX/level1");
            secondLevelSong = Content.Load<Song>("SoundFX/level2");
            surviveSong = Content.Load<Song>("SoundFX/win");
            deadSong = Content.Load<Song>("SoundFX/lose");

            MediaPlayer.Volume = 0.8f;
            MediaPlayer.Play(titleSong);
            MediaPlayer.IsRepeating = true;
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouseState = mouseState;
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            player.Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < slimes.Count; i++)
            {
                slimes[i].Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                slimes[i].AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            for (int i = 0; i < orcs.Count; i++)
            {
                orcs[i].Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                orcs[i].AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            for (int i = 0; i < plants.Count; i++)
            {
                plants[i].Time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                plants[i].AttackTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


            if (screen == Screen.Title)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (tutorialButtonRect.Contains(mouseState.Position))
                    {
                        Window.Title = "Dungeon Mayhem: Tutorial";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(tutorialSong);
                        MediaPlayer.Volume = 0.7f;
                        screen = Screen.Tutorial;
                    }
                    else if (gameButtonRect.Contains(mouseState.Position))
                    {
                        monstersKilled = 0;
                        monsterCountMax = 3;
                        descentRect = new Rectangle(440, 160, 35, 35);
                        Window.Title = "Dungeon Mayhem: Level One";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(firstLevelSong);
                        MediaPlayer.Volume = 0.3f;
                        slimes.Clear();
                        slimes.Add(new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeFirstCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture));
                        orcs.Add(new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcFirstCollisionRect, orcDrawRect, player, orcIdleTexture, orcWalkRect));
                        plants.Add(new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantFirstCollisionRect, plantDrawRect, player, plantWalkRect, plantIdleTexture));
                        for (int i = 0; i < slimes.Count; i++)
                        {
                            slimes[i].AttackCooldown = 0.65f;
                            orcs[i].AttackCooldown = 0.65f;
                            plants[i].AttackCooldown = 0.65f;
                        }
                        screen = Screen.First;
                    }
                }
            }
            else if (screen == Screen.Tutorial)
            {
                EnemiesUpdate(orcs, slimes, plants, tutorialBarriers, player);

                player.Update(keyboardState, healthTextures, healthRects, tutorialBarriers, healthOpacity, slimes, orcs, plants);

                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    clickCounter++;

                    if (clickCounter == 1)
                    {
                        instructionText = "You can you WASD or the ARROW KEYS to move around. \n\nTo fight, press the SPACEBAR to swing your sword! \n\nTry to kill the slime over there!";
                    }
                    else if (clickCounter >= 2 && monstersKilled == 1)
                    {
                        instructionText = "Great job! You can now descend to Level 1! \n\nStand on the stairs and LEFT-CLICK to descend.";
                    }

                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        monsterCountMax = 3;
                        monstersKilled = 0;
                        descentRect = new Rectangle(440, 160, 35, 35);
                        monstersKilled = 0;
                        Window.Title = "Dungeon Mayhem: Level One";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(firstLevelSong);
                        MediaPlayer.Volume = 0.3f;
                        player.Health = 10;
                        slimes.Clear();
                        slimes.Add(new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeFirstCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture));
                        orcs.Add(new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcFirstCollisionRect, orcDrawRect, player, orcIdleTexture, orcWalkRect));
                        plants.Add(new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantFirstCollisionRect, plantDrawRect, player, plantWalkRect, plantIdleTexture));

                        for (int i = 0; i < slimes.Count; i++)
                        {
                            slimes[i].AttackCooldown = 0.65f;
                            orcs[i].AttackCooldown = 0.65f;
                            plants[i].AttackCooldown = 0.65f;
                        }
         
                        screen = Screen.First;
                    }
                }
            }
            else if (screen == Screen.First)
            {
                EnemiesUpdate(orcs, slimes, plants, firstLevelBarriers, player);

                player.Update(keyboardState, healthTextures, healthRects, firstLevelBarriers, healthOpacity, slimes, orcs, plants);
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        monsterCountMax = 6;
                        monstersKilled = 0;
                        descentRect = new Rectangle(665, 35, 35, 35);
                        Window.Title = "Dungeon Mayhem: Level Two";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(secondLevelSong);
                        MediaPlayer.Volume = 0.3f;
                        player.Health = 10;
                        slimes.Clear();
                        orcs.Clear();
                        plants.Clear();
                       
                        slimes.Add(new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeSecondCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture));
                        orcs.Add(new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcSecondCollisionRect, orcDrawRect, player, orcIdleTexture, orcWalkRect));
                        plants.Add(new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantSecondCollisionRect, plantDrawRect, player, plantWalkRect, plantIdleTexture));
                        slimes.Add(new Slime(slimeDeathTexture, slimeWalkTexture, slimeAttackTexture, rectangleTexture, slimeThirdCollisionRect, slimeDrawRect, player, slimeWalkRect, slimeIdleTexture));
                        orcs.Add(new Orc(orcDeathTexture, orcWalkTexture, orcAttackTexture, rectangleTexture, orcThirdCollisionRect, orcDrawRect, player, orcIdleTexture, orcWalkRect));
                        plants.Add(new Plant(plantDeathTexture, plantWalkTexture, plantAttackTexture, rectangleTexture, plantThirdCollisionRect, plantDrawRect, player, plantWalkRect, plantIdleTexture));
                        for (int i = 0; i < slimes.Count; i++)
                        {
                            slimes[i].AttackCooldown = 0.45f;
                            orcs[i].AttackCooldown = 0.45f;
                            plants[i].AttackCooldown = 0.45f;
                        }
                        screen = Screen.Second;
                        player.Rectangle = new Rectangle(400, 100, 25, 45);
                        player.Location = new Vector2(400, 100);
                    }
                }
                if (player.Health <= 0)
                {
                    Window.Title = "Dungeon Mayhem: You Lose";
                    MediaPlayer.Stop();
                    MediaPlayer.Play(deadSong);
                    MediaPlayer.Volume = 0.7f;
                    screen = Screen.Lose;
                }

            }
            else if (screen == Screen.Second)
            {
                EnemiesUpdate(orcs, slimes, plants, secondLevelBarriers, player);
                player.Update(keyboardState, healthTextures, healthRects, secondLevelBarriers, healthOpacity, slimes, orcs, plants);
                
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (player.Intersects(descentRect) && monstersKilled == monsterCountMax)
                    {
                        Window.Title = "Dungeon Mayhem: You Win";
                        MediaPlayer.Stop();
                        MediaPlayer.Play(surviveSong);
                        MediaPlayer.Volume = 0.7f;
                        screen = Screen.Win;
                    }
                }

                if (player.Health <= 0)
                {
                    Window.Title = "Dungeon Mayhem: You Lose";
                    MediaPlayer.Stop();
                    MediaPlayer.Play(deadSong);
                    MediaPlayer.Volume = 0.7f;
                    screen = Screen.Lose;
                }

            }
            else if (screen == Screen.Win)
            { 
            
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
                _spriteBatch.DrawString(titleFont, "Dungeon Mayhem", new Vector2(10,0), Color.White);
                _spriteBatch.DrawString(instructionFont, "Tutorial", new Vector2(100, 330), Color.White);
                _spriteBatch.DrawString(instructionFont, "Main Game", new Vector2(100, 285), Color.White);

            }
            else if (screen == Screen.Tutorial)
            {
                _spriteBatch.Draw(tutorialMapTexture, tutorialBackgroundRect, Color.White);
  

                player.Draw(_spriteBatch);

                for (int i = 0; i < slimes.Count; i++)
                {
                    slimes[i].Draw(_spriteBatch);
                }
                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White * healthOpacity[i]);
                }
                _spriteBatch.DrawString(counterFont, $"{monstersKilled} / {monsterCountMax} Killed!", new Vector2(650, 5), Color.HotPink);
                _spriteBatch.Draw(instructionBoxTexture, instructionBoxRect, Color.White);
                _spriteBatch.DrawString(instructionFont, instructionText, new Vector2(20, 470), Color.White);
                _spriteBatch.DrawString(counterFont, "Click to continue", new Vector2(580, 570), Color.YellowGreen);

                //Text 1 - Welcome to the dungeon! *insert story if possible?* Your goal is to killed every monster on each floor, descending until you reach the bottom!
                // Text 2 - You can you WASD or the ARROW KEYS to move around. To fight, press the SPACEBAR to swing your sword! Try to kill the slime over there!
                // Text 3 - Great job! You can now descend to Level 1! Stand on the stairs and LEFT-CLICK to descend.

            }
            else if (screen == Screen.First)
            {
                _spriteBatch.Draw(firstMapTexture, window, Color.White);
                for (int i = 0; i < slimes.Count; i++)
                {
                    slimes[i].Draw(_spriteBatch);
                }

                for (int i = 0; i < orcs.Count; i++)
                {
                    orcs[i].Draw(_spriteBatch);
                }

                for (int i = 0; i < plants.Count; i++)
                {
                    plants[i].Draw(_spriteBatch);
                }

                player.Draw(_spriteBatch);
               

                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White * healthOpacity[i]);
                }
                _spriteBatch.DrawString(counterFont, $"{monstersKilled} / {monsterCountMax} Killed!", new Vector2(650, 5), Color.HotPink);

            }
            else if(screen == Screen.Second) 
            {
                _spriteBatch.Draw(secondMapTexture, window, Color.White);

                for (int i = 0; i < slimes.Count; i++)
                {
                    slimes[i].Draw(_spriteBatch);
                }

                for (int i = 0; i < orcs.Count; i++)
                {
                    orcs[i].Draw(_spriteBatch);
                }

                for (int i = 0; i < plants.Count; i++)
                {
                    plants[i].Draw(_spriteBatch);
                }

                player.Draw(_spriteBatch);

                for (int i = 0; i < healthRects.Count; i++)
                {
                    _spriteBatch.Draw(healthTextures[i], healthRects[i], Color.White * healthOpacity[i]);
                }
                _spriteBatch.DrawString(counterFont, $"{monstersKilled} / {monsterCountMax} Killed!", new Vector2(650, 5), Color.HotPink);

            }
            else if (screen == Screen.Win)
            {
                _spriteBatch.Draw(winBackgroundTexture, window, Color.White);
                // should have buttons to restart/go back to main menu and then reset everything
                _spriteBatch.DrawString(titleFont, "You survived \nthe Dungeon!", new Vector2(175, 210), Color.White);
               
            }
            else 
            {
                _spriteBatch.Draw(loseBackgroundTexture, window, Color.White);
                _spriteBatch.DrawString(titleFont, "You lost to \nthe Dungeon...", new Vector2(175, 210), Color.White);

            }



            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void EnemiesUpdate(List<Orc> orcs, List<Slime> slimes, List<Plant> plants, List<Rectangle> barriers, Player player)
        {
            for (int i = 0; i < slimes.Count; i++)
            {
                slimes[i].Update(player, barriers);
                if (!slimes[i].Drawing)
                {
                    slimes.RemoveAt(i);
                    i--;
                    monstersKilled += 1;
                }
            }

            for (int i = 0; i < orcs.Count; i++)
            {
                orcs[i].Update(player, barriers);
                if (!orcs[i].Drawing)
                {
                    orcs.RemoveAt(i);
                    i--;
                    monstersKilled += 1;
                }
            }

            for (int i = 0; i < plants.Count; i++)
            {
                plants[i].Update(player, barriers);
                if (!plants[i].Drawing)
                {
                    plants.RemoveAt(i);
                    i--;
                    monstersKilled += 1;
                }
            }


        }
    }
}
