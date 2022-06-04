﻿using System;
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
        private int _countSnakeNode = 0;

        public MainWindow()
        {
            InitializeComponent();

            int foodX = _rd.Next(0, 37) * 10;
            int foodY = _rd.Next(0, 35) * 10;

            _snake = new Snake(100, 100);
            _bot = new Snake(200, 200, true);
            _food.Add(new Food(foodX, foodY));

            _time.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _time.Tick += GameProcess;
        }

        private void CreateFood()
        {
            _food[0].SetFoodPosition();
            gameField.Children.Insert(0, _food[0].ell);
        }


        private void CreateSnake()
        {
            foreach (SnakeNode snake in _snake.SnakeBody)
            {
                snake.SetSnakeNodePosition(Brushes.Red);
                gameField.Children.Add(snake.rec);
            }

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

            if (_snake.Direction != 0)
            {
                _snake.Move();
            }

            _snake.ChangeDirection();
      

            if(_snake.IsLeftTheBorder() || _snake.IsMetTail())
            {
                this.Close();
            }


            RedrawSnake();
        }

        

        private void FoodHunt()
        {
            if (_snake.IsEat(_food) || _bot.IsEat(_food))
            {
                _food[0] = new Food(_rd.Next(0, 37) * 10, _rd.Next(0, 35) * 10);
                gameField.Children.RemoveAt(0);
                CreateFood();
                _score++;
                txtbScore.Text = _score.ToString();
            }
        }

        private void RedrawSnake()
        {
            for (int i = 0; i < gameField.Children.Count; i++)
            {
                if (gameField.Children[i] is Rectangle)
                    _countSnakeNode++;
            }

            gameField.Children.RemoveRange(1, _countSnakeNode);
            _countSnakeNode = 0;
            CreateSnake();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSnake();
            CreateFood();
            _time.Start();
        }
    }
}