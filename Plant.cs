using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    //TODO: add dying animations,  attack when intersects or something similar, reconfig attack hitboxes
    //CONSIDER: walking hitbox so it can actually walk through doors....
    //DONE: hitboxes, player detection and movement
    public class Plant
    {

        private int _rows, _columns, _directionRow;
        private int _width, _height, _health;
        private int _frame, _frames, _attackAddition, _detectionRadius;
        private int _leftRow, _rightRow, _upRow, _downRow, _walkFrames;
        private float _speed, _frameSpeed, _time, _walkSpeed, _attackFrame;
        private Vector2 _location, _direction, _center, _playerDistance;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _rectangleTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _startingAttackRect;

        public Plant(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Rectangle attackRect, Player player)
        {
            // Spritesheet Variables
            _columns = 7;
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _leftRow;
            _frameSpeed = 0.09f;
            _frames = 7;
            _frame = 0;
            _speed = 1.5f;
            _time = 0.0f;
            _walkFrames = 6;
            _walkSpeed = 0.1f;
            _attackFrame = 1;

            // Textures
            _deathTexture = deathTexture; 
            _walkTexture = walkTexture;  
            _attackTexture = attackTexture; 
            _rectangleTexture = rectangleTexture;
            _currentTexture = _attackTexture;

            // Rectangles
            _collisionRect = collisionRect;
            _drawRect = drawRect;
            _location = new Vector2(100, 100);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;
            _attackCollisionRect = attackRect;
            _startingAttackRect = _attackCollisionRect;
            _attackAddition = -5;  

            _center = _collisionRect.Center.ToVector2();
            _playerDistance = player.Center - _center;

            _detectionRadius = 115;

            _health = 10; //might need to adjust

            UpdateRects();

        }

        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public void Update(Player player, List<Rectangle>barriers)
        {
            _center = _collisionRect.Center.ToVector2();
            _playerDistance = player.Center - _center;
            if (_playerDistance.Length() <= _detectionRadius)
            {
                _currentTexture = _attackTexture;
                _direction = _playerDistance;
            }
            else
            {
                _direction = Vector2.Zero;
                _currentTexture = _walkTexture;
            }

            if (_direction != Vector2.Zero)
            {
                _direction.Normalize();
                if (_direction.X < 0) // Moving left
                    _directionRow = _leftRow;
                else if (_direction.X > 0) // Moving right
                    _directionRow = _rightRow;
                else if (_direction.Y < 0) // Moving up
                    _directionRow = _upRow;
                else
                    _directionRow = _downRow; // Moving down
            }
            _location += _direction * _speed;
            UpdateRects();

            foreach (Rectangle barrier in barriers)
            {
                if (_collisionRect.Intersects(barrier))
                {
                    _location -= _direction * _speed;

                    UpdateRects();
                }
            }


            if (_frame == 0)
            {
                if (_directionRow == _leftRow)
                {
                    _attackAddition = -5;
                    _attackFrame = 1;
                }
                else if (_directionRow == _rightRow)
                {
                    _attackAddition = 5;
                    _attackFrame = 2;

                }
                else if (_directionRow == _upRow || _directionRow == _downRow)
                {
                    _attackAddition = 0;
                }

            }

            if (_currentTexture == _walkTexture)
            {
                _frames = _walkFrames;
                _speed = _walkSpeed;
            }
            else
            {
                _frames = 7;
                _speed = 0.08f;
            }
                


            if (_time > _frameSpeed)
            {
                _time = 0f;
                _frame += 1;
                if (_frame > _attackFrame) 
                {
                    _attackCollisionRect.X += _attackAddition; 
                }
               
       
                if (_frame >= _frames)
                {
                    _frame = 0;
                    _attackCollisionRect.X = _startingAttackRect.X;
                }
                   
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
            _drawRect.Y = _collisionRect.Y - 10;
            //_attackCollisionRect.X = _collisionRect.X + 10;
            //_attackCollisionRect.Y = _collisionRect.Y + 10;
            //_startingAttackRect = _attackCollisionRect;
        }
    }
}
