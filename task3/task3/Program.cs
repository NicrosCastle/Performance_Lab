using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace task3
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            //читаем ссылку на каталог, читаем данные из файлов и сохраняем в массив
            string[] paths = Directory.GetFiles(args[0]);
            string[] files = new string[5];
            for (int i = 0; i < paths.Length; i++)
            {
                using (FileStream fStream = File.OpenRead(paths[i]))
                {
                    byte[] array = new byte[fStream.Length];
                    fStream.Read(array, 0, array.Length);
                    files[i] = Encoding.Default.GetString(array);
                }
            }

            //создаем двумерный массив [количество файлов, количество значений], преобразуем значения
            float[,] values = new float[5, 16];
            for (int i = 0; i < files.Length; i++)
            {
                //сделано как в первом задании, понято как "входящие данные без \n в конце строк"
                string[] value = files[i].Replace("\r\n", " ").Substring(0).Split(' ');
                for (int j = 0; j < value.Length; j++)
                {
                    values[i, j] = float.Parse(value[j]);
                }
            }

            //создаем массив средних значений по всем кассам за каждый период времени
            float[] averageOfAllCash = new float[16];
            for (int i = 0; i < values.GetLength(1); i++)
            {
                float sumOfAllCash = 0f;
                for (int j = 0; j < values.GetLength(0); j++)
                {
                    sumOfAllCash += values[j, i];
                }
                averageOfAllCash[i] = sumOfAllCash / 5f;
            }

            //выясняем период времени, в котором наибольшее среднее значение посетителей, выводим результат
            int indexOfMaximumVisitors = 0;
            for (int i = 1; i < averageOfAllCash.Length; i++)
            {
                if (averageOfAllCash[indexOfMaximumVisitors] < averageOfAllCash[i]) indexOfMaximumVisitors = i;
            }
            Console.WriteLine($"{indexOfMaximumVisitors + 1}");
        }
    }
}
