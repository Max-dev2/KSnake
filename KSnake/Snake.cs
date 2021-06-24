using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KSnake
{
    public class Snake : GameElements, IDrawList<Snake>
    {
        public Snake TailBackup { get; set; }
        public bool IsHead { get; set; }
        public Image HeadRotate { get; private set; }

        //Конструктор для створення голови змійки
        public Snake(bool head)
        {
            Image imgSnake = new Image();

            imgSnake.Source = BitmapFrame.Create(new System.Uri(@"D:\Навчання\Visual (education)\KSnake\Pictures\SnakeHead.png",
                System.UriKind.Relative));
            imgSnake.Width = SizeCell + 1;
            imgSnake.Height = SizeCell + 1;
            
            UIElement = imgSnake;
            HeadRotate = imgSnake;
        }
        //Конструктор для створення тіла змійки
        public Snake()
        {
            Rectangle snakeBody = new Rectangle();
            snakeBody.Width = SizeCell +  1;
            snakeBody.Height = SizeCell + 1;
            snakeBody.Fill = Brushes.Green;
            UIElement = snakeBody;

        }
        
        public List<Snake> InitializeSnake(List<Snake> snakeElements,int row, int numberElementBackUpSnake)
        {
            //Добавляння голови змійки
            snakeElements.Add(new Snake(true)
            {
                X = 5 * SizeCell,
                Y = (row - 5) * SizeCell,
                IsHead = true
            });
            //Добавляння тіла змійки
            for (int i = 1; i < numberElementBackUpSnake ; i++)
            {
                
                snakeElements.Add(new Snake()
                {
                    X = (5 + i) * SizeCell,
                    Y = ((row - 5 ) * SizeCell) ,
                    IsHead = false
                }) ;
            }
            return snakeElements;
        }
        //Реалізація інтерфейсу для відображення змійки на ігровому полі
        public List<Snake> DrawList(SnakeField snakeField, List<Snake> snakeElements)
        {
            foreach (var element in snakeElements)  
            {
                if (!snakeField.Snake_Game.Children.Contains(element.UIElement))
                {
                    snakeField.Snake_Game.Children.Add(element.UIElement);
                }
                Canvas.SetLeft(element.UIElement, element.X);
                Canvas.SetTop(element.UIElement, element.Y);
            }
            return snakeElements;
        }
        //Зміна координат змійки відповідно до вибраного напрямку
        private void MoveDirection(Direction directrion, Snake head)
        {
           
            switch (directrion)
            {
                case Direction.Up:
                    head.Y -= SizeCell;
                    RotateSnake(head, 180);
                    break;
                case Direction.Down:
                    head.Y += SizeCell;
                    RotateSnake(head, 0);
                    break;
                case Direction.Left:
                    head.X -= SizeCell;
                    RotateSnake(head, 90);
                    break;
                case Direction.Right:
                    head.X += SizeCell;
                    RotateSnake(head, 270);
                    break;
                default:
                    break;
            }
        }
        //Повертає голову змійки відповідно до напрямку 
        private void RotateSnake(Snake head, int angle)
        {
            if (head.HeadRotate != null)
            {
                head.HeadRotate.LayoutTransform = new RotateTransform(angle);
                head.UIElement = head.HeadRotate;
            }
    
        }

        //Рух змійки
        public Direction MoveSnake(List<Snake> snakeElements, Direction directrion)
        {
            Snake headFirst = snakeElements[0];
            Snake tail = snakeElements[snakeElements.Count - 1];
            TailBackup = new Snake()
            {
                X = tail.X,
                Y = tail.Y
            };
            //Запам'ятовуємо координати голови змійки
            tail.X = headFirst.X;
            tail.Y = headFirst.Y;
            MoveDirection(directrion,headFirst);
            //Видалення хвоста змійки
            snakeElements.RemoveAt(snakeElements.Count - 1);
            //Видалення голови змійки
            snakeElements.RemoveAt(0);
            //Ставимо хвіст замість голови
            snakeElements.Insert(0, tail);
            //Ставимо голову спереду
            snakeElements.Insert(0, headFirst);
            return directrion;
        }

        //Перевіряє зіткнення голови змійки з тілом
        public bool CollisionWithSelf(List<Snake> snakeElements)
        {
            Snake snakehead = snakeElements[0];

            if (snakehead != null)
            {
                foreach (var snakeelement in snakeElements)
                {
                    if (!snakeelement.IsHead)
                    {
                        if (snakeelement.X == snakehead.X && snakeelement.Y == snakehead.Y)
                        {
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        //Рух змійки біля відкритих дверей
        public Direction FinalMoveSnake(List<Snake> snakeElements, Direction directrion)
        {
            Snake headFirst = snakeElements[0];
            Snake tail = snakeElements[snakeElements.Count - 1];
            headFirst.IsHead = false;
            tail.IsHead = true;
            //Запам'ятовуємо координати голови змійки
            tail.X = headFirst.X;
            tail.Y = headFirst.Y;
            MoveDirection(directrion, tail);
            //Видалення хвоста змійки
            snakeElements.RemoveAt(snakeElements.Count - 1);
            //Ставимо хвіст замість голови
            snakeElements.Insert(0, tail);
            return directrion;
        }

       //Перевіряє зіткнення м'яча з тілом змійки
        public Direction CollisionBallWithSnakeBody(List<Snake> snakeElements, Ball ball, Direction ballDirection)
        {
            
            foreach (var snakeElement in snakeElements)
            {
                if (!snakeElement.IsHead)
                {
                    //Якщо м'яч влучив у нижню сторону змійки
                    if (ball.Y == snakeElement.Y - SizeCell && ball.X == snakeElement.X)
                    {
                        SoundGameElements();
                        if (ballDirection == Direction.Right)
                            ballDirection = Direction.Left;
                        else if (ballDirection == Direction.Down)
                            ballDirection = Direction.Up;
                    }
                    //Якщо м'яч влучив у верхню сторону змійки
                    else if (ball.Y == snakeElement.Y + SizeCell && ball.X == snakeElement.X)
                    {
                        SoundGameElements();
                        if (ballDirection == Direction.Left)
                            ballDirection = Direction.Right;
                        else if (ballDirection == Direction.Up)
                            ballDirection = Direction.Down;
                    }
                    //Якщо м'яч влучив у праву сторону змійки
                    else if (ball.Y == snakeElement.Y && ball.X == snakeElement.X - SizeCell)
                    {
                        SoundGameElements();
                        if (ballDirection == Direction.Left)
                            ballDirection = Direction.Up;
                        else if (ballDirection == Direction.Right)
                            ballDirection = Direction.Down;
                    }
                    //Якщо м'яч влучив у ліву сторону змійки
                    else if (ball.Y == snakeElement.Y && ball.X == snakeElement.X + SizeCell)
                    {
                        SoundGameElements();
                        if (ballDirection == Direction.Down)
                            ballDirection = Direction.Right;
                        else if (ballDirection == Direction.Up)
                            ballDirection = Direction.Left;
                    }
                }
            }
            Canvas.SetLeft(ball.UIElement, ball.X);
            Canvas.SetTop(ball.UIElement, ball.Y);
            return ballDirection;
        }
        public override string Message() => "Ви врізалися в себе";
       

    }
}
