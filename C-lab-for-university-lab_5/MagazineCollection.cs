using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_1
{
    public delegate void MagazineListHandler(object source, MagazineListHandlerEventArgs args);

    public class MagazineCollection
    {
        private readonly List<Magazine> _magazines = new();

        public string CollectionName { get; init; } = "MagazineCollection";

        public event MagazineListHandler? MagazineAdded;
        public event MagazineListHandler? MagazineReplaced;

        public MagazineCollection() { }

        public MagazineCollection(IEnumerable<Magazine> magazines)
        {
            if (magazines != null)
                _magazines.AddRange(magazines);
        }

        public void AddDefaults()
        {
            AddMagazine(new Magazine("Tech Today", Frequency.Monthly, new DateTime(2023, 1, 1), 50000,
                new List<Person> { new Person("John", "Doe", new DateTime(1980, 1, 1)) },
                new List<Article> { new Article(new Person("Alice", "Smith", new DateTime(1990, 1, 1)), "AI Trends", 4.5) }));

            AddMagazine(new Magazine("Science Weekly", Frequency.Weekly, new DateTime(2023, 3, 1), 25000,
                new List<Person> { new Person("Mary", "Johnson", new DateTime(1975, 3, 3)) },
                new List<Article> { new Article(new Person("Bob", "Brown", new DateTime(1988, 5, 5)), "Quantum Computing", 3.8) }));

            AddMagazine(new Magazine("Business Yearly", Frequency.Yearly, new DateTime(2022, 12, 31), 100000,
                new List<Person> { new Person("Anna", "Lee", new DateTime(1982, 4, 4)) },
                new List<Article> { new Article(new Person("Carol", "Davis", new DateTime(1991, 6, 6)), "Global Markets", 4.9) }));
        }

        public void AddMagazines(params Magazine[] magazines)
        {
            if (magazines == null)
                return;

            foreach (var magazine in magazines)
                AddMagazine(magazine);
        }

        public bool AddMagazine(Magazine magazine)
        {
            if (magazine == null)
                return false;

            _magazines.Add(magazine);
            OnMagazineAdded(new MagazineListHandlerEventArgs(CollectionName, $"Added magazine '{magazine.Name}'", _magazines.Count - 1));
            return true;
        }

        public bool Replace(int j, Magazine magazine)
        {
            if (magazine == null || j < 0 || j >= _magazines.Count)
                return false;

            _magazines[j] = magazine;
            OnMagazineReplaced(new MagazineListHandlerEventArgs(CollectionName, $"Replaced magazine at index {j} with '{magazine.Name}'", j));
            return true;
        }

        public Magazine this[int index]
        {
            get => _magazines[index];
            set => Replace(index, value);
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= _magazines.Count)
                return false;

            _magazines.RemoveAt(index);
            return true;
        }

        protected virtual void OnMagazineAdded(MagazineListHandlerEventArgs args)
        {
            MagazineAdded?.Invoke(this, args);
        }

        protected virtual void OnMagazineReplaced(MagazineListHandlerEventArgs args)
        {
            MagazineReplaced?.Invoke(this, args);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var mag in _magazines)
            {
                sb.AppendLine(mag.ToString());
                sb.AppendLine(new string('-', 40));
            }

            return sb.ToString();
        }

        public virtual string ToShortString()
        {
            var sb = new StringBuilder();
            foreach (var mag in _magazines)
            {
                sb.AppendLine($"Name: {mag.Name}, Periodicity: {mag.Periodicity}, IssueDate: {mag.IssueDate:d}, Circulation: {mag.Circulation}, AvgRating: {mag.Rating:F2}, Editors: {mag.Editors.Count}, Articles: {mag.Articles.Count}");
            }
            return sb.ToString();
        }

        public void SortByName()
        {
            _magazines.Sort();
        }

        public void SortByIssueDate()
        {
            _magazines.Sort(new EditionIssueDateComparer());
        }

        public void SortByCirculation()
        {
            _magazines.Sort(new EditionCirculationComparer());
        }

        public double MaxAverageRating
        {
            get
            {
                if (_magazines.Count == 0)
                    return 0.0;

                return _magazines.Max(m => m.Rating);
            }
        }

        public IEnumerable<Magazine> MonthlyMagazines
        {
            get
            {
                return _magazines.Where(m => m.Periodicity == Frequency.Monthly);
            }
        }

        public List<Magazine> RatingGroup(double value)
        {
            return _magazines.Where(m => m.Rating >= value).ToList();
        }

        public List<Magazine> Magazines => _magazines;

        public static Magazine GenerateMagazine(int index)
        {
            string name = $"AutoMagazine_{index}";
            var period = index % 3 == 0 ? Frequency.Monthly : (index % 3 == 1 ? Frequency.Weekly : Frequency.Yearly);
            int circulation = 10000 + index * 5000;
            var issueDate = DateTime.Today;
            var editor = new Person($"Editor{index}", "Auto", new DateTime(1980, 1, 1).AddYears(index % 20));
            var article = new Article(new Person($"Author{index}", "Auto", new DateTime(1990, 1, 1).AddYears(index % 30)), $"Article#{index}", 2.5 + (index % 5));

            return new Magazine(name, period, issueDate, circulation, new List<Person> { editor }, new List<Article> { article });
        }
    }

    public class MagazineListHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; }
        public string ChangeInfo { get; }
        public int ElementIndex { get; }

        public MagazineListHandlerEventArgs(string collectionName, string changeInfo, int elementIndex)
        {
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            ChangeInfo = changeInfo ?? throw new ArgumentNullException(nameof(changeInfo));
            ElementIndex = elementIndex;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, Change: {ChangeInfo}, Index: {ElementIndex}";
        }
    }

    public class ListEntry
    {
        public string CollectionName { get; }
        public string Info { get; }
        public int ElementIndex { get; }

        public ListEntry(string collectionName, string info, int elementIndex)
        {
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            ElementIndex = elementIndex;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, Info: {Info}, Index: {ElementIndex}";
        }
    }

    public class Listener
    {
        public List<ListEntry> Changes { get; } = new();

        public void HandleChange(object? source, MagazineListHandlerEventArgs args)
        {
            if (args == null)
                return;

            Changes.Add(new ListEntry(args.CollectionName, args.ChangeInfo, args.ElementIndex));
        }

        public override string ToString()
        {
            if (Changes.Count == 0)
                return "No changes recorded.";

            var sb = new StringBuilder();
            foreach (var entry in Changes)
            {
                sb.AppendLine(entry.ToString());
            }
            return sb.ToString();
        }
    }
}