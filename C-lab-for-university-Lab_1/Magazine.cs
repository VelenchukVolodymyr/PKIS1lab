using System;
using System.Text;

namespace Lab_1
{
    public class Magazine
    {
        private string _name = null!;
        private Frequency _periodicity;
        private DateTime _issueDate;
        private int _circulation;
        private Article[] _articles = null!;

        public Magazine(string name, Frequency periodicity, DateTime issueDate, int circulation, Article[] articles)
        {
            Name = name;
            Periodicity = periodicity;
            IssueDate = issueDate;
            Circulation = circulation;
            Articles = Array.Empty<Article>();
        }

        public Magazine():this("", Frequency.Weekly, DateTime.MinValue, 0, Array.Empty<Article>())
        {

        }

        
        public string Name { get => _name; init => _name = value; }
        public Frequency Periodicity { get => _periodicity; init => _periodicity = value; }
        public DateTime IssueDate { get => _issueDate; init => _issueDate = value; }
        public int Circulation { get => _circulation; init => _circulation = value; }
        public Article[] Articles { get => _articles; init => _articles = value; }

        
        public double AverageRating
        {
            get
            {
                if (_articles == null || _articles.Length == 0) return 0.0;
                double sum = 0;
                foreach (var a in _articles) sum += a.Rating;
                return sum / _articles.Length;
            }
        }

       
        public bool this[Frequency freq]
        {
            get => Periodicity == freq;
        }

        public void AddArticles(params Article[] newArticles)
        {
            if (newArticles == null || newArticles.Length == 0) return;
            if (Articles == null || Articles.Length == 0) 
            { 
                _articles = newArticles;
                return; 
            }
                
            int oldLen = Articles.Length;
            Array.Resize(ref _articles, oldLen + newArticles.Length);
            for (int i = 0; i < newArticles.Length; i++)
                Articles[oldLen + i] = newArticles[i];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Periodicity: {Periodicity}");
            sb.AppendLine($"IssueDate: {IssueDate:d}");
            sb.AppendLine($"Circulation: {Circulation}");
            sb.AppendLine("Articles:");
            if (Articles != null && Articles.Length > 0)
            {
                foreach (var a in Articles)
                    sb.AppendLine(a.ToString());
            }
            else sb.AppendLine("(none)");
            return sb.ToString();
        }

        public virtual string ToShortString()
        {
            return $"Name: {Name}, Periodicity: {Periodicity}, IssueDate: {IssueDate:d}, Circulation: {Circulation}, AvgRating: {AverageRating}";
        }
    }
}