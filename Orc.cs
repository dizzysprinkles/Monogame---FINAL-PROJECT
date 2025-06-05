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
        private int _width, _height, _health;
        private int _frame, _frames, _walkFrames, _detectionRadius;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time;
        private Vector2 _location, _direction, _center, _playerDistance;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _rectangleTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _leftAttackRect, _rightAttackRect, _upAttackRect, _downAttackRect;

        public Orc(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Rectangle attackRect, Player player)
        {
            // Spritesheet Variables
            _columns = 8;
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _rightRow;
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

            _location = new Vector2(200, 200);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;
            _downAttackRect = new Rectangle(200, 220, 55, 40);
            _leftAttackRect = new Rectangle(170, 195, 40, 55);
            _upAttackRect = new Rectangle(195,195,60,40);
            _rightAttackRect = new Rectangle(200,200,65,65);
            _attackCollisionRect = _downAttackRect;

            _detectionRadius = 115;
            _center = _collisionRect.Center.ToVector2();
            _playerDistance = _center - player.Center;
            _health = 10; // leave for now... Might need to increase 

            UpdateRects();

        }
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public void Update(Player player)
        {
            _playerDistance = _center - player.Center;
            if (_playerDistance.Length() <= _detectionRadius)
            {
                _currentTexture = _attackTexture;
                _direction = _playerDistance;
            }
            else
                _currentTexture = _walkTexture;

            if (_direction != Vector2.Zero)
            {
                _direction.Normalize();
                if (_direction.X > 0) // Moving left
                    _directionRow = _leftRow;
                else if (_direction.X < 0) // Moving right
                    _directionRow = _rightRow;
                else if (_direction.Y > 0) // Moving up
                    _directionRow = _upRow;
                else
                    _directionRow = _downRow; // Moving down
            }



            if (_directionRow == _downRow)
            {
                _attackCollisionRect = _downAttackRect;
            }
            else if (_directionRow == _upRow)
            {
                _attackCollisionRect = _upAttackRect;
            }
            else if (_directionRow == _leftRow)
            {
                _attackCollisionRect = _leftAttackRect;
            }
            else
            { 
                _attackCollisionRect= _rightAttackRect;
            }




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
            _drawRect.X = _collisionRect.X - 8;
            _drawRect.Y = _collisionRect.Y - 12;

        }
    }
}
