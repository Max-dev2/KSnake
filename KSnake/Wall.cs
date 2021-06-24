using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace KSnake
{
    public class Wall: GameElements,IDrawList<Wall>
    {

        private int row;
        private int column;
        public Wall(int row, int column)
        {
            this.row = row;
            this.column = column;
            Border br = new Border()
            {
                Background = Brushes.Red,
                Width = SizeCell,
                Height = SizeCell,
            };

            Image img = new Image()
            {
                Source = BitmapFrame.Create(new System.Uri(@"D:\Навчання\Visual (education)\KSnake\Pictures\wall.jpg", System.UriKind.Relative)),
                Width = SizeCell * 2,
                Height = SizeCell * 2
            };
            br.Child = img;
            UIElement = br;
        
        }

        //Створення стіни 
        public List<Wall> CreateWall(List<Wall> walls)
        {
            for (int i = 0; i <= column; i++)
            {
                for (int j = 0; j <= row; j++)
                {
                    if (i > 0 && (j == 0 || j == row) || i == column || i == 0)
                    {
                        //стіна не створюється на місці дверей
                        if ((i == 21 || i == 22) && j == 0)
                            continue;
                        walls.Add(new Wall(row, column)
                        {
                            X = i * SizeCell,
                            Y = j * SizeCell
                        });
                    }
                }
            }
            //створення стіни навколо дверей 
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 20; j <= 23; j += 3)
                {
                    walls.Add(new Wall(row, column)
                    {
                        X = j * SizeCell,
                        Y = i * SizeCell
                    });
                }
            }
            return walls;
        }

        //Реалізація інтерфейсу для відображення стіни
        public List<Wall> DrawList(SnakeField snakeField, List<Wall> walls)
        { 
            walls = CreateWall(walls);
            foreach (var elementWall in walls)
            {
                if (!snakeField.Snake_Game.Children.Contains(elementWall.UIElement))
                {
                    snakeField.Snake_Game.Children.Add(elementWall.UIElement);
                }
                Canvas.SetLeft(elementWall.UIElement, elementWall.X);
                Canvas.SetTop(elementWall.UIElement, elementWall.Y);
            }
            return walls;
        }

        //Перевіряє зіткнення голови змійки з стіною 
        public bool CollisionWittWall(List<Snake> snakeElements, List<Wall> walls)
        {
            Snake head = snakeElements[0];
            foreach (var elementWall in walls)
            {
                if (head.X == elementWall.X && head.Y == elementWall.Y)
                {
                    return true;
                }
            }
            return false;
        }
        public override string Message() => "Ви врізалися в стіну";
  


    }
}
