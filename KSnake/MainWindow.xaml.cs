using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace KSnake
{

    public partial class MainSnakeWindow : Window
    {
        private MediaPlayer player = new MediaPlayer();
        public MainSnakeWindow()
        {   
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(KeyPress);

        }

        //Обробник подій для кнопки, яка розпочинає гру. 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text.Length == 0)
            {
                MessageBox.Show("Введіть ім'я, щоб розпочати гру");
                return;
            }
            //Перевірка, який рівень вибрано у RadioButton 
            if ((bool)Easy.IsChecked)
                ChooseLevel(10);
            else if ((bool)Medium.IsChecked)
                ChooseLevel(20);
            else if ((bool)Hard.IsChecked)
                ChooseLevel(30);
        }
        //Відкриває ігрове вікно для вибраного рівня
        private void ChooseLevel(int choose)
        {
            SnakeField Level = new SnakeField(choose, UserName.Text);
            Level.Show();
            this.Hide();
        }
       
        private void KeyPress(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    this.HelpText();
                    break;
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
        //Метод для створення вікна з інструкцією для користувача
        public void HelpText()
        {
            //Читає файл з інструкцією
            string path = @"D:\Навчання\Visual (education)\KSnake\Instruction.txt";
            var text = File.ReadAllText(path);
            //Створення нового вікна
            var TextWindow = new Window();
            TextWindow.Title = "Instruction";
            TextWindow.Width = 420;
            TextWindow.Height = 400;
            TextWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            TextWindow.Background = Brushes.LawnGreen;
            TextWindow.ResizeMode = ResizeMode.NoResize;

            Grid rules = new Grid();
            //запис даних з файлу у нове вікно
            TextBlock textBlock = new TextBlock(new Run(text));
            textBlock.FontFamily = new FontFamily("Monotype Corsiva");
            textBlock.FontSize = 18;
            textBlock.FontWeight = FontWeights.Bold;
            rules.Children.Add(textBlock);
            ScrollViewer scroll = new ScrollViewer();
            scroll.Content = rules;
            TextWindow.Content = scroll;
            TextWindow.ShowDialog();

        }
        //Обробник подій для відкриття вікна з інструкцією
        private void Instruction(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HelpText();
            this.Show();
        }
        //Обробник подій для відкриття Топ-10 гравців
        private void HighScore(object sender, RoutedEventArgs e)
        {
            this.Hide();

            player.Open(new Uri(@"D:\Навчання\Visual (education)\KSnake\Music\Champions.mp3", UriKind.Relative));
            player.Play();

            HighScores top = new HighScores();
            top.OpenTop();
            top.ShowDialog();
            player.Stop();
            this.Show();    
        }
        private void Close_Window(object sender, RoutedEventArgs e)
        { 
            this.Close();
        }
        
        private void TextEnd(object sender, KeyEventArgs e)
        {
            //Видаляє пробіли з імені користувача
            UserName.Text = UserName.Text.Replace(" ", "");
            UserName.MaxLength = 15;

            if (UserName.Text.Length == 15)
            {
                MessageBox.Show("Довжина імені не повинна перевищувати 15 символів");
            }

            if (e.Key == Key.Enter)
            {
                if (CheckSameName(UserName.Text))
                {
                    StartGame.IsEnabled = true;
                    StartGame.Focus();
                    UserName.IsEnabled = false;
                }
            }
        }
        //Перевіряє ім’я користувача на повторення в файлі. 
        private bool CheckSameName(string user)
        {
            HighScores top = new HighScores();
            string[] lines = top.ReadAllUsers();
            foreach (var line in lines)
            {
                if (user == line.Split(": ")[0])
                {
                    MessageBoxResult option = MessageBox.Show("Користувач з таким іменем вже існує! Якщо ви хочете продовжити під цим ім'ям," +
                       " натисніть 'Так', якщо змінити ім'я, натисніть 'Ні' ", "Увага!", MessageBoxButton.YesNo);
                    if (option == MessageBoxResult.Yes)
                    {
                        return true;
                    }
                    return false;

                }
            }
            return true;
        }

      
    }
}
