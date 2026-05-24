using System;
using System.Collections.Generic;
using System.IO;

namespace Lab_1
{
    class Program
    {
        static void Main()
        {
            RunMagazineDemo();
        }

        private static void RunMagazineDemo()
        {
            Console.WriteLine("=== Magazine console demo ===");

            var magazine = new Magazine("Platform Review", Frequency.Monthly, DateTime.Today, 42000,
                new List<Person> { new Person("Ivan", "Petrov", new DateTime(1985, 7, 12)) },
                new List<Article> { new Article(new Person("Olena", "Shevchenko", new DateTime(1990, 2, 18)), "Platform Architecture", 4.7) });

            Console.WriteLine("--- Original magazine ---");
            Console.WriteLine(magazine);

            var magazineCopy = (Magazine)magazine.DeepCopy();
            Console.WriteLine("--- Deep copy of magazine ---");
            Console.WriteLine(magazineCopy);

            Console.Write("Введіть ім'я файлу (або натисніть Enter для використання файлу за замовчуванням): ");
            string filename = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = "data.json";
            }
            else
            {
                filename = Path.ChangeExtension(filename, ".json");
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine($"Файл не існує. Створюю новий файл {filename}...");
                magazine.Save(filename);
            }
            else
            {
                Console.WriteLine($"Файл існує. Завантажую дані з файлу {filename}...");
                magazine.Load(filename);
            }

            Console.WriteLine("--- Magazine after file operation ---");
            Console.WriteLine(magazine);

            Console.WriteLine("--- AddFromConsole() then Save(filename) ---");
            magazine.AddFromConsole();
            magazine.Save(filename);
            Console.WriteLine(magazine);

            Console.WriteLine("--- Static Load(filename, magazine), AddFromConsole(), Save(filename, magazine) ---");
            Magazine.Load(filename, magazine);
            magazine.AddFromConsole();
            Magazine.Save(filename, magazine);
            Console.WriteLine(magazine);
        }
    }
}
