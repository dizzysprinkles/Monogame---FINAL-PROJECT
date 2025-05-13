using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private float _speed, _time, _frameSpeed;
        private Vector2 _playerLocation, _playerDirection;
        private Texture2D _playerIdleTexture, _playerWalkTexture, _playerAttackTexture;

        public Player(float time)
        {
            _time = time;
            _columns = 6; // only for attack spritesheet... will need to edit it to make it 4 columns like walk and idle
            _rows = 4;
            _leftRow = 0;
            _rightRow = 1;
            _upRow = 2;
            _downRow = 3;
            _directionRow = _downRow;
        }
    }
}
