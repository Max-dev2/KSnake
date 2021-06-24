using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace KSnake
{

    public partial class SnakeField : Window
    {
        private GameWorld game;
        private MediaPlayer gameMusic = new MediaPlayer();
        private DispatcherTimer MainTimer = new DispatcherTimer();
        public int ChooseNumberApples { get; set; }
        public int ScoreGame { get; set; }
        private bool checkSlider = true;
        private bool time = true;
        public string UserName { get; set; }


        private bool checkMusic = true;
        private int numberSnakeElements = 3;
        public int NumberSnakeElements { get => numberSnakeElements; set => numberSnakeElements = value; }

        //Відкриває ігрове вікно вибране користувачем
        public SnakeField(int CounterApples, string name)
        {
            InitializeComponent();
            this.ChooseNumberApples = CounterApples;
            UserName = name;
            GameSound();
        }

        //Конструктор для переходу на новий рівень
        public SnakeField(int сounterApples, int score, string name, int NumberSnakeElements)
        {
            this.ChooseNumberApples = сounterApples;
            this.ScoreGame = score;
            UserName = name;
            this.NumberSnakeElements = NumberSnakeElements;
            InitializeComponent();
            GameSound();
            TextLevelAnimation();
        }

        //Виникає для відображення вікна з різними елементами 
        protected override void OnContentRendered(EventArgs e)
        {
            //Створення екземпляр класу для ігрової логіки
            game = new GameWorld(this); ;
            //Робить затримку клавіш
            TimerKeyDelay();
            //Відображення лічильників на екрані
            EatApple.Content = $"Залишилося з'їсти яблук: {ChooseNumberApples}";
            TimeEat.Content = $"До додаткового яблука: ";
            Score.Content = $"Очки: {ScoreGame}";
            LabelLevel();
            base.OnContentRendered(e);
        }

        //Відображення вибраного рівня на екрані
        private void LabelLevel()
        {
            if (ChooseNumberApples == 10)
            {
                Level.Content = $"Рівень: Легкий";
            }
            else if (ChooseNumberApples == 20)
            {
                Level.Content = $"Рівень: Середній";
            }
            else
            {
                Level.Content = $"Рівень: Складний";
            }
        }

        //Анімація тексту, який повідомляє про новий рівень
        private void TextLevelAnimation()
        {
            LevelAnimation.Visibility = Visibility.Visible;
            DoubleAnimation buttonAnimation = new DoubleAnimation();
            buttonAnimation.From = 0;
            buttonAnimation.To = 250;

            buttonAnimation.Duration = TimeSpan.FromSeconds(2);
            buttonAnimation.Completed += (s, e) => LevelAnimation.Visibility = Visibility.Hidden;
            LevelAnimation.BeginAnimation(Label.WidthProperty, buttonAnimation);
        }

        //Відтворення фонової музики
        private void GameSound()
        {
            gameMusic.Open(new Uri(@"D:\Навчання\Visual (education)\KSnake\Music\GameMusic.mp3", UriKind.Relative));
            gameMusic.MediaEnded += new EventHandler(MediaRepeat);
            gameMusic.Play();
            gameMusic.Volume = 0.1;
        }

        //Повторення музики 
        private void MediaRepeat(object sender, EventArgs e)
        {
            gameMusic.Position = TimeSpan.Zero;
            gameMusic.Play();
        }

        //Створення таймеру для затримки клавіш
        //Затримка потрібна, щоб уникнути непердбачених дій користувача,
        //таких як натиснення всіх клавіш зараз
        private void TimerKeyDelay()
        {
            MainTimer.Interval = TimeSpan.FromMilliseconds(150);
            MainTimer.Tick += EventDelayKey;
            MainTimer.Start();
        }

        //Спрацьовує при кожному Tick таймеру
        private void EventDelayKey(object sender, EventArgs e)
        {
            time = true;
            this.KeyDown += new KeyEventHandler(KeyPress);
        }

        //Обробник подій клавіатури
        private void KeyPress(object sender, KeyEventArgs e)
        {
            if (game.CheckRemoveSnake)
            {
                e.Handled = true;
                return;
            }
            if (time)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        if (game.snakeDirection != Direction.Down)
                        {
                            game.snakeDirection = Direction.Up;
                        }
                        break;
                    case Key.Down:

                        if (game.snakeDirection != Direction.Up)
                        {
                            game.snakeDirection = Direction.Down;
                        }
                        break;
                    case Key.Left:
                        if (game.snakeDirection != Direction.Right)
                        {
                            game.snakeDirection = Direction.Left;
                        }
                        break;
                    case Key.Right:
                        if (game.snakeDirection != Direction.Left)
                        {
                            game.snakeDirection = Direction.Right;
                        }
                        break;
                    case Key.F1:
                        game.PauseGame();
                        MainSnakeWindow menu = new MainSnakeWindow();
                        menu.HelpText();
                        game.ContinueGame();
                        break;
                    case Key.Escape:
                        game.StopGame();
                        MenuOpen();
                        break;

                }
                time = false;
            }

        }

        //Відкриває головне вікно
        public void MenuOpen()
        {
            gameMusic.Stop();
            this.Close();
            MainSnakeWindow menu = new MainSnakeWindow();
            menu.Show();
        }

        //Переходить на новий рівень
        public void NewLevel(int scoreLevel, int backupSnake)
        {
            gameMusic.Stop();
            this.Close();
            SnakeField newSnakeField = new SnakeField(ChooseNumberApples + 10, scoreLevel, UserName, backupSnake);
            newSnakeField.Show();
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {        
            game.StopGame();
            game.LosingGame("Ви завершили гру!");
           
            
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            gameMusic.Pause();

            game.PauseGame();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (checkMusic)
            {
                gameMusic.Play();

            }
            game.ContinueGame();
        }

        //Змінення швидкості відповідно до вибраної гравцем швидкості
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (!checkSlider)
            {
                game.FastGame(Slider.Value);
            }
            else
            {
                checkSlider = false;
            }
        }

        //Зупиняє або продовжує відтворення фонової музики
        private void Sound_Click(object sender, RoutedEventArgs e)
        {
            if ((string)Sound.Header == "  Вимкнути музику")
            {
                checkMusic = false;
                gameMusic.Pause();
                Sound.Header = "  Увімкнути музику";
            }
            else
            {
                checkMusic = true;
                gameMusic.Play();
                Sound.Header = "  Вимкнути музику";
            }
        }
    }
}

