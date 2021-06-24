
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace KSnake
{
    public class Door : GameElements, IDraw<Door>
    {

        public Door()
        {
            Image img = new Image();
            img.Source = BitmapFrame.Create(new System.Uri(@"D:\Навчання\Visual (education)\KSnake\Pictures\door.png",
                System.UriKind.Relative));
            img.Width = SizeCell * 2;
            img.Height = SizeCell * 3;
            Border border = new Border();
            border.Child = img;
            UIElement = border;

        }

        //Анімація відкриття дверей 
        public void Animation(Door door)
        {
            //Відтворення звуку під час відкриття дверей
            SoundGameElements();
            DoubleAnimation doorAnimation = new DoubleAnimation();
            doorAnimation.From = SizeCell * 2;
            doorAnimation.To = 10;

            doorAnimation.Duration = TimeSpan.FromSeconds(3);
            door.UIElement.BeginAnimation(Button.WidthProperty, doorAnimation);

        }
        public override void SoundGameElements()
        {
            MediaPlayer soundEat = new MediaPlayer();
            soundEat.Open(new Uri(@"D:\Навчання\Visual (education)\KSnake\Music\OpenDoor.mp3", UriKind.Relative));
            soundEat.Play();
        }

        //Реалізація інтерфейсу 
        public Door Draw(SnakeField snakeField)
        {
            Door door = new Door() { X = 21 * SizeCell, Y = 0 };
            snakeField.Snake_Game.Children.Add(door.UIElement);
            Canvas.SetLeft(door.UIElement, door.X);
            Canvas.SetTop(door.UIElement, door.Y);
            return door;
        }

        //Перевіряє зіткнення голови змійки з дверима 
        public bool CollisionWithDoor(List<Snake> snakeElements, Door door, bool checkOpenDoor)
        {

            Snake snakehead = snakeElements[0];
            if ((door.X == snakehead.X || door.X + SizeCell == snakehead.X) && door.Y + 2 * SizeCell == snakehead.Y && !checkOpenDoor)
                return true;
            return false;
        }

        public override string Message() => "Ви врізалися в двері";

    }
}
