using System.IO;
using System.Windows;

namespace KSnake
{

    public partial class HighScores : Window
    {
        public HighScores()
        {
            InitializeComponent();
        }
        //Читання файлу з результатами гравців
        public string[] ReadAllUsers()
        {
            string path = @"D:\Навчання\Visual (education)\KSnake\MaxScore.txt";
            string[] lines = File.ReadAllLines(path);
            return lines;
        }
        //Виведення на екран Топ-10
        public void OpenTop()
        {
            string[] lines = ReadAllUsers();
            //Виведення перших 10 найкраших гравців
            for (int i = 0; i < lines.Length; i++)
            {
                if(i == 10 || lines[0] == "-1")
                {
                    break;
                }
                Player.Content += $"{lines[i].Split(": ")[0]}\n";
                Scores.Content += $"{lines[i].Split(": ")[1]}\n";
                NumberOfPlayers.Content += $"{i+1}.\n";
            }
        }
        //Подія, яка спрацьовує при натиснені кнопки "Очистити список"
        private void ClickRemoveHighScore(object sender, RoutedEventArgs e)
        {
            string path = @"D:\Навчання\Visual (education)\KSnake\MaxScore.txt";
            //Записує, замість всіх користувачів, "-1" у файл
            File.WriteAllText(path,"-1");
            Player.Content = "";
            Scores.Content = "";
            NumberOfPlayers.Content = "";
        }
    }
    
}
