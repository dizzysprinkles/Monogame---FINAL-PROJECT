using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    //TODO: add fighting and dying animations, had fighting hitbox, add player detection and then move towards player and then attack when intersects or something similar
    //ALMOST DONE: fight box; just need to add for all sides so it's easier
    public class Slime
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height;
        private int _frame, _frames, _walkFrames;
        private int _leftRow, _rightRow, _upRow, _downRow, _attackAddition;
        private float _speed, _frameSpeed, _time, _walkSpeed;
        private Vector2 _location, _direction;
        private Texture2D _deathTexture, _walkTexture, _attackTexture, _testTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect, _attackCollisionRect, _startingAttackRect;

        public Slime(Texture2D deathTexture, Texture2D walkTexture, Texture2D attackTexture, Texture2D rectangleTexture, Rectangle collisionRect, Rectangle drawRect, Rectangle attackRect)
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
            _testTexture = rectangleTexture;
            _currentTexture = _attackTexture;

            // Rectangles
            _attackCollisionRect = attackRect;
            _startingAttackRect = _attackCollisionRect;
            
            _collisionRect = collisionRect;
            _drawRect = drawRect;
            _location = new Vector2(40,40);
            _direction = Vector2.Zero;
            _width = _attackTexture.Width / _columns;
            _height = _attackTexture.Height / _rows;
            _attackAddition = -3;

            //UpdateRects();  //still have to configure

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

        public void Update()
        {

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
                    _attackCollisionRect.X += _attackAddition;  // +3 for right, -3 for left
                }
                else
                {
                    _attackCollisionRect.X = _startingAttackRect.X; // starting is 60, 70 for left, for up and down, collision box just contains the whole box;
                }


                if (_frame >= _frames)
                {
                    _frame = 0;
                }  
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_testTexture, _collisionRect, Color.Black * 0.3f);
            spriteBatch.Draw(_currentTexture, _drawRect, new Rectangle(_frame * _width, _directionRow * _height, _width, _height), Color.White);
            spriteBatch.Draw(_testTexture, _attackCollisionRect, Color.Red * 0.3f);
        }

        //public void UpdateRects()
        //{
        //    _collisionRect.Location = _location.ToPoint();
        //    _drawRect.X = _collisionRect.X - 12;
        //    _drawRect.Y = _collisionRect.Y - 20;

        //}
    }
}
