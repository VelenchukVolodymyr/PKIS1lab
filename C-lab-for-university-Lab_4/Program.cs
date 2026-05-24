using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_1
{
    class Program
    {
        static void Main()
        {
            var collection = new MagazineCollection();
            collection.AddDefaults();
            collection.AddMagazines(
                new Magazine("Daily Tech", Frequency.Weekly, DateTime.Today.AddDays(-7), 30000,
                    new List<Person> { new Person("Ivan", "Ivanov", new DateTime(1978, 2, 2)) },
                    new List<Article> { new Article(new Person("Anna", "Petrova", new DateTime(1990, 3, 3)), "Cloud Computing", 4.2) }),
                new Magazine("Eco Monthly", Frequency.Monthly, DateTime.Today.AddMonths(-1), 20000,
                    new List<Person> { new Person("Oleg", "Smirnov", new DateTime(1982, 4, 4)) },
                    new List<Article> { new Article(new Person("Lena", "Kovalenko", new DateTime(1993, 5, 5)), "Sustainable Energy", 4.7) })
            );

            Console.WriteLine("=== MagazineCollection full info ===");
            Console.WriteLine(collection.ToString());

            Console.WriteLine("=== MagazineCollection short info ===");
            Console.WriteLine(collection.ToShortString());

            Console.WriteLine("=== Sort by name ===");
            collection.SortByName();
            Console.WriteLine(collection.ToShortString());

            Console.WriteLine("=== Sort by issue date ===");
            collection.SortByIssueDate();
            Console.WriteLine(collection.ToShortString());

            Console.WriteLine("=== Sort by circulation ===");
            collection.SortByCirculation();
            Console.WriteLine(collection.ToShortString());

            Console.WriteLine($"Max average article rating: {collection.MaxAverageRating:F2}");

            Console.WriteLine("Monthly magazines:");
            foreach (var m in collection.MonthlyMagazines)
                Console.WriteLine(m.ToShortString());

            Console.WriteLine("=== Rating >= 4.0 group ===");
            foreach (var m in collection.RatingGroup(4.0))
                Console.WriteLine(m.ToShortString());

            var testCollections = new TestCollections(200000);

            var firstEdition = testCollections.Editions.First();
            var middleEdition = testCollections.Editions.ElementAt(testCollections.Count / 2);
            var lastEdition = testCollections.Editions.Last();
            var missingEdition = new Edition("Missing", DateTime.MinValue, 0);

            Console.WriteLine("--- Search timings (Edition list) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindEditionInList(firstEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindEditionInList(middleEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindEditionInList(lastEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindEditionInList(missingEdition).TotalMilliseconds} ms");

            var firstKey = testCollections.Editions.First().Name + "_0";
            var middleKey = testCollections.Editions.ElementAt(testCollections.Count / 2).Name + "_" + (testCollections.Count / 2);
            var lastKey = testCollections.Editions.Last().Name + "_" + (testCollections.Count - 1);
            var missingKey = "NoKey";

            Console.WriteLine("--- Search timings (string list) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindStringInList(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindStringInList(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindStringInList(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindStringInList(missingKey).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (Dictionary Edition) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInEditionDictionary(firstEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInEditionDictionary(middleEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInEditionDictionary(lastEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInEditionDictionary(missingEdition).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (ImmutableList Edition) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindEditionInImmutableList(firstEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindEditionInImmutableList(middleEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindEditionInImmutableList(lastEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindEditionInImmutableList(missingEdition).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (SortedList Edition) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInEditionSortedList(firstEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInEditionSortedList(middleEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInEditionSortedList(lastEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInEditionSortedList(missingEdition).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (SortedDictionary Edition) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInEditionSortedDictionary(firstEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInEditionSortedDictionary(middleEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInEditionSortedDictionary(lastEdition).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInEditionSortedDictionary(missingEdition).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (string list) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindStringInList(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindStringInList(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindStringInList(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindStringInList(missingKey).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (ImmutableList string) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindStringInImmutableList(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindStringInImmutableList(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindStringInImmutableList(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindStringInImmutableList(missingKey).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (SortedList string) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInStringSortedList(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInStringSortedList(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInStringSortedList(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInStringSortedList(missingKey).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (SortedDictionary string) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInStringSortedDictionary(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInStringSortedDictionary(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInStringSortedDictionary(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInStringSortedDictionary(missingKey).TotalMilliseconds} ms");

            Console.WriteLine("--- Search timings (Dictionary string) ---");
            Console.WriteLine($"First: {testCollections.MeasureFindInStringDictionary(firstKey).TotalMilliseconds} ms");
            Console.WriteLine($"Middle: {testCollections.MeasureFindInStringDictionary(middleKey).TotalMilliseconds} ms");
            Console.WriteLine($"Last: {testCollections.MeasureFindInStringDictionary(lastKey).TotalMilliseconds} ms");
            Console.WriteLine($"Missing: {testCollections.MeasureFindInStringDictionary(missingKey).TotalMilliseconds} ms");
        }
    }
}
