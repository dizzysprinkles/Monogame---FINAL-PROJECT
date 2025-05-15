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
        private float _speed, _frameSpeed, _time;
        private Vector2 _playerLocation, _playerDirection;
        private Texture2D _playerIdleTexture, _playerWalkTexture, _playerAttackTexture, _testTexture, _playerMainTexture;
        private Rectangle _playerCollisionRect, _playerDrawRect;

        public Player(Texture2D idleTexture, Texture2D walkTexture, Texture2D attackTexture, Rectangle collisionRect, Rectangle drawRect, Texture2D rectangleTexture)
        {
            // Spritesheet Variables
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
            _speed = 1.5f;
            _time = 0.0f;
            

            // Textures
            _playerAttackTexture = attackTexture;
            _playerWalkTexture = walkTexture;
            _playerIdleTexture = idleTexture;
            _testTexture = rectangleTexture;
            _playerMainTexture = _playerWalkTexture;

            // Rectangles && Vectors
            _playerCollisionRect = collisionRect;
            _playerDrawRect = drawRect;
            _playerLocation = new Vector2(20, 20);
            _playerDirection = Vector2.Zero;

            _width = _playerWalkTexture.Width / _columns;
            _height = _playerWalkTexture.Height / _rows;
            UpdatePlayerRects();
        }

        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public void Update(KeyboardState keyboardState)
        {
            if (_time > _frameSpeed && _playerDirection != Vector2.Zero)
            {
                _time = 0f; // cannot assign time to 0 in the class... doesn't work lol
                _frame += 1;
                if (_frame >= _frames)
                    _frame = 0;
            }
            SetPlayerDirection(keyboardState);
            _playerLocation += _playerDirection * _speed;
            UpdatePlayerRects();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_testTexture, _playerCollisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_playerMainTexture, _playerDrawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
        }

        public void SetPlayerDirection(KeyboardState keyboardState)
        {
            _playerDirection = Vector2.Zero;
            if (keyboardState.IsKeyDown(Keys.A))
                _playerDirection.X += -1;
            if (keyboardState.IsKeyDown(Keys.D))
                _playerDirection.X += 1;
            if (keyboardState.IsKeyDown(Keys.W))
                _playerDirection.Y += -1;
            if (keyboardState.IsKeyDown(Keys.S))
                _playerDirection.Y += 1;

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
                _frame = 0;
        }

        public void UpdatePlayerRects()
        {
            _playerCollisionRect.Location = _playerLocation.ToPoint();
            _playerDrawRect.X = _playerCollisionRect.X - 12;
            _playerDrawRect.Y = _playerCollisionRect.Y - 8;

        }
    }
}
