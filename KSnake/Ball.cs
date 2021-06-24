using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace KSnake
{
    public class Ball: GameElements, IDraw<Ball>
    {
        public Ball()
        {
            Image img = new Image();
            img.Source = BitmapFrame.Create(new System.Uri(@"D:\Навчання\Visual (education)\KSnake\Pictures\ball.png", System.UriKind.Relative));
            img.Width = SizeCell;
            img.Height = SizeCell;
            UIElement = img;
        }

        //Реалізація інтерфейсу  
        public Ball Draw(SnakeField snakeField)
        {
           
            Ball ball = new Ball();
            ball.X = 3 * SizeCell;
            ball.Y = 3 * SizeCell;
            snakeField.Snake_Game.Children.Add(ball.UIElement);
            Canvas.SetLeft(ball.UIElement, ball.X);
            Canvas.SetTop(ball.UIElement, ball.Y);
            return ball;
        }

        //Зміна координат руху м'яча
        private void MoveDirectionBall(Direction balldirection,Ball ball)
        {
           
            switch (balldirection)  
            {
                case Direction.Up:
                    ball.Y -= SizeCell;
                    ball.X -= SizeCell;
                    break;
                case Direction.Down:
                    ball.Y += SizeCell;
                    ball.X -= SizeCell;
                    break;
                case Direction.Left:
                    ball.Y -= SizeCell;
                    ball.X += SizeCell;
                    break;
                case Direction.Right:
                    ball.Y += SizeCell;
                    ball.X += SizeCell;
                    break;
                default:
                    break;
            }

        }

        //Рух м'яча та зіткнення його з стіною
        public Direction MoveBall(List<Wall> walls, Ball ball, Direction balldirection,Door door, bool openDoor)
        {
            MoveDirectionBall(balldirection, ball);

            foreach (var wall in walls)
            {
                //Якщо м'яч влучив у нижню сторону стіни 
                if (ball.Y == wall.Y - SizeCell && ball.X == wall.X)
                {
                    SoundGameElements();
                    if (balldirection == Direction.Right)
                    {
                        balldirection = Direction.Left;
                    }
                    else if (balldirection == Direction.Down)
                    {
                        balldirection = Direction.Up;
                    }

                }
                //Якщо м'яч влучив у верхню сторону стіни
                else if (ball.Y == wall.Y + SizeCell && ball.X == wall.X)
                {
                    SoundGameElements();
                    if (balldirection == Direction.Left)
                    {
                        balldirection = Direction.Right;
                    }
                    else if (balldirection == Direction.Up)
                    {
                        balldirection = Direction.Down;
                    }
                }
                //Якщо м'яч влучив у праву сторону стіни
                else if (ball.Y == wall.Y && ball.X == wall.X - SizeCell)
                {
                    SoundGameElements();
                    if (balldirection == Direction.Left)
                    {
                        balldirection = Direction.Up;
                    }
                    else if (balldirection == Direction.Right)
                    {
                        balldirection = Direction.Down;
                    }
                }
                //Якщо м'яч влучив у праву сторону стіни
                else if (ball.Y == wall.Y && ball.X == wall.X + SizeCell)
                {
                    SoundGameElements();
                    if (balldirection == Direction.Down)
                    {
                        balldirection = Direction.Right;
                    }
                    else if (balldirection == Direction.Up)
                    {
                        balldirection = Direction.Left;
                    }
                }
            }
            //Якщо м'яч влучив у закриті двері
            if ((door.X == ball.X || door.X + SizeCell == ball.X) && door.Y + 3 * SizeCell == ball.Y && !openDoor)
            {
                SoundGameElements();
                if (balldirection == Direction.Left)
                    balldirection = Direction.Right;
                else if (balldirection == Direction.Up)
                    balldirection = Direction.Down;
            }

            Canvas.SetLeft(ball.UIElement, ball.X);
            Canvas.SetTop(ball.UIElement, ball.Y);
            return balldirection;

        }

        //Перевіряє зіткнення м'яча з головою змійки
        public bool CollisionSnakeHeadWithBall(List<Snake> snakeElements,Ball ball, Direction currentDirection)
        {
            if (ball == null)
            { 
                return false; 
            }
            Snake snakehead = snakeElements[0];
            //Якщо м'яч влучив у голову змійки, яка знаходилася у горизонтальному положені
            if ((snakehead.Y + SizeCell == ball.Y || snakehead.Y - SizeCell == ball.Y) && (currentDirection == Direction.Right && 
                snakehead.X - SizeCell == ball.X  || currentDirection == Direction.Left && snakehead.X + SizeCell == ball.X))
            {
                return true;
            }
            //Якщо м'яч влучив у голову змійки, яка знаходилася у вертикальному положені
            else if ((snakehead.X + SizeCell == ball.X || snakehead.X - SizeCell == ball.X) && (currentDirection == Direction.Up &&
                snakehead.Y + SizeCell == ball.Y || currentDirection == Direction.Down && snakehead.Y - SizeCell == ball.Y))
            {
                return true;
            }
            //Якщо м'яч рухається назустріч голови змійки
            else if (snakehead.X == ball.X && snakehead.Y == ball.Y)
            {
                return true;
            }
            return false;
        }
        public override string Message() => "М'яч потрапив у голову";
        
    }
}
