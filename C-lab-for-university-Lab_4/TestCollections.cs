using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        private readonly ImmutableList<Edition> _editionImmutableList;
        private readonly ImmutableList<string> _stringKeysImmutableList;
        private readonly ImmutableDictionary<Edition, Magazine> _editionImmutableDictionary;
        private readonly ImmutableDictionary<string, Magazine> _stringImmutableDictionary;
        private readonly SortedList<Edition, Magazine> _editionSortedList;
        private readonly SortedDictionary<Edition, Magazine> _editionSortedDictionary;
        private readonly SortedList<string, Magazine> _stringSortedList;
        private readonly SortedDictionary<string, Magazine> _stringSortedDictionary;

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

            _editionImmutableList = _editions.ToImmutableList();
            _stringKeysImmutableList = _stringKeys.ToImmutableList();
            _editionImmutableDictionary = _editionDictionary.ToImmutableDictionary();
            _stringImmutableDictionary = _stringDictionary.ToImmutableDictionary();
            _editionSortedList = new SortedList<Edition, Magazine>(_editionDictionary);
            _editionSortedDictionary = new SortedDictionary<Edition, Magazine>(_editionDictionary);
            _stringSortedList = new SortedList<string, Magazine>(_stringDictionary);
            _stringSortedDictionary = new SortedDictionary<string, Magazine>(_stringDictionary);
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

        public TimeSpan MeasureFindEditionInImmutableList(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionImmutableList.Contains(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindStringInImmutableList(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringKeysImmutableList.Contains(key);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInEditionImmutableDictionary(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionImmutableDictionary.ContainsKey(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInStringImmutableDictionary(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringImmutableDictionary.ContainsKey(key);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInEditionSortedList(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionSortedList.ContainsKey(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInEditionSortedDictionary(Edition edition)
        {
            var sw = Stopwatch.StartNew();
            bool found = _editionSortedDictionary.ContainsKey(edition);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInStringSortedList(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringSortedList.ContainsKey(key);
            sw.Stop();
            return sw.Elapsed;
        }

        public TimeSpan MeasureFindInStringSortedDictionary(string key)
        {
            var sw = Stopwatch.StartNew();
            bool found = _stringSortedDictionary.ContainsKey(key);
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
