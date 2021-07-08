using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace task
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            string textFromFile;
            using (FileStream fStream = File.OpenRead(args[0]))
            {
                byte[] array = new byte[fStream.Length];
                fStream.Read(array, 0, array.Length);
                textFromFile = Encoding.Default.GetString(array);
            }
            string[] inputValues = textFromFile.Replace("\r\n", " ").Substring(0).Split(' ');
            double[] values = new double[inputValues.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = double.Parse(inputValues[i]);
            }
            Percentile(values, 90);
            Mediana(values);
            MaxValue(values);
            MinValue(values);
            AverageValue(values);
        }



        //вычисление и вывод 90 процентиля
        static void Percentile(double[] values, int percentile)
        {
            Array.Sort(values);
            double rank = percentile / 100.0 * (values.Length - 1) + 1;
            int index = (int)rank;
            double fractional = rank - index;
            double percentileValue = values[index-1] + fractional * (values[index] - values[index - 1]);
            Console.WriteLine(percentileValue.ToString("f2"));
        }

        //вычисление и вывод медианы
        static void Mediana(double[] values)
        {
            double sumAllValues = 0;
            double medianaSum = 0;
            foreach (double item in values)
            {
                sumAllValues += item;
            }
            for (int i = 0; i < values.Length; i++)
            {
                medianaSum += values[i];
                if (medianaSum > sumAllValues / 2.0) 
                {
                    Console.WriteLine(values[i - 1].ToString("f2"));
                    break;
                }
            }
        }

        //вычисление и вывод максимального значения
        static void MaxValue(double[] values)
        {
            double maxValue = double.MinValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (maxValue < values[i]) maxValue = values[i];
            }
            Console.WriteLine(maxValue.ToString("f2"));
        }

        //вычисление и вывод минимального значения
        static void MinValue(double[] values)
        {
            double minValue = double.MaxValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (minValue > values[i]) minValue = values[i];
            }
            Console.WriteLine(minValue.ToString("f2"));
        }

        //вычисление и вывод среднего значения
        static void AverageValue(double[] values)
        {
            double averageValue = 0;
            foreach (double item in values)
            {
                averageValue += item;
            }
            Console.WriteLine((averageValue / values.Length).ToString("f2"));
        }
    }

}
