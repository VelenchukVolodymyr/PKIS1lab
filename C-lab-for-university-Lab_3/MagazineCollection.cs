using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab_1
{
    public class MagazineCollection
    {
        private readonly List<Magazine> _magazines = new();

        public MagazineCollection() { }

        public MagazineCollection(IEnumerable<Magazine> magazines)
        {
            if (magazines != null)
                _magazines.AddRange(magazines);
        }

        public void AddDefaults()
        {
            _magazines.AddRange(new[]
            {
                new Magazine("Tech Today", Frequency.Monthly, new DateTime(2023, 1, 1), 50000,
                    new List<Person> { new Person("John", "Doe", new DateTime(1980, 1, 1)) },
                    new List<Article> { new Article(new Person("Alice", "Smith", new DateTime(1990, 1, 1)), "AI Trends", 4.5) }),

                new Magazine("Science Weekly", Frequency.Weekly, new DateTime(2023, 3, 1), 25000,
                    new List<Person> { new Person("Mary", "Johnson", new DateTime(1975, 3, 3)) },
                    new List<Article> { new Article(new Person("Bob", "Brown", new DateTime(1988, 5, 5)), "Quantum Computing", 3.8) }),

                new Magazine("Business Yearly", Frequency.Yearly, new DateTime(2022, 12, 31), 100000,
                    new List<Person> { new Person("Anna", "Lee", new DateTime(1982, 4, 4)) },
                    new List<Article> { new Article(new Person("Carol", "Davis", new DateTime(1991, 6, 6)), "Global Markets", 4.9) }),
            });
        }

        public void AddMagazines(params Magazine[] magazines)
        {
            if (magazines == null)
                return;

            _magazines.AddRange(magazines);
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
}