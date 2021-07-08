using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace task4
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            //читаем данные из файла
            string textFromFile;
            using (FileStream fStream = File.OpenRead(args[0]))
            {
                byte[] array = new byte[fStream.Length];
                fStream.Read(array, 0, array.Length);
                textFromFile = Encoding.Default.GetString(array);
            }

            //сделано как в первом задании, понято как "входящие данные без \n в конце строк"
            string[] values = textFromFile.Replace("\r\n", " ").Replace(":", ".").Substring(0).Split(' ');

            //добавляем куски времени в словарь, отсчет ключей от 1, нечетные ключи - вход посетителя, четные - выход
            Dictionary<int, float> dict = new Dictionary<int, float>(values.Length);
            for (int i = 0; i < values.Length; i++)
            {
                dict.Add(i + 1, float.Parse(values[i]));
            }

            //сортируем словарь по времени от меньшего к большему, выясняем максимальное кол-во посетителей, 
            //в лист сохраняем порядок временных отрезков по ключам 
            int counter = 0;
            int maxCounter = 0;
            ArrayList index = new ArrayList();
            foreach (var pair in dict.OrderBy(pair => pair.Value))
            {
                if (pair.Key % 2 == 1) counter++;
                else counter--;
                if (counter > maxCounter) maxCounter = counter;
                index.Add(pair.Key);
            }

            //добавляем в лист 2 временных отрезка при максимальном кол-ве посетителей:
            //начальный отрезок времени добавляется при максимальное кол-во посетителей,
            //конечный отрезок находится по следующиему ключу начального отрезка из листа индексов
            counter = 0;
            ArrayList result = new ArrayList();
            foreach (var pair in dict.OrderBy(pair => pair.Value))
            {
                if (pair.Key % 2 == 1) counter++;
                else counter--;
                if (counter == maxCounter)
                {
                    result.Add(float.Parse(pair.Value.ToString()));
                    result.Add(float.Parse(dict[index.IndexOf(pair.Key + 1)].ToString()));
                }
            }

            //удаляем повторяющиеся отрезки времени, например 18:00 - 18:00, если они существуют
            //склеиваем отрезки (если такие имеются), если конец первого совпадает с началом второго
            //один из удаленных отрезков всегда начальный, второй всегда конечный, т.к. индексы чет/нечет
            for (int i = 1; i < result.Count; i++)
            {
                if ((float)result[i - 1] == (float)result[i])
                {
                    result.RemoveAt(i);
                    result.RemoveAt(i - 1);
                    i-=2;
                }
            }

            //добавляем в массив результаты, преобразуем и выводим
            float[] x = new float[result.Count];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = (float)result[i];
                if (i % 2 != 0) Console.WriteLine($@"{ x[i - 1].ToString("f2").Replace(".", ":")} {x[i].ToString("f2").Replace(".", ":")}");
            }
        }
    }
}
