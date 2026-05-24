using System;

namespace Lab_1
{
    class Program
    {
        static void Main()
        {
                    Console.WriteLine(
            "Enter number of rows and columns in one line.\n" +
            "Use any of these separators: space, comma or semicolon.");

        string input = Console.ReadLine();

        
        string[] parts = input.Split(new char[] { ' ', ',', ';', '\t' },
                                    StringSplitOptions.RemoveEmptyEntries);

            int nRows = Convert.ToInt32(parts[0]);
            int nCols = Convert.ToInt32(parts[1]);

            int total = nRows * nCols;

            var mag= new Magazine();
            Console.WriteLine(mag.ToShortString());

            Console.WriteLine($"Weekly index: {mag[Frequency.Weekly]}");
            Console.WriteLine($"Monthly index: {mag[Frequency.Monthly]}");
            Console.WriteLine($"Yearly index: {mag[Frequency.Yearly]}");

             mag = new Magazine
            {
                Name = "Tech Today",
                Periodicity = Frequency.Monthly,
                IssueDate = DateTime.Now,
                Circulation = 50000,
                Articles = Array.Empty<Article>()
            };
            Console.WriteLine(mag.ToString());

            var a1 = new Article(new Person("John","Doe", new DateTime(1990,1,1)), "AI Revolution", 4.5);
            var a2 = new Article(new Person("Jane","Smith", new DateTime(1985,5,5)), "Deep Learning", 3.8);
            mag.AddArticles(a1, a2);
            Console.WriteLine("\nAfter adding articles:");
            Console.WriteLine(mag.ToString());


            Article[] arr1 = new Article[total];
            Article[,] arr2 = new Article[nRows, nCols];

            int sum = 0;
            int size = 1;
            int rows = 0;

            while (sum < total)
            {
                sum += Math.Min(size, total - sum);
                size++;
                rows++;
            }

            Article[][] arr3 = new Article[rows][];

            sum = 0;
            size = 1;

            for (int i = 0; i < rows; i++)
            {
                int currentSize = Math.Min(size, total - sum);
                arr3[i] = new Article[currentSize];

                sum += currentSize;
                size++;
            }

            int start, end;

            start = Environment.TickCount;
            for (int i = 0; i < total; i++)
                arr1[i] = new Article();
            end = Environment.TickCount;
            Console.WriteLine($"1D array initialization time: {end - start}");

            start = Environment.TickCount;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    arr2[i, j] = new Article();
            end = Environment.TickCount;
            Console.WriteLine($"2D rectangular array initialization time: {end - start}");

            start = Environment.TickCount;
            for (int i = 0; i < arr3.Length; i++)
                {
                    for (int j = 0; j < arr3[i].Length; j++)
                    {
                        arr3[i][j] = new Article();
                    }
                }
            end = Environment.TickCount;
            Console.WriteLine($"2D jagged array initialization time: {end - start}");
           
        }
    }
}