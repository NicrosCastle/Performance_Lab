using System;
using System.Globalization;

namespace task
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            short[] inputValues = InputData();
            Percentile(inputValues, 90);
            Mediana(inputValues);
            MaxValue(inputValues);
            MinValue(inputValues);
            AverageValue(inputValues);
        }

        //получение данных
        static short[] InputData()
        {
            string[] inputValues = new string[1000];
            string inputValue;
            int counter = 0;
            for (int i = 0; i < 1000; i++)
            {
                inputValue = Console.ReadLine();
                if (inputValue.Contains("\\n")) inputValue = inputValue.Substring(0, inputValue.IndexOf('\\'));
                if (inputValue != String.Empty)
                {
                    inputValues[i] = inputValue;
                    counter++;
                }
                else break;
            }
            short[] values = new short[counter];
            for (int i = 0; i < counter; i++)
            {
                values[i] = short.Parse(inputValues[i]);
            }
            return values;
        }

        //вычисление и вывод 90 процентиля
        static void Percentile(short[] values, int percentile)
        {
            Array.Sort(values);
            double rank = percentile / 100.0 * (values.Length - 1) + 1;
            int index = (int)rank;
            double fractional = rank - index;
            double percentileValue = values[index-1] + fractional * (values[index] - values[index - 1]);
            Console.WriteLine(percentileValue.ToString("f2") + "\\n");
        }

        //вычисление и вывод медианы
        static void Mediana(short[] values)
        {
            int sumAllValues = 0;
            int medianaSum = 0;
            foreach (short item in values)
            {
                sumAllValues += item;
            }
            for (int i = 0; i < values.Length; i++)
            {
                medianaSum += values[i];
                if (medianaSum > sumAllValues / 2.0) 
                {
                    Console.WriteLine(values[i - 1].ToString("f2") + "\\n");
                    break;
                }
            }
        }

        //вычисление и вывод максимального значения
        static void MaxValue(short[] values)
        {
            short maxValue = short.MinValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (maxValue < values[i]) maxValue = values[i];
            }
            Console.WriteLine(maxValue.ToString("f2") + "\\n");
        }

        //вычисление и вывод минимального значения
        static void MinValue(short[] values)
        {
            short minValue = short.MaxValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (minValue > values[i]) minValue = values[i];
            }
            Console.WriteLine(minValue.ToString("f2") + "\\n");
        }

        //вычисление и вывод среднего значения
        static void AverageValue(short[] values)
        {
            double averageValue = 0;
            foreach (double item in values)
            {
                averageValue += item;
            }
            Console.WriteLine((averageValue / values.Length).ToString("f2") + "\\n");
        }
    }

}
