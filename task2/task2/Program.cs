using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace task2
{
    class Program
    {
        static void Main(string[] args)
        {
            float[,] pointsOfFigure = ReadDataFromFile(args[0]);
            float[,] points = ReadDataFromFile(args[1]);
            int[] result = CalculatingPosition(pointsOfFigure, points);
            foreach (int item in result)
            {
                Console.WriteLine($"{item-1}\\n");
            }
        }

        //чтение данных из файла и возврат массива точек
        static float [,] ReadDataFromFile(string file)
        {
            string textFromFile;
            using (FileStream fStream = File.OpenRead(file))
            {
                byte[] array = new byte[fStream.Length];
                fStream.Read(array, 0, array.Length);
                textFromFile = Encoding.Default.GetString(array);
            }
            string[] values = textFromFile.Replace("\\n\r\n", " ").Replace("\\n", String.Empty).Substring(0).Split(' ');
            float[,] arrayOfValues = new float[values.Length / 2, 2];
            int counter = 0;
            for (int i = 0; i < arrayOfValues.GetLength(0); i++)
            {
                for (int j = 0; j < arrayOfValues.GetLength(1); j++)
                {
                    arrayOfValues[i, j] = Convert.ToSingle(values[counter++], CultureInfo.InvariantCulture);
                }
            }
            return arrayOfValues;
        }

        //вычисление позиции точек
        static int[] CalculatingPosition(float[,] pointsOfFigure, float[,] points)
        {
            int[] result = new int[points.GetLength(0)];
            CheckingPointVertex(pointsOfFigure, points, result);
            CheckingPointSide(pointsOfFigure, points, result);
            CheckingPointOutsideOrInside(pointsOfFigure, points, result);
            return result;
        }

         
        //Сравниваем каждую точку с каждым углом четырехугольника
        static void CheckingPointVertex(float[,] pointsOfFigure, float[,] points, int[] result)
        {
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 0; j < pointsOfFigure.GetLength(0); j++)
                {
                    if (pointsOfFigure[j, 0] == points[i, 0] && pointsOfFigure[j, 1] == points[i, 1]) result[i] = 1;
                }
            }
        }


        /*
         * Находим длину каждой стороны четырехугольника, записываем в массив,
         * сравниваем с суммой длин от точки до углов каждой стороны,
         * если значения равны, то точка находится на стороне.
         */
        static void CheckingPointSide(float[,] pointsOfFigure, float[,] points, int[] result)
        {
            double[] side = new double[4];
            for (int i = 1; i < pointsOfFigure.GetLength(0); i++)
            {
                side[i-1] = Math.Sqrt(Math.Pow(pointsOfFigure[i,0] - pointsOfFigure[i-1,0], 2) + Math.Pow(pointsOfFigure[i, 1] - pointsOfFigure[i - 1, 1], 2));
                if (i==3) side[i] = Math.Sqrt(Math.Pow(pointsOfFigure[0, 0] - pointsOfFigure[i, 0], 2) + Math.Pow(pointsOfFigure[0, 1] - pointsOfFigure[i, 1], 2));
            }
            double firstSegment;
            double secondSegment;
            for (int i = 0; i < points.GetLength(0); i++)
            {
                for (int j = 1; j < pointsOfFigure.GetLength(0) + 1; j++)
                {
                    if (j!=4)
                    {
                        firstSegment = Math.Sqrt(Math.Pow(points[i, 0] - pointsOfFigure[j - 1, 0], 2) + Math.Pow(points[i, 1] - pointsOfFigure[j - 1, 1], 2));
                        secondSegment = Math.Sqrt(Math.Pow(pointsOfFigure[j, 0] - points[i, 0], 2) + Math.Pow(pointsOfFigure[j, 1] - points[i, 1], 2));
                    }
                    else
                    {
                        firstSegment = Math.Sqrt(Math.Pow(points[i, 0] - pointsOfFigure[j - 1, 0], 2) + Math.Pow(points[i, 1] - pointsOfFigure[j - 1, 1], 2));
                        secondSegment = Math.Sqrt(Math.Pow(pointsOfFigure[0, 0] - points[i, 0], 2) + Math.Pow(pointsOfFigure[0, 1] - points[i, 1], 2));
                    }
                    if (firstSegment + secondSegment == side[j - 1] && result[i] != 1) result[i] = 2;
                }
            }
        }



        /*
         * Находим длину диагонали четырехугольника и длины от углов диагонали до точки,
         * получаем треугольник со сторонами a,b,c, где с - диагональ четырехугольника.
         * По формуле получаем значение угла в проверяемой точке (если угол тупой - точка 
         * находится внутри фигуры, если острый - снаружи), по таблице косинусов 
         * отрицательные значения соответствуют тупому углу, т.е. точка внутри фигуры,
         * а положительные - острому, т.е. точка вне фигуры.
         */
        static void CheckingPointOutsideOrInside(float[,] pointsOfFigure, float[,] points, int[] result)
        {
            double firstSide, secindSide, corner;
            double thirdSide = Math.Sqrt(Math.Pow(pointsOfFigure[2, 0] - pointsOfFigure[0, 0], 2) + Math.Pow(pointsOfFigure[2, 1] - pointsOfFigure[0, 1], 2));
            for (int i = 0; i < points.GetLength(0); i++)
            {
                if (result[i] != 1 || result[i] != 2)
                {
                    firstSide = Math.Sqrt(Math.Pow(points[i, 0] - pointsOfFigure[0, 0], 2) + Math.Pow(points[i, 1] - pointsOfFigure[0, 1], 2));
                    secindSide = Math.Sqrt(Math.Pow(pointsOfFigure[2, 0] - points[i, 0], 2) + Math.Pow(pointsOfFigure[2, 1] - points[i, 1], 2));
                    corner = ((firstSide * firstSide) + (secindSide * secindSide) - (thirdSide * thirdSide)) / (2 * firstSide * secindSide);
                    if (corner < 0) result[i] = 3;
                    if (corner > 0) result[i] = 4;
                }
            }
        }



    }
}
