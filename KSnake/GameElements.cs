using System;
using System.Windows;
using System.Windows.Media;

namespace KSnake
{
    public abstract class GameElements
    {
      
        public UIElement UIElement { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int SizeCell { get => 20; }

        //Відтворює музику при програші 
        public void LosingGameMusic() 
        {
            MediaPlayer soundGameOver = new MediaPlayer();
            soundGameOver.Open(new Uri(@"D:\Навчання\Visual (education)\KSnake\Music\Collision.mp3", UriKind.Relative));
            soundGameOver.Play();
        }
        //Метод для виведення на екран діалогового вікна з причиною програшу
        public abstract string Message();

        //Метод для відтворення різних звуків 
        public virtual void SoundGameElements()
        {
            MediaPlayer soundEat = new MediaPlayer();
            soundEat.Open(new Uri(@$"D:\Навчання\Visual (education)\KSnake\Music\SoundBall.mp3", UriKind.Relative));
            soundEat.Play();
        }
    }
}
