using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SnakeGame
{
    public class SnakeNode
    {
        public double x, y;
        public Rectangle rec = new Rectangle();
        public bool isBot;

        public SnakeNode(double x,double y, bool isBot = false)
        {
            this.isBot = isBot;
            this.x = x;
            this.y = y;
        }
        public void SetSnakeNodePosition(Brush color)
        {
            rec.Width = 10;
            rec.Height = 10;
            rec.Fill = color;
            Canvas.SetLeft(rec, x);
            Canvas.SetTop(rec, y);
        }
    }
}
