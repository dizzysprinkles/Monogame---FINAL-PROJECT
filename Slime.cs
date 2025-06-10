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
    //TODO: add dying animations,  attack when intersects or something similar, Reconfig attack hitboxes
    //DONE: fight box, Player detection & movement
    //CONSIDER: walking hitbox so it can actually walk through doors....
    public class Slime
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height, _detectionRadius;
        private int _frame, _frames, _walkFrames, _health;
        private int _leftRow, _rightRow, _upRow, _downRow, _attackAddition;
        private float _speed, _frameSpeed, _time, _walkSpeed;
        private Vector2 _location, _direction, _playerDistance;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _rectangleTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _startingAttackRect;
        private Vector2 _center;

        public Slime(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Rectangle attackRect, Player player)
        {
            // Spritesheet Variables
            _columns = 11; 
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _upRow;
            _frameSpeed = 0.08f;
            _frames = 11;
            _frame = 0;
            _speed = 1.5f;
            _time = 0.0f;
            _walkFrames = 8;
            _walkSpeed = 0.1f;



            // Textures
            _deathTexture = deathTexture; 
            _walkTexture = walkTexture; 
            _attackTexture = attackTexture; 
            _rectangleTexture = rectangleTexture;
            _currentTexture = _deathTexture;

            // Rectangles
            _attackCollisionRect = attackRect;
            _startingAttackRect = _attackCollisionRect;
            
            _collisionRect = collisionRect;
            _drawRect = drawRect;
            _location = new Vector2(62,60);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;
            _attackAddition = -3;

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

        public Rectangle Attack
        {
            get { return _attackCollisionRect; }
        }

        

        public void Update(Player player, List<Rectangle> barriers)
        {
            // So far, the slime will switch textures and track the player when it is in the radius
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

            if (_frame == 0) // will have to slightly fix when moving startingAttackRect can't be stable, has to move with it
            {
                if (_directionRow == _leftRow)
                {
                    _attackAddition = -3;
                    _startingAttackRect.X = 70;

                }
                else if (_directionRow == _rightRow)
                {
                    _attackAddition = 3;
                    _startingAttackRect.X = 60;
                }
                else if (_directionRow == _upRow || _directionRow == _downRow)
                {
                    _attackAddition = 0;
                    _startingAttackRect.X = 70; 
                }

            }
 

            if (_currentTexture == _walkTexture)
            {
                _frames = _walkFrames;
                _frameSpeed = _walkSpeed;
            }
            else
            {
                _frames = 11;
                _speed = 0.08f;
            }
                
            if (_time > _frameSpeed)
            {
                _time = 0f;
                _frame += 1;
                if (_frame < 8) // first 7 animations; last 4 are slime blowing up
                {
                    _attackCollisionRect.X += _attackAddition;  
                }
                else
                {
                    _attackCollisionRect.X = _startingAttackRect.X; 
                }

                if (_frame >= _frames)
                {
                    _frame = 0;
                }  
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_rectangleTexture, _collisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_currentTexture, _drawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
            //spriteBatch.Draw(_testTexture, _attackCollisionRect, Color.Red * 0.3f);
        }

        public void UpdateRects()
        {
            _collisionRect.Location = _location.ToPoint();
            _drawRect.X = _collisionRect.X - 22;
            _drawRect.Y = _collisionRect.Y - 20;

        }
    }
}
