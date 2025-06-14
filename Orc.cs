using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    //TODO: add dying animations, attack cooldown, hit taken
    //DONE: hitboxes, player detection and movement, attack
    public class Orc
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height, _health;
        private int _frame, _frames, _walkFrames, _detectionRadius, _attackRadius, _idleFrames;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time, _attackCooldown, _timeSinceLastAttack;
        private Vector2 _location, _direction, _center, _playerDistance;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _rectangleTexture, _currentTexture, _idleTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _leftAttackRect, _rightAttackRect, _upAttackRect, _downAttackRect, _walkCollisionRect;
        private bool _canDealDamage;

        public Orc(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Player player, Texture2D idleTexture, Rectangle walkRect)
        {
            // Spritesheet Variables
            _columns = 8;
            _rows = 4;
            _leftRow = 2;
            _rightRow = 3;
            _upRow = 1;
            _downRow = 0;
            _directionRow = _upRow;
            _frameSpeed = 0.08f;
            _frames = 8;
            _frame = 0;
            _speed = 1.5f;
            _time = 0.0f;
            _walkFrames = 6;
            _idleFrames = 4;

            _attackCooldown = 0.65f;
            _timeSinceLastAttack = 0f;
            _canDealDamage = true;



            // Textures
            _deathTexture = deathTexture; 
            _walkTexture = walkTexture; 
            _attackTexture = attackTexture; 
            _rectangleTexture = rectangleTexture;
            _idleTexture = idleTexture;
            _currentTexture = _attackTexture;

            // Rectangles
            _collisionRect = collisionRect;
            _drawRect = drawRect;

            _location = new Vector2(220, 300);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;
            _downAttackRect = new Rectangle(212, 320, 65, 40); 
            _leftAttackRect = new Rectangle(200, 300, 60, 53);
            _upAttackRect = new Rectangle(214,283,60,40);
            _rightAttackRect = new Rectangle(220,300,65,53);
            _attackCollisionRect = _upAttackRect;
            _walkCollisionRect = walkRect;

            

            _detectionRadius = 115;
            _attackRadius = 40;
            _center = _collisionRect.Center.ToVector2();
            _playerDistance = player.Center - _center;
            _health = 10; // leave for now... Might need to increase 

            UpdateRects();

        }
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public float AttackTime
        {
            get { return _timeSinceLastAttack; }
            set { _timeSinceLastAttack = value; }
        }

        public void Update(Player player, List<Rectangle> barriers)
        {
            _center = _collisionRect.Center.ToVector2();
            _playerDistance = player.Center - _center;
            if (_playerDistance.Length() <= _detectionRadius && _playerDistance.Length() > _attackRadius)
            {
                _currentTexture = _walkTexture;
                _direction = _playerDistance;
            }
            else if (_playerDistance.Length() <= _attackRadius && _canDealDamage)
            {
                _currentTexture = _attackTexture;
                _direction = Vector2.Zero;
            }
            else
            {
                _direction = Vector2.Zero;
                _currentTexture = _idleTexture;
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

            if (_timeSinceLastAttack >= _attackCooldown)
            {
                _canDealDamage = true;
            }

           

            foreach (Rectangle barrier in barriers)
            {
                if (_walkCollisionRect.Intersects(barrier))
                {
                    _location -= _direction * _speed;

                    UpdateRects();
                }
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
            else if (_currentTexture == _idleTexture)
            {
                _frames = _idleFrames;
            }
            else
                _frames = 8;


            if (_currentTexture != _attackTexture)
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
                    if (_frame >= _frames)
                    {
                        _frame = 0;
                        if (_attackCollisionRect.Intersects(player.Rectangle) && _canDealDamage)
                        {
                            //player loses health here
                            _canDealDamage = false;
                            _timeSinceLastAttack = 0f;
                            _currentTexture = _idleTexture;
                        }
                    }
                        
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_rectangleTexture, _collisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_currentTexture, _drawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
            spriteBatch.Draw(_rectangleTexture, _attackCollisionRect, Color.Red * 0.3f);
        }


        public void UpdateRects()
        {
            _collisionRect.Location = _location.ToPoint();
            _drawRect.X = _collisionRect.X - 8;
            _drawRect.Y = _collisionRect.Y - 12;

            _downAttackRect.X = _collisionRect.X - 8;
            _downAttackRect.Y = _collisionRect.Y + 20;

            _leftAttackRect.X = _collisionRect.X - 20;
            _leftAttackRect.Y = _collisionRect.Y;

            _rightAttackRect.X = _collisionRect.X;
            _rightAttackRect.Y = _collisionRect.Y;

            _upAttackRect.X = _collisionRect.X - 6;
            _upAttackRect.Y = _collisionRect.Y - 17;

        }
    }
}
