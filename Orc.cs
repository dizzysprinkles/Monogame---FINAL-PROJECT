using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    //TODO: add fighting and dying animations, add player detection and then move towards player and then attack when intersects or something similar
    public class Orc
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height;
        private int _frame, _frames, _walkFrames;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time;
        private Vector2 _location, _direction;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _rectangleTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _startingAttackRect;

        public Orc(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Rectangle attackRect)
        {
            // Spritesheet Variables
            _columns = 8;
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _rightRow;
            // downRow collision - (200, 220, 55, 40); leftRow collision - (170, 195, 40, 55); upRow collision - (195, 195, 60, 40); rightRow collision - (200, 200, 65, 55);
            _frameSpeed = 0.08f;
            _frames = 8;
            _frame = 0;
            _speed = 1.5f;
            _time = 0.0f;
            _walkFrames = 6;

            // Textures
            _deathTexture = deathTexture; 
            _walkTexture = walkTexture; 
            _attackTexture = attackTexture; 
            _rectangleTexture = rectangleTexture;
            _currentTexture = _attackTexture;

            // Rectangles
            _collisionRect = collisionRect;
            _drawRect = drawRect;
            _attackCollisionRect = attackRect;
            _startingAttackRect = _attackCollisionRect;
            _location = new Vector2(200, 200);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;

            UpdateRects();

        }
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public void Update()
        {
            if (_currentTexture == _walkTexture)
            {
                _frames = _walkFrames;
            }
            else
                _frames = 8;



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
            spriteBatch.Draw(_rectangleTexture, _collisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_currentTexture, _drawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
            //spriteBatch.Draw(_rectangleTexture, _attackCollisionRect, Color.Red * 0.3f);
        }

        public void UpdateRects()
        {
            _collisionRect.Location = _location.ToPoint();
            _drawRect.X = _collisionRect.X - 15;
            _drawRect.Y = _collisionRect.Y - 12;

        }
    }
}
