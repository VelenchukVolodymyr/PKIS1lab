using System;
using System.Collections;
using System.Text;

namespace Lab_1
{
    public class Magazine : Edition, IRateAndCopy, IEnumerable
    {
        private Frequency _periodicity;
        private ArrayList _editors = new();
        private ArrayList _articles = new();

        public Magazine(string name, Frequency periodicity, DateTime issueDate, int circulation, ArrayList? editors, ArrayList? articles)
            : base(name, issueDate, circulation)
        {
            Periodicity = periodicity;
            Editors = editors ?? new ArrayList();
            Articles = articles ?? new ArrayList();
        }

        public Magazine(string name, Frequency periodicity, DateTime issueDate, int circulation)
            : this(name, periodicity, issueDate, circulation, new ArrayList(), new ArrayList())
        {
        }

        public Magazine() : this("", Frequency.Weekly, DateTime.MinValue, 0)
        {
        }

        public Frequency Periodicity
        {
            get => _periodicity;
            init => _periodicity = value;
        }

        public ArrayList Editors
        {
            get => _editors ??= new ArrayList();
            init => _editors = value ?? new ArrayList();
        }

        public ArrayList Articles
        {
            get => _articles ??= new ArrayList();
            init => _articles = value ?? new ArrayList();
        }

        public double AverageRating
        {
            get
            {
                if (Articles == null || Articles.Count == 0)
                    return 0.0;

                double sum = 0;
                foreach (Article a in Articles)
                    sum += a.Rating;

                return sum / Articles.Count;
            }
        }

        public double Rating => AverageRating;

        public Edition Edition
        {
            get => this;
            init
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                Name = value.Name;
                IssueDate = value.IssueDate;
                Circulation = value.Circulation;
            }
        }

        public bool this[Frequency freq] => Periodicity == freq;

        public void AddArticles(params Article[] newArticles)
        {
            if (newArticles == null || newArticles.Length == 0)
                return;

            foreach (var article in newArticles)
            {
                if (article != null)
                    Articles.Add(article);
            }
        }

        public void AddEditors(params Person[] newEditors)
        {
            if (newEditors == null || newEditors.Length == 0)
                return;

            foreach (var editor in newEditors)
            {
                if (editor != null)
                    Editors.Add(editor);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Periodicity: {Periodicity}");
            sb.AppendLine($"IssueDate: {IssueDate:d}");
            sb.AppendLine($"Circulation: {Circulation}");

            sb.AppendLine("Editors:");
            if (Editors != null && Editors.Count > 0)
            {
                foreach (Person p in Editors)
                    sb.AppendLine(p.ToString());
            }
            else
            {
                sb.AppendLine("(none)");
            }

            sb.AppendLine("Articles:");
            if (Articles != null && Articles.Count > 0)
            {
                foreach (Article a in Articles)
                    sb.AppendLine(a.ToString());
            }
            else
            {
                sb.AppendLine("(none)");
            }

            return sb.ToString();
        }

        public override string ToShortString()
        {
            return $"Name: {Name}, Periodicity: {Periodicity}, IssueDate: {IssueDate:d}, Circulation: {Circulation}, AvgRating: {AverageRating}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Magazine magazine)
                return false;

            if (!base.Equals(obj) || Periodicity != magazine.Periodicity)
                return false;

            var editors = Editors;
            var otherEditors = magazine.Editors;
            var articles = Articles;
            var otherArticles = magazine.Articles;

            if (editors.Count != otherEditors.Count || articles.Count != otherArticles.Count)
                return false;

            for (int i = 0; i < editors.Count; i++)
            {
                if (!editors[i]!.Equals(otherEditors[i]))
                    return false;
            }

            for (int i = 0; i < articles.Count; i++)
            {
                if (!articles[i]!.Equals(otherArticles[i]))
                    return false;
            }

            return true;
        }

        public static bool operator ==(Magazine? m1, Magazine? m2)
        {
            if (ReferenceEquals(m1, m2))
                return true;

            if (m1 is null || m2 is null)
                return false;

            return m1.Equals(m2);
        }

        public static bool operator !=(Magazine? m1, Magazine? m2)
        {
            return !(m1 == m2);
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(base.GetHashCode(), Periodicity);
            foreach (Person editor in Editors)
            {
                hash = HashCode.Combine(hash, editor);
            }

            foreach (Article article in Articles)
            {
                hash = HashCode.Combine(hash, article);
            }

            return hash;
        }

        public override object DeepCopy()
        {
            var editorsCopy = new ArrayList();
            foreach (Person editor in Editors)
            {
                editorsCopy.Add(editor.DeepCopy());
            }

            var articlesCopy = new ArrayList();
            foreach (Article article in Articles)
            {
                articlesCopy.Add(article.DeepCopy());
            }

            return new Magazine(Name, Periodicity, IssueDate, Circulation, editorsCopy, articlesCopy);
        }

        public IEnumerator GetEnumerator()
        {
            return new MagazineEnumerator(this);
        }

        public class MagazineEnumerator : IEnumerator
        {
            private readonly ArrayList _list;
            private int _position = -1;

            public MagazineEnumerator(Magazine magazine)
            {
                _list = new ArrayList();

                foreach (Article article in magazine.Articles)
                {
                    if (!magazine.Editors.Contains(article.Author))
                        _list.Add(article);
                }
            }

            public object Current => _list[_position]!;

            public bool MoveNext()
            {
                _position++;
                return _position < _list.Count;
            }

            public void Reset()
            {
                _position = -1;
            }
        }

        public IEnumerable<Article> GetArticlesWithRatingGreaterThan(double rating)
        {
            foreach (Article article in Articles)
            {
                if (article.Rating > rating)
                    yield return article;
            }
        }

        public IEnumerable<Article> GetArticlesWithTitleContaining(string substring)
        {
            if (string.IsNullOrEmpty(substring))
                yield break;

            foreach (Article article in Articles)
            {
                if (!string.IsNullOrEmpty(article.Title) &&
                    article.Title.Contains(substring, StringComparison.CurrentCultureIgnoreCase))
                {
                    yield return article;
                }
            }
        }

        public IEnumerable<Article> GetArticlesByEditors()
        {
            foreach (Article article in Articles)
            {
                if (Editors.Contains(article.Author))
                    yield return article;
            }
        }

        public IEnumerable<Person> GetEditorsWithoutArticles()
        {
            foreach (Person editor in Editors)
            {
                bool hasArticle = false;
                foreach (Article article in Articles)
                {
                    if (article.Author.Equals(editor))
                    {
                        hasArticle = true;
                        break;
                    }
                }

                if (!hasArticle)
                    yield return editor;
            }
        }
    }
}
