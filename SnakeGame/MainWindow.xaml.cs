using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _time = new DispatcherTimer();
        private Snake _snake;
        private Snake _bot;
        private List<Food> _food = new List<Food>();
        private Random _rd = new Random();

        private int _left = 1;
        private int _right = 2;
        private int _up = 3;
        private int _down = 4;

        private int _score = 0;

        public MainWindow()
        {
            InitializeComponent();

            _snake = new Snake(100, 100);
            _bot = new Snake(200, 200, true);

            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));
            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));
            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));
            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));
            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));
            _food.Add(new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10));

            _time.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _time.Tick += GameProcess;
        }

        private void CreateFood(int index = -1)
        {
            if (index == -1)
            {
                foreach (Food food in _food)
                {
                    food.SetFoodPosition();
                    gameField.Children.Add(food.ell);
                }
            }
            else
            {
                _food[index].SetFoodPosition();
                gameField.Children.Insert(index, _food[index].ell);
            }
        }


        private void CreateSnake()
        {
            foreach (SnakeNode snake in _snake.SnakeBody)
            {
                snake.SetSnakeNodePosition(Brushes.Red);
                gameField.Children.Add(snake.rec);
            }
        }

        private void CreateBot()
        {
            foreach (SnakeNode snake in _bot.SnakeBody)
            {
                snake.SetSnakeNodePosition(Brushes.Green);
                gameField.Children.Add(snake.rec);
            }
        }

        private void BotProcess()
        {
            FoodHunt();

            if (_bot.Direction != 0)
            {
                _bot.Move();
            }

            _bot.ChangeDirection(_food);
        }


        private void GameProcess(object sender, EventArgs e)
        {
            BotProcess();

            FoodHunt();
            RanIntoBot();

            if (_snake.Direction != 0)
            {
                _snake.Move();
            }

            _snake.ChangeDirection();
      

            if(_snake.IsLeftTheBorder() || _snake.IsMetTail())
            {
                RestartGame();
            }

            if (_score >= 15)
            {
                MessageBox.Show("You win!");
                RestartGame();
            }

            RedrawSnake();
        }

        private void EatTailNode(SnakeNode node)
        {
            _snake.EatTailNode(node);
            _score++;
            txtbScore.Text = _score.ToString();
        }

        private void FoodHunt()
        {
            for (int i = 0; i < _food.Count; i++)
            {
                if (_snake.IsEat(_food[i]) || _bot.IsEat(_food[i]))
                {
                    if (_snake.IsEat(_food[i]))
                        _score++;
                    else
                        _bot.ChangeFoodHunted(_food.Count);

                    _food[i] = new Food(_rd.Next(0, 70) * 10, _rd.Next(0, 45) * 10);
                    gameField.Children.RemoveAt(i);
                    CreateFood(i);

                    txtbScore.Text = _score.ToString();

                    break;
                }
            }
        }

        private void RanIntoBot()
        {
            foreach (SnakeNode node in _bot.SnakeBody)
            {
                if (_snake.IsSnake(node))
                {
                    int bodyCount = _bot.SnakeBody.Count;
                    int nodeIndex = _bot.SnakeBody.IndexOf(node);
                    for (int i = bodyCount - 1; i >= nodeIndex; i--)
                    {
                        EatTailNode(_bot.SnakeBody[i]);
                        _bot.SnakeBody.RemoveAt(i);
                    }

                    if (_bot.SnakeBody.Count == 0)
                    {
                        CreateBot();
                    }

                    break;
                }
            }
        }

        private void RedrawSnake()
        {
            List<int> snakeIndexes = new List<int>();

            for (int i = 0; i < gameField.Children.Count; i++)
            {
                if (gameField.Children[i] is Rectangle)
                {
                    snakeIndexes.Add(i);
                }
            }

            snakeIndexes.Reverse();

            foreach(int index in  snakeIndexes)
            {
                gameField.Children.RemoveAt(index);
            }

            CreateSnake();
            CreateBot();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_snake.SnakeBody.Count == 1)
            {
                if (e.Key == Key.Up)
                    _snake.Direction = _up;
                if (e.Key == Key.Down)
                    _snake.Direction = _down;
                if (e.Key == Key.Left)
                    _snake.Direction = _left;
                if (e.Key == Key.Right)
                    _snake.Direction = _right;
            }
            else
            {
                if (e.Key == Key.Up && _snake.Direction != _down)
                    _snake.Direction = _up;
                if (e.Key == Key.Down && _snake.Direction != _up)
                    _snake.Direction = _down;
                if (e.Key == Key.Left && _snake.Direction != _right)
                    _snake.Direction = _left;
                if (e.Key == Key.Right && _snake.Direction != _left)
                    _snake.Direction = _right;
            }
        }

        private void RestartGame()
        {
            _snake = new Snake(100, 100);
            _bot = new Snake(200, 200, true);
            _score = 0;

            txtbScore.Text = _score.ToString();

            int foodX = _rd.Next(0, 37) * 10;
            int foodY = _rd.Next(0, 35) * 10;

            _food.Clear();
            _food.Add(new Food(_rd.Next(0, 37) * 10, _rd.Next(0, 35) * 10));
            CreateFood();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSnake();
            CreateBot();
            CreateFood();
            _time.Start();
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }
    }
}
