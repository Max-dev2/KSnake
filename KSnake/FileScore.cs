using System.IO;
using System.Windows;

namespace KSnake
{
    class FileScore
    {     
        //Метод для читання файлу зі збереженими результатами
        private string[] FileReader()
        {

            string path = @"D:\Навчання\Visual (education)\KSnake\MaxScore.txt";

            string[] lines = File.ReadAllLines(path);
            return lines;
        }

        //Пошук імені користувача для перезаписування його кількості набраних балів 
        private bool SearchName(string row, int score, string name)

        {
            string[] lines = FileReader();
            
            for (int i = 0; i < lines.Length; i++)
            {
                string[] names = lines[i].Split(": ");
                if (names[0] == name)
                {
                    if (score > int.Parse(names[1]))
                    {
                        lines[i] = lines[i].Replace(lines[i], row);
                        File.WriteAllLines(@"D:\Навчання\Visual (education)\KSnake\MaxScore.txt", lines);
                    }
                    return true;

                }
            }
            return false;

        }

        //Перебіряє чи більше набраних балів гравця за користувача на першому місці
        public bool FirstPlace(int score, string name)
        {
            //Якщо файл порожній, то записує нового гравця на перше місце
            if (FileReader()[0] == "-1")
            {
                StreamWriter ScoreWrite = new StreamWriter(@"D:\Навчання\Visual (education)\KSnake\MaxScore.txt");
                ScoreWrite.WriteLine(name + ": " + score);
                ScoreWrite.Close();
                return false;
            }
            int maxScore = int.Parse(FileReader()[0].Split(": ")[1]);
           
            if (maxScore < score)
            {
                return true;
            }
            return false;

        }

        //Сортує гравців за кількістю набрних балів
        private void SortScore()
        {
            string[] lines = FileReader();
            for (int i = 1; i < lines.Length; i++)
            {
                for (int j = 0; j < lines.Length - i; j++)
                {
                    if (int.Parse(lines[j].Split(": ")[1]) < int.Parse(lines[j + 1].Split(": ")[1]))
                    {
                        string temp = lines[j];
                        lines[j] = lines[j + 1];
                        lines[j + 1] = temp;
                    }
                }
            }
            File.WriteAllLines(@"D:\Навчання\Visual (education)\KSnake\MaxScore.txt", lines);
            return;
        }

        //Запис даних про нового гравця у файл
        public void FileWrite(int score, string name)
        {
            
            string fileRow = name + ": " + score;
            if (FirstPlace(score,name))
            {
                MessageBox.Show("Вітаю! Ви посіли перше місце у рейтингу");
            }
            if (SearchName(fileRow, score, name))
            {
                SortScore();
            }
            else
            {
                StreamWriter ScoreWrite = new StreamWriter(@"D:\Навчання\Visual (education)\KSnake\MaxScore.txt", true);
                ScoreWrite.WriteLine(fileRow);
                ScoreWrite.Close();
                SortScore();
            }

        }
    }
}
