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

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return;

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
                Editors = new System.Collections.ArrayList(),
                Articles = new System.Collections.ArrayList()
            };
            Console.WriteLine(mag.ToString());

            var a1 = new Article(new Person("John","Doe", new DateTime(1990,1,1)), "AI Revolution", 4.5);
            var a2 = new Article(new Person("Jane","Smith", new DateTime(1985,5,5)), "Deep Learning", 3.8);
            mag.AddArticles(a1, a2);

            var edition1 = new Edition("Tech Today", DateTime.Today, 50000);
            var edition2 = new Edition("Tech Today", DateTime.Today, 50000);

            Console.WriteLine($"edition1 == edition2 (reference): {ReferenceEquals(edition1, edition2)}");
            Console.WriteLine($"edition1.Equals(edition2) (value): {edition1.Equals(edition2)}");
            
            Console.WriteLine($"hash1: {edition1.GetHashCode()}, hash2: {edition2.GetHashCode()}");

            try
            {
                var badEdition = new Edition("Bad", DateTime.Today, -1);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

       
            mag.AddEditors(new Person("Editor", "One", new DateTime(1970, 1, 1)),
                           new Person("Editor", "Two", new DateTime(1980, 1, 1)));

            Console.WriteLine("\nAfter adding articles and editors:");
            Console.WriteLine(mag.ToString());

           
            Console.WriteLine("Magazine.Edition: " + mag.Edition.ToString());


            var magCopy = (Magazine)mag.DeepCopy();

            magCopy.AddArticles(new Article(new Person("Copy", "Author", new DateTime(1995, 1, 1)), "Copy Article", 1.1));
            mag.AddArticles(new Article(new Person("Original", "Author", new DateTime(1999, 1, 1)), "Original Article", 9.9));

            Console.WriteLine("\nOriginal magazine after modification:");
            Console.WriteLine(mag.ToString());
            Console.WriteLine("Copy of magazine after modification:");
            Console.WriteLine(magCopy.ToString());

          
            Console.WriteLine("\nArticles with rating > 4.0:");
            foreach (var article in mag.GetArticlesWithRatingGreaterThan(4.0))
                Console.WriteLine(article);

          
            Console.WriteLine("\nArticles with title containing 'AI':");
            foreach (var article in mag.GetArticlesWithTitleContaining("AI"))
                Console.WriteLine(article);

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