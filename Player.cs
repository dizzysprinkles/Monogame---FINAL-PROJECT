using Microsoft.Xna.Framework;
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
        private float _speed, _frameSpeed, _time, _swordRotation, _upSwordMax, _downSwordMax, _leftSwordMax, _rightSwordMax, _startingSwordRotation, _leftSwordAdd, _rightSwordAdd, _upSwordAdd, _downSwordAdd, _swordMaxRotation, _swordAddition;
        private Vector2 _playerLocation, _playerDirection, _swordLocation;
        private Texture2D _playerIdleTexture, _playerWalkTexture, _playerAttackTexture, _rectangleTexture, _playerMainTexture;
        private Rectangle _playerCollisionRect, _playerDrawRect, _swordCollisionRect;
        private int _health;
        private Vector2 _playerCenter;



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

            _downSwordMax = 0.5f;
            _downSwordAdd = 0.45f;

            _upSwordMax = 4.8f;
            _upSwordAdd =0.75f;

            _leftSwordAdd = 0.3f;
            _leftSwordMax = 2.4f;

            _rightSwordAdd = -0.3f;
            _rightSwordMax = 4.5f;

            _startingSwordRotation = -0.9f;
            _swordRotation = _startingSwordRotation;
            _swordAddition = _downSwordAdd;
            _swordMaxRotation = _downSwordMax;

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
            _playerLocation = new Vector2(188, 250);
            _playerDirection = Vector2.Zero;
            _swordLocation = new Vector2(196,275);

            _playerCenter = _playerCollisionRect.Center.ToVector2();


            _width = _playerWalkTexture.Width / _columns;
            _height = _playerWalkTexture.Height / _rows;
            UpdatePlayerRects();

            //Other stuff
            _health = 10; // 5 hearts drawn to screen, lose half a heart per hit, need to figure that out...
        }

        public Rectangle Rectangle
        {
            get { return _playerCollisionRect; }
        }

        public bool Intersects(Rectangle player) //intersects a rectangle
        {
            return _playerCollisionRect.Intersects(player);
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

        public Rectangle Sword
        {
            get { return _swordCollisionRect; }

        }

        public Texture2D Skin
        {
            get { return _playerMainTexture; }
        }

        //Mouse state is called for update but isn't used.... 
        public void Update(KeyboardState keyboardState, MouseState mouseState, List<Texture2D> healthTextures, List<Rectangle> heartRects, List<Rectangle> barriers, List<float>heartOpacities, Orc orc, Plant plant, Slime slime)
        {
            //Animation && Sword Rotation
            if (_time > _frameSpeed )
            {
                _time = 0f;
                _frame += 1;
                if (_playerMainTexture == _playerAttackTexture)
                {
                    _swordRotation += _swordAddition;
                    if (_startingSwordRotation != 5.5f)
                    { 
                        if (_swordRotation >= _swordMaxRotation) 
                        {
                            _swordRotation = _startingSwordRotation;
                        }

                    }
                    else
                    {
                        if (_swordRotation <= _swordMaxRotation)
                        {
                            _swordRotation = _startingSwordRotation;
                        }
                    }
                   
                }
                else
                    _swordRotation = _startingSwordRotation;

                if (_frame >= _frames)
                {

                    _frame = 0;
                }
            }
     
            //Movement
            SetPlayerDirection(keyboardState);
            _playerLocation += _playerDirection * _speed;
            
            UpdatePlayerRects();


            _playerCenter = _playerCollisionRect.Center.ToVector2();
            UpdateHearts(heartRects, heartOpacities);

            if (_swordCollisionRect.Intersects(orc.Rectangle) && _playerMainTexture == _playerAttackTexture)
            {
                orc.Health -= 1;
            }

            if (_swordCollisionRect.Intersects(slime.Rectangle) && _playerMainTexture == _playerAttackTexture)
            {
                slime.Health -= 1;
            }

            if (_swordCollisionRect.Intersects(plant.Rectangle) && _playerMainTexture == _playerAttackTexture)
            {
                plant.Health -= 1;
            }


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
            spriteBatch.Draw(_rectangleTexture, _playerCollisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_playerMainTexture, _playerDrawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
            spriteBatch.Draw(_rectangleTexture, _swordCollisionRect,null, Color.Red * 0.0f, _swordRotation, new Vector2(_playerCollisionRect.Width/2, _playerCollisionRect.Height/2), SpriteEffects.None, 0f);
        }

        public void SetPlayerDirection(KeyboardState keyboardState)
        {
      
            _playerDirection = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            { 
                _playerDirection.X += -1;
                _startingSwordRotation = 1.4f;
                _swordRotation = _startingSwordRotation;
                _swordMaxRotation = _leftSwordMax;
                _swordAddition = _leftSwordAdd;
                _swordCollisionRect.Height = 20;
            }

            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                _playerDirection.X += 1;
                _startingSwordRotation = 5.5f;
                _swordRotation = _startingSwordRotation;
                _swordMaxRotation = _rightSwordMax;
                _swordAddition = _rightSwordAdd;
                _swordCollisionRect.Height = 30;
            }
                
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                _playerDirection.Y += -1;
                _startingSwordRotation = 1.7f;
                _swordRotation = _startingSwordRotation;
                _swordMaxRotation = _upSwordMax;
                _swordAddition = _upSwordAdd;
                _swordCollisionRect.Height = 30;
            }
                
            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                _playerDirection.Y += 1;
                _startingSwordRotation = -0.9f;
                _swordRotation = _startingSwordRotation;
                _swordMaxRotation = _downSwordMax;
                _swordAddition = _downSwordAdd;
                _swordCollisionRect.Height = 30;
            }


            if (_playerDirection == Vector2.Zero)
            {
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
                if (_playerDirection != Vector2.Zero)
                {
                    _playerDirection.Normalize();
                    if (_playerDirection.X < 0) // Moving left
                        _directionRow = _leftRow;
                    else if (_playerDirection.X > 0) // Moving right
                        _directionRow = _rightRow;
                    else if (_playerDirection.Y < 0) // Moving up
                        _directionRow = _upRow;
                    else
                        _directionRow = _downRow; // Moving down
                }
                else
                {
                    _frame = 0;
                }

            }
        }

        public void UpdatePlayerRects()
        {
            _playerCollisionRect.Location = _playerLocation.ToPoint();
            _playerDrawRect.X = _playerCollisionRect.X - 12;
            _playerDrawRect.Y = _playerCollisionRect.Y - 10;
            _swordLocation.X = _playerLocation.X + 8;
            _swordLocation.Y = _playerLocation.Y + 25;
            _swordCollisionRect.Location = _swordLocation.ToPoint();
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
