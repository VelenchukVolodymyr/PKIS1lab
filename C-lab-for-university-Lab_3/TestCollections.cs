using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Lab_1
{
    public class TestCollections
    {
        private readonly List<Edition> _editions = new();
        private readonly List<string> _stringKeys = new();
        private readonly Dictionary<Edition, Magazine> _editionDictionary = new();
        private readonly Dictionary<string, Magazine> _stringDictionary = new();

        public TestCollections(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Magazine magazine = MagazineCollection.GenerateMagazine(i + 1);
                Edition edition = new Edition(magazine.Name, magazine.IssueDate, magazine.Circulation);
                string key = $"{magazine.Name}_{i}";

                _editions.Add(edition);
                _stringKeys.Add(key);
                _editionDictionary.TryAdd(edition, magazine);
                _stringDictionary.TryAdd(key, magazine);
            }
        }

        public static Magazine Generate(int i) => MagazineCollection.GenerateMagazine(i);

        public TimeSpan MeasureFindEditionInList(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editions.Contains(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindStringInList(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringKeys.Contains(key);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInEditionDictionary(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionDictionary.ContainsKey(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInStringDictionary(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringDictionary.ContainsKey(key);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindValueInDictionary(Magazine value)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionDictionary.ContainsValue(value) || _stringDictionary.ContainsValue(value);
            sw.Stop();
            return sw.Elapsed;
        }

        public int Count => _editions.Count;
        public IEnumerable<Edition> Editions => _editions;
        public IEnumerable<Magazine> Magazines => _editionDictionary.Values;
    }
}
