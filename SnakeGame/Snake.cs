using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Snake
    {
        private bool _isBot;

        private double _x, _y;
        private int _left = 1;
        private int _right = 2;
        private int _up = 3;
        private int _down = 4;

        public int Direction { get; set; }

        public List<SnakeNode> SnakeBody { get; private set; } = new List<SnakeNode>();

        public Snake(double x, double y, bool isBot = false)
        {
            _isBot = isBot;
            _x = x;
            _y = y;

            SnakeBody.Add(new SnakeNode(x, y));
        }

        public bool IsEat(List<Food> food)
        {
            if (SnakeBody[0].x == food[0].x && SnakeBody[0].y == food[0].y)
            {
                SnakeBody.Add(new SnakeNode(food[0].x, food[0].y));

                return true;
            }

            return false;
        }

        public void Move()
        {
            for (int i = SnakeBody.Count - 1; i > 0; i--)
            {
                SnakeBody[i] = SnakeBody[i - 1];
            }
            SnakeBody[0] = new SnakeNode(_x, _y);
        }

        public bool IsLeftTheBorder()
        {
            return SnakeBody[0].x > 940 || SnakeBody[0].y > 550 || SnakeBody[0].x < 0 || SnakeBody[0].y < 0;
        }

        public bool IsMetTail()
        {
            for (int i = 1; i < SnakeBody.Count; i++)
            {
                if (SnakeBody[0].x == SnakeBody[i].x && SnakeBody[0].y == SnakeBody[i].y)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeDirection(List<Food> food = null)
        {
            if (_isBot)
            {
                if (food[0].x < SnakeBody[0].x)
                    Direction = 1;
                if (food[0].x > SnakeBody[0].x)
                    Direction = 2;
                if (food[0].y < SnakeBody[0].y)
                    Direction = 3;
                if (food[0].y > SnakeBody[0].y)
                    Direction = 4;

                if (Direction == _up)
                    _y -= 10;
                if (Direction == _down)
                    _y += 10;
                if (Direction == _left)
                    _x -= 10;
                if (Direction == _right)
                    _x += 10;
            }
            else
            {
                if (Direction == _up)
                    _y -= 10;
                if (Direction == _down)
                    _y += 10;
                if (Direction == _left)
                    _x -= 10;
                if (Direction == _right)
                    _x += 10;
            }
        }
    }
}
