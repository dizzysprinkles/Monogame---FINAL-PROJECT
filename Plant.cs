using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    //TODO: Basically whole class... see Slime when it's completed for reference
    public class Plant
    {

        private int _rows, _columns, _directionRow;
        private int _width, _height;
        private int _frame, _frames;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time;
        private Vector2 _location, _direction;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _testTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect;

        public Plant(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect)
        {
            // Spritesheet Variables
            _columns = 7;
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _downRow;
            _frameSpeed = 0.08f;
            _frames = 7;
            _frame = 0;
            _speed = 1.5f;
            _time = 0.0f;

            // Textures
            _deathTexture = deathTexture; // Good
            _walkTexture = walkTexture;  // all frames except 1 is off
            _attackTexture = attackTexture; // Good
            _testTexture = rectangleTexture;
            _currentTexture = _attackTexture;

            // Rectangles
            _collisionRect = collisionRect;
            _drawRect = drawRect;
            _location = new Vector2(100, 100);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;

            //UpdateRects();

        }

        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public void Update()
        {
            if (_time > _frameSpeed)
            {
                _time = 0f;
                _frame += 1;
                if (_frame >= _frames)
                    _frame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_testTexture, _collisionRect, Color.Purple * 0.3f);
            spriteBatch.Draw(_currentTexture, _drawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
        }

        public void UpdateRects()
        {
            _collisionRect.Location = _location.ToPoint();
            _drawRect.X = _collisionRect.X - 15;
            _drawRect.Y = _collisionRect.Y - 10;

        }
    }
}
