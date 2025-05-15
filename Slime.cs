using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___FINAL_PROJECT
{
    public class Slime
    {
        private int _rows, _columns, _directionRow;
        private int _width, _height;
        private int _frame, _frames;
        private int _leftRow, _rightRow, _upRow, _downRow;
        private float _speed, _frameSpeed, _time;
        private Vector2 _location, _direction;
        private Texture2D _deathdleTexture, _walkTexture, _attackTexture, _testTexture, _currentTexture;
        private Rectangle _collisionRect, _drawRect;

        public Slime()
        {

        }
    }
}
