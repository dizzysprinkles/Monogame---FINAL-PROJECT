using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    public class Player
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height;
        private int _frame, _frames;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time;
        private float _attackCooldown, _timeSinceLastAttack;
        private Vector2 _playerLocation, _playerDirection;
        private Texture2D _playerIdleTexture, _playerWalkTexture, _playerAttackTexture, _rectangleTexture, _playerMainTexture;
        private Rectangle _playerCollisionRect, _playerDrawRect, _swordCollisionRect, _upAttackRect, _downAttackRect, _leftAttackRect, _rightAttackRect;
        private int _health;
        private Vector2 _playerCenter;
        private bool _canDealDamage;



        public Player(Texture2D idleTexture, Texture2D walkTexture, Texture2D attackTexture, Rectangle collisionRect, Rectangle drawRect, Texture2D rectangleTexture, Rectangle swordRect)
        {
            //Spritesheet variable
            _columns = 4;
            _rows = 4;
            _leftRow = 0;
            _rightRow = 1;
            _upRow = 2;
            _downRow = 3;
            _directionRow = _downRow;
            _frameSpeed = 0.08f;
            _frames = 4;
            _frame = 0;
            _speed = 2f;
            _time = 0.0f;

            _canDealDamage = true;
            _timeSinceLastAttack = 0.0f;
            _attackCooldown = 0.15f;

            // Textures
            _playerAttackTexture = attackTexture;
            _playerWalkTexture = walkTexture;
            _playerIdleTexture = idleTexture;
            _rectangleTexture = rectangleTexture;
            _playerMainTexture = _playerWalkTexture;

            // Rectangles && Vectors
            _playerCollisionRect = collisionRect;
            _playerDrawRect = drawRect;
            _swordCollisionRect = swordRect;
            _playerLocation = _playerCollisionRect.Location.ToVector2();
            _playerDirection = Vector2.Zero;

            //playerCollisionRect = new Rectangle(200,260,25,45); 
            _downAttackRect = new Rectangle(190, 280, 55, 40);
            _leftAttackRect = new Rectangle(185, 260, 30, 45);
            _upAttackRect = new Rectangle(190, 260, 55, 40);
            _rightAttackRect = new Rectangle(210, 260, 30, 45);
            _swordCollisionRect = _downAttackRect;


            _playerCenter = _playerCollisionRect.Center.ToVector2();


            _width = _playerWalkTexture.Width / _columns;
            _height = _playerWalkTexture.Height / _rows;
            UpdatePlayerRects();

            //Other stuff
            _health = 10; 
        }

        public Rectangle Rectangle
        {
            get { return _playerCollisionRect; }
            set { _playerCollisionRect = value; }
        }

        public Vector2 Location
        {
            get { return _playerLocation;}
            set { _playerLocation = value; }    
        }

        public bool Intersects(Rectangle player) //intersects a rectangle
        {
            return _playerCollisionRect.Intersects(player);
        }

        public float AttackTime
        {
            get { return _timeSinceLastAttack; }
            set { _timeSinceLastAttack = value; }
        }

        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public Vector2 Center
        {
            get { return _playerCenter; }
        }


        public void Update(KeyboardState keyboardState, List<Texture2D> healthTextures, List<Rectangle> heartRects, List<Rectangle> barriers, List<float>heartOpacities, List<Slime> slimes, List<Orc>orcs, List<Plant> plants, SoundEffectInstance slimeHurt, SoundEffectInstance orcHurt, SoundEffectInstance plantHurt, SoundEffectInstance playerAttack, SoundEffectInstance playerWalk)
        {
            //Animation && Sword Rotation

            if (_playerMainTexture != _playerAttackTexture)
            {
                if (_time > _frameSpeed)
                {
                    _time = 0f;
                    _frame += 1;
                    if (_frame >= _frames)
                        _frame = 0;
                }
            }
            else
            {
                if (_time > _frameSpeed && _canDealDamage)
                {
                    _time = 0f;
                    _frame += 1;
                    playerAttack.Play();
                    if (_frame >= _frames)
                    {
                        _frame = 0;
                        for (int i = 0; i < slimes.Count; i++)
                        {
                            // Slime 
                            if (_swordCollisionRect.Intersects(slimes[i].Rectangle) && _canDealDamage)
                            {
                                slimes[i].Health -= 1;
                                slimeHurt.Play();
                                _canDealDamage = false;
                                _timeSinceLastAttack = 0f;
                                _playerMainTexture = _playerIdleTexture;
                            }
                        }
                        for (int i = 0; i < orcs.Count; i++)
                        {   // Orc
                            if (_swordCollisionRect.Intersects(orcs[i].Rectangle) && _canDealDamage)
                            {
                                orcs[i].Health -= 1;
                                orcHurt.Play();
                                _canDealDamage = false;
                                _timeSinceLastAttack = 0f;
                                _playerMainTexture = _playerIdleTexture;
                            }
                        }
                        for (int i = 0; i < plants.Count; i++)
                        {   // Plant
                            if (_swordCollisionRect.Intersects(plants[i].Rectangle) && _canDealDamage)
                            {
                                plants[i].Health -= 1;
                                plantHurt.Play();
                                _canDealDamage = false;
                                _timeSinceLastAttack = 0f;
                                _playerMainTexture = _playerIdleTexture;
                            }
                        }

                    }

                }
            }
            

            //Movement
            SetPlayerDirection(keyboardState, playerWalk);
            _playerLocation += _playerDirection * _speed;
            
            UpdatePlayerRects();

            if (_timeSinceLastAttack >= _attackCooldown)
            {
                _canDealDamage = true;
            }


            _playerCenter = _playerCollisionRect.Center.ToVector2();
            UpdateHearts(heartRects, heartOpacities);


            //Barrier Detection
            foreach (Rectangle barrier in barriers)
            {
                if (_playerCollisionRect.Intersects(barrier))
                {
                    _playerLocation -= _playerDirection * _speed;
                 
                    UpdatePlayerRects();
                }
            }

            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_playerMainTexture, _playerDrawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
        }

        public void SetPlayerDirection(KeyboardState keyboardState, SoundEffectInstance playerWalk)
        {
            _playerDirection = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            { 
                _playerDirection.X += -1;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _playerDirection.X += 1;
            }
                
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _playerDirection.Y += -1;
            }
                
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _playerDirection.Y += 1;
            }


            if (_playerDirection == Vector2.Zero)
            {
                playerWalk.Stop();
               _playerMainTexture = _playerIdleTexture;
            }
            else
            {
                _playerMainTexture = _playerWalkTexture;
            }

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _playerMainTexture = _playerAttackTexture;

            }

            if (_playerMainTexture == _playerWalkTexture)
            {
                playerWalk.Play();
                if (_playerDirection != Vector2.Zero)
                {
                    _playerDirection.Normalize();
                    if (_playerDirection.X < 0) // Moving left
                    {
                        _swordCollisionRect = _leftAttackRect;
                        _directionRow = _leftRow;
                    }
                    else if (_playerDirection.X > 0) // Moving right
                    {
                        _swordCollisionRect = _rightAttackRect;
                        _directionRow = _rightRow;
                    }
                    else if (_playerDirection.Y < 0) // Moving up
                    {
                        _swordCollisionRect = _upAttackRect;
                        _directionRow = _upRow;
                    }
                    else
                    {
                        _swordCollisionRect = _downAttackRect;
                        _directionRow = _downRow; // Moving down
                    }
                }
                else
                {
                    playerWalk.Stop();
                    _frame = 0;
                }

            }
        }

        public void UpdatePlayerRects()
        {
            _playerCollisionRect.Location = _playerLocation.ToPoint();
            _playerDrawRect.X = _playerCollisionRect.X - 12;
            _playerDrawRect.Y = _playerCollisionRect.Y - 10;

            _leftAttackRect.X = _playerCollisionRect.X - 15;
            _leftAttackRect.Y = _playerCollisionRect.Y;

            _rightAttackRect.X = _playerCollisionRect.X + 10;
            _rightAttackRect.Y = _playerCollisionRect.Y;

            _downAttackRect.X = _playerCollisionRect.X - 10;
            _downAttackRect.Y = _playerCollisionRect.Y + 20;

            _upAttackRect.X = _playerCollisionRect.X - 10;
            _upAttackRect.Y = _playerCollisionRect.Y;

        }

        public void UpdateHearts( List<Rectangle> heartRects, List<float> heartOpacities)
        {
            for (int i = 0; i < heartRects.Count; i++)
            {
                int heartValue = _health - (i * 2);

                if (heartValue >= 2)
                    heartOpacities[i] = 1f;
                else if (heartValue == 1)
                    heartOpacities[i] = 0.5f;
                else
                    heartOpacities[i] = 0.0f;
            }
        }
    }
}
