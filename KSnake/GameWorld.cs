using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace KSnake
{
    class GameWorld: FileScore
    {

        private List<Snake> snakeElements = new List<Snake>();
        private List<Wall> walls = new List<Wall>();
        private List<Apple> apples = new List<Apple>();
      
        private Door door = new Door();    
        private Ball ball = new Ball();
        private Snake snake = new Snake();       
        private Wall wall;
        private Apple apple;

        private SnakeField snakeField;

        private int numberColumns; 
        private int numberRows; 
        private int NumberSnakeElements;

        DispatcherTimer MainTimer = new DispatcherTimer();
        DispatcherTimer TimerForExtraApple = new DispatcherTimer();
        DispatcherTimer TimerForLabel = new DispatcherTimer();
      
        public Direction snakeDirection { get; set; }
        private Direction ballDirection;
        
        private Snake tailBackup;

        private int counterApplesOnField = 0;
        private int counterEatApples = 0;

        private bool checkOpenDoor = false;

        private int timeNewApples = 0;
      
        private bool checkReturnSnake = true;
 
        public bool CheckRemoveSnake { get; set; }

       
        public GameWorld(SnakeField snakeField) 
        {
            this.snakeField = snakeField;
            NumberSnakeElements = snakeField.NumberSnakeElements;
         
            InitializeGame();
            DrawElements();
            InitializeSnake();
            InitializeTime();
        }
       
        private void InitializeGame()
        {
            //розміри ігрового поля
            numberColumns = (int)snakeField.Snake_Game.ActualWidth / door.SizeCell;
            numberRows = (int)snakeField.Snake_Game.ActualHeight / door.SizeCell;
            wall = new Wall(numberRows, numberColumns);
            apple = new Apple(numberRows, numberColumns);
        }

        //метод, який зображує всі елементи на полю
        private void DrawElements()
        {
            walls = wall.DrawList(snakeField, walls);
            snake.DrawList(snakeField, snakeElements);
            door = door.Draw(snakeField);
            ball = ball.Draw(snakeField);
            
        }

        private void InitializeSnake()
        {
            //Початкове значення змійки дорівнює 3 елемента, а для переходу на новий рівень
            //ініціалізується нова змійка з відповідною кількістю елементів
            if (NumberSnakeElements == 3)
            {
                snakeElements = snake.InitializeSnake(snakeElements, numberRows, NumberSnakeElements);
            }
            else
            {
                snakeElements = snake.InitializeSnake(snakeElements, numberRows, NumberSnakeElements);
            }
            //Початковий напрямок змійки вгору
            snakeDirection = Direction.Up;
        }

        private void InitializeTime()
        {
            //Основний лічильник для руху гри
            MainTimer.Interval = TimeSpan.FromMilliseconds(150);
            MainTimer.Tick += GameLoop;
            MainTimer.Start();
            //Лічильник для появи додаткових яблук
            TimerForExtraApple.Interval = TimeSpan.FromSeconds(10);
            //Лічильник для відображення секунд до появи додаткових яблук
            TimerForLabel.Interval = TimeSpan.FromSeconds(1);
            TimerForLabel.Tick += TimerNewApplesTick;
            TimerForLabel.Start();
        }

        //Метод для руху м'яча
        private void MoveBall()
        {
            if (ball == null)
                return;
            try
            {
                ballDirection = ball.MoveBall(walls, ball, ballDirection, door, checkOpenDoor);
            }
            catch (NullReferenceException)
            {

            }
          
        }

        //Подія, яка працює з інтервалом часу відповідно до лічильника MainTimer
        private void GameLoop(object sender, EventArgs e)
        {
            //Якщо змійка вийшла через відкриті двері
            if (snakeElements[0].Y <= 0)
            {
                //зберігає поточну кількість елементів змійки для переходу на новий рівень
                if (checkReturnSnake)
                {
                    checkReturnSnake = false;
                    NumberSnakeElements = snakeElements.Count;
                }
                RemoveWinnerSnake();
                if (snakeElements.Count != 0)
                {
                    snake.FinalMoveSnake(snakeElements, snakeDirection);
                }
            }
            else
            {
                snakeDirection = snake.MoveSnake(snakeElements, snakeDirection);
                MoveBall();
                CheckCollision();
                FirstApple();
            }
            snake.DrawList(snakeField, snakeElements);
    
        }

        //Поява першого яблука на ігровому полі
        private void FirstApple()
        {
            if (apples.Count == 0 && counterApplesOnField != snakeField.ChooseNumberApples)
            {
                apples = apple.DrawApple(snakeField, apples, snakeElements, walls, door);
                counterApplesOnField++;
                TimerForExtraApple.Start();
                TimerForExtraApple.Tick += ExtraApples;
            }
        }

        //Видалення елементів змійки, коли вона зайшла у двері
        private void RemoveWinnerSnake()
        {
            CheckRemoveSnake = true;
            snakeField.Snake_Game.Children.Remove(snakeElements[0].UIElement);
            snakeElements.Remove(snakeElements[0]);
            //Перехід на новий рівень
            if (snakeElements.Count == 0 && (snakeField.ChooseNumberApples == 10 || snakeField.ChooseNumberApples == 20))
            {
                StopGame();
                snakeField.NewLevel(snakeField.ScoreGame,NumberSnakeElements);
               
            }
            //Завершення гри 
            else if (snakeElements.Count == 0 && snakeField.ChooseNumberApples == 30)
            {
                StopGame();
                WinnerGame();
            }
            
        }
      
       //Подія, яка створює додаткові яблука
        private void ExtraApples(object sender, EventArgs e)
        {
            if (counterApplesOnField == snakeField.ChooseNumberApples)
            {
                TimerForLabel.Stop();
                return;
            }
            apples = apple.DrawApple(snakeField, apples, snakeElements, walls, door);
            counterApplesOnField++;
            timeNewApples = 0;
        }

        //Подія для виведення на екран часу до додаткового яблука
        private void TimerNewApplesTick(object sender, EventArgs e)
        {
            timeNewApples++;
            snakeField.TimeEat.Content = $"До додаткового яблука: {10 - timeNewApples}";
        }

        //Перевіє всі можливі зіткнення 
        private void CheckCollision()
        {
            CollisionWithApple();
            if (wall.CollisionWittWall(snakeElements, walls))
            {
                wall.LosingGameMusic();
                LosingGame(wall.Message());
            }
            else if (snake.CollisionWithSelf(snakeElements))
            {
                snake.LosingGameMusic();
                LosingGame(snake.Message());
            }
            else if (door.CollisionWithDoor(snakeElements, door, checkOpenDoor))
            {
                door.LosingGameMusic();
                LosingGame(door.Message());
            }
            else if (ball != null)
            {
                ballDirection = snake.CollisionBallWithSnakeBody(snakeElements, ball, ballDirection);

                if (ball.CollisionSnakeHeadWithBall(snakeElements, ball, snakeDirection))
                {
                    ball.LosingGameMusic();
                    LosingGame(ball.Message());
                }

            }
        }

        //Зіткнення змійки з яблуком
        private void CollisionWithApple()
        {
            if (apple.ChechCollisionWithApple(snakeElements, snakeField, apples))
            {
                counterEatApples++;
                snakeField.EatApple.Content = $"Залишилося з'їсти яблук: {snakeField.ChooseNumberApples - counterEatApples}";
                GrowSnake();
                TimerForExtraApple.Interval = TimeSpan.FromSeconds(10);
                Score();
                timeNewApples = 0;
                //Добавляє нове яблуко на поле
                if (!CheckWinnerGame() && counterApplesOnField < snakeField.ChooseNumberApples)
                {
                    apples = apple.DrawApple(snakeField, apples, snakeElements, walls, door);
                    counterApplesOnField++;
                }
            }
           
        }

        //Зміна швидкості відповідно до вибраної швидкості
        public void FastGame(double fast)
        {
            MainTimer.Interval = TimeSpan.FromMilliseconds(150 / fast);
        }
        
        //Завершення гри
        public void StopGame()
        {
            MainTimer.Stop();
            MainTimer.Tick -= GameLoop;
            TimerForExtraApple.Tick -= ExtraApples;
            TimerForExtraApple.Stop();
            TimerForLabel.Stop();
        }

        //Перевіряє завершення відповідного рівня
        private bool CheckWinnerGame()
        {
            if (counterEatApples == snakeField.ChooseNumberApples)
            {
                //відкриття дверей 
                door.Animation(door);
                //Видалення м'яча з поля
                RemoveBall();
                checkOpenDoor = true;
                return true;
            }  
            return false;

        }

        //Видалення м'яча після відкриття дверей
        private void RemoveBall()
        {
            snakeField.Snake_Game.Children.Remove(ball.UIElement);
            ball = null;
            
        }

        //Збільшення змійки на одну клітинку
        private void GrowSnake()
        {
            tailBackup = snake.TailBackup;
            snakeElements.Add(new Snake() { X = tailBackup.X, Y = tailBackup.Y });
        }

        //Збільше кількості балів після кожного з'їденого яблука
        public void Score()
        {
            snakeField.ScoreGame += 10 - timeNewApples;
            snakeField.Score.Content = $"Очки: {snakeField.ScoreGame}";
        }

        //Метод для завершення гри і переходу на головне вікно
        public void LosingGame(string reason)
        {
            StopGame();
            if (!FirstPlace(snakeField.ScoreGame, snakeField.UserName))
            {
                MessageBox.Show($"Ви програли! {reason}");
            }
            FileWrite(snakeField.ScoreGame, snakeField.UserName);
            snakeField.MenuOpen();

        }
      
        public void PauseGame()
        {
            MainTimer.Stop();
            TimerForExtraApple.Stop();
            TimerForLabel.Stop();
        }

        public void ContinueGame()
        {
            MainTimer.Start();
            TimerForExtraApple.Start();
            TimerForLabel.Start();
        }

        public void WinnerGame()
        {
            MessageBox.Show("Вітаю! Ви пройшли складний рівень");
            FileWrite(snakeField.ScoreGame, snakeField.UserName);
            snakeField.MenuOpen();
        }
    }
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down,

    }



}
