using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KSnake
{
    class Apple :GameElements
    {

        private int row;
        private int column;
       
        public Apple(int row, int column)
        {
            this.row = row;
            this.column = column;
            //Завантаження зображення яблука
            Image img = new Image();
            img.Source = BitmapFrame.Create(new System.Uri(@"D:\Навчання\Visual (education)\KSnake\Pictures\apple.png", System.UriKind.Relative));
            img.Width = SizeCell;
            img.Height = SizeCell;    
            UIElement = img;
            
        }

        //Метод для створення яблука 
       public List<Apple> CreateApple(List<Apple> apples,List<Snake> snakeElements, List<Wall> walls,Door door)
        {

            Random random = new Random();
            //Створює яблуко рандомно на полі
            apples.Add(new Apple(row,column)
            {

                X = random.Next(1, column) * SizeCell,
                Y = random.Next(1, row) * SizeCell
            });
            for (int i = 0; i < snakeElements.Count; i++)
            {
                //Перевіряє чи не створилося яблуко на змійці
                if (snakeElements[i].X == apples[apples.Count - 1].X && snakeElements[i].Y == apples[apples.Count - 1].Y)
                {
                    apples[apples.Count - 1].X = random.Next(1, column) * SizeCell;
                    apples[apples.Count - 1].Y = random.Next(1, row) * SizeCell;
                    i = 0;
                }
            }
            //Перевіряє чи не створилося яблуко на стіні чи на дверях
            for (int i = walls.Count - 4; i < walls.Count; i++)
            {
                if ((door.X == apples[apples.Count - 1].X || door.X + SizeCell == apples[apples.Count - 1].X)
                     && (door.Y + SizeCell == apples[apples.Count - 1].Y || door.Y + 2 * SizeCell == apples[apples.Count - 1].Y)
                     || (walls[i].X == apples[apples.Count - 1].X && walls[i].Y == apples[apples.Count - 1].Y))
                {
                    apples[apples.Count - 1].X = random.Next(1, column) * SizeCell;
                    apples[apples.Count - 1].Y = random.Next(1, row) * SizeCell;
                    i = walls.Count - 4;
                }

            }
            return apples;

        }

        //Зображує яблуко на ігровому полі
        public List<Apple> DrawApple(SnakeField snakeField, List<Apple> apples, List<Snake> snakeElements, List<Wall> walls, Door door)
        {
            apples = CreateApple(apples, snakeElements, walls, door);
            foreach (var ap in apples)
            {
                if (!snakeField.Snake_Game.Children.Contains(ap.UIElement))
                {
                    snakeField.Snake_Game.Children.Add(ap.UIElement);
                }
                Canvas.SetLeft(ap.UIElement, ap.X);
                Canvas.SetTop(ap.UIElement, ap.Y);
            }
            return apples;

        }

        //Перевіряє зіткнення яблука зі змійкою
        public bool ChechCollisionWithApple(List<Snake> snakeElements, SnakeField snakeField, List<Apple> apples)
        {
            Snake head = snakeElements[0];
            for (int i = 0; i < apples.Count; i++)
            {
                if (head.X == apples[i].X && head.Y == apples[i].Y)
                {
                    SoundGameElements();
                    snakeField.Snake_Game.Children.Remove(apples[i].UIElement);
                    apples.Remove(apples[i]);
                    return true;
                }
            }
            return false;

        }

        //Відтворення звуку при поїданні яблука
        public override void SoundGameElements()
        {
            MediaPlayer soundEat = new MediaPlayer();
            soundEat.Open(new Uri(@"D:\Навчання\Visual (education)\KSnake\Music\SoundEatApple.mp3", UriKind.Relative));
            soundEat.Play();
        }
     
        public override string Message() => "Змійка з'їла яблуко";

      
    }
    
}
