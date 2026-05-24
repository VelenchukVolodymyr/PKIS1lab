using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab_1
{
    [JsonConverter(typeof(Magazine.MagazineJsonConverter))]
    public class Magazine : Edition, IRateAndCopy, IEnumerable<Article>
    {
        private Frequency _periodicity;
        private List<Person> _editors = new();
        private List<Article> _articles = new();

        public Magazine(string name, Frequency periodicity, DateTime issueDate, int circulation, List<Person>? editors, List<Article>? articles)
            : base(name, issueDate, circulation)
        {
            Periodicity = periodicity;
            Editors = editors ?? new List<Person>();
            Articles = articles ?? new List<Article>();
        }

        public Magazine(string name, Frequency periodicity, DateTime issueDate, int circulation)
            : this(name, periodicity, issueDate, circulation, new List<Person>(), new List<Article>())
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

        public List<Person> Editors
        {
            get => _editors ??= new List<Person>();
            init => _editors = value ?? new List<Person>();
        }

        public List<Article> Articles
        {
            get => _articles ??= new List<Article>();
            init => _articles = value ?? new List<Article>();
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
            var options = new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNameCaseInsensitive = true
            };

            using var memoryStream = new MemoryStream();
            JsonSerializer.Serialize(memoryStream, this, options);
            memoryStream.Position = 0;

            Magazine? copy = JsonSerializer.Deserialize<Magazine>(memoryStream, options);
            return copy ?? throw new InvalidOperationException("Deep copy failed.");
        }

        public bool Save(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return false;

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                using FileStream stream = new(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                JsonSerializer.Serialize(stream, this, options);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Load(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
                return false;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                using FileStream stream = new(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                Magazine? loaded = JsonSerializer.Deserialize<Magazine>(stream, options);
                if (loaded == null)
                    return false;

                CopyFrom(loaded);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Save(string filename, Magazine? magazine)
        {
            if (magazine == null)
                return false;

            return magazine.Save(filename);
        }

        public static bool Load(string filename, Magazine? magazine)
        {
            if (magazine == null)
                return false;

            return magazine.Load(filename);
        }

        public bool AddFromConsole()
        {
            Console.WriteLine("Введіть дані статті у форматі: Назва, Ім'я автора, Прізвище автора, Дата народження автора(yyyy-MM-dd), Рейтинг ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string[] parts = input.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5)
            {
                Console.WriteLine("Невірний формат. Очікується 5 значень, розділених комами.");
                return false;
            }

            try
            {
                string title = parts[0];
                string authorName = parts[1];
                string authorSurname = parts[2];
                if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDate))
                {
                    Console.WriteLine("Невірна дата народження автора. Використовуйте формат yyyy-MM-dd.");
                    return false;
                }

                string ratingText = parts[4].Replace(',', '.');
                if (!double.TryParse(ratingText, NumberStyles.Float, CultureInfo.InvariantCulture, out double rating))
                {
                    Console.WriteLine("Невірний рейтинг. Використовуйте десяткову крапку або кому.");
                    return false;
                }

                var article = new Article(new Person(authorName, authorSurname, birthDate), title, rating);
                Articles.Add(article);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка обробки даних: {ex.Message}");
                return false;
            }
        }

        private void CopyFrom(Magazine other)
        {
            _name = other.Name;
            _issueDate = other.IssueDate;
            _circulation = other.Circulation;
            _periodicity = other.Periodicity;
            _editors = new List<Person>();
            foreach (var editor in other.Editors)
                _editors.Add((Person)editor.DeepCopy());

            _articles = new List<Article>();
            foreach (var article in other.Articles)
                _articles.Add((Article)article.DeepCopy());
        }

        public IEnumerator<Article> GetEnumerator()
        {
            return new MagazineEnumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class MagazineEnumerator : IEnumerator<Article>
        {
            private readonly List<Article> _list;
            private int _position = -1;

            public MagazineEnumerator(Magazine magazine)
            {
                _list = new List<Article>();

                foreach (Article article in magazine.Articles)
                {
                    if (!magazine.Editors.Contains(article.Author))
                        _list.Add(article);
                }
            }

            public Article Current => _list[_position];

            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                _position++;
                return _position < _list.Count;
            }

            public void Reset()
            {
                _position = -1;
            }

            public void Dispose() { }
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

        public class MagazineJsonConverter : JsonConverter<Magazine>
        {
            public override Magazine? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                using JsonDocument document = JsonDocument.ParseValue(ref reader);
                JsonElement root = document.RootElement;

                string name = root.GetProperty("Name").GetString() ?? string.Empty;
                Frequency periodicity = Enum.TryParse(root.GetProperty("Periodicity").GetString(), out Frequency result)
                    ? result
                    : Frequency.Weekly;
                DateTime issueDate = root.GetProperty("IssueDate").GetDateTime();
                int circulation = root.GetProperty("Circulation").GetInt32();

                var editors = JsonSerializer.Deserialize<List<Person>>(root.GetProperty("Editors").GetRawText(), options) ?? new List<Person>();
                var articles = JsonSerializer.Deserialize<List<Article>>(root.GetProperty("Articles").GetRawText(), options) ?? new List<Article>();

                return new Magazine(name, periodicity, issueDate, circulation, editors, articles);
            }

            public override void Write(Utf8JsonWriter writer, Magazine value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("Name", value.Name);
                writer.WriteString("Periodicity", value.Periodicity.ToString());
                writer.WriteString("IssueDate", value.IssueDate);
                writer.WriteNumber("Circulation", value.Circulation);
                writer.WritePropertyName("Editors");
                JsonSerializer.Serialize(writer, value.Editors, options);
                writer.WritePropertyName("Articles");
                JsonSerializer.Serialize(writer, value.Articles, options);
                writer.WriteEndObject();
            }
        }
    }
}
