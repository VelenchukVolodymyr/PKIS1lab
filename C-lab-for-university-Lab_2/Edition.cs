using System;

namespace Lab_1
{
    public class Edition
    {
        protected string _name = null!;
        protected DateTime _issueDate;
        protected int _circulation;

        public Edition(string name, DateTime issueDate, int circulation)
        {
            Name = name;
            IssueDate = issueDate;
            Circulation = circulation;
        }

        public Edition() : this("", DateTime.MinValue, 0)
        {
        }

        public string Name
        {
            get => _name;
            init => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public DateTime IssueDate
        {
            get => _issueDate;
            init => _issueDate = value;
        }

        public int Circulation
        {
            get => _circulation;
            init
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value), "Circulation cannot be negative");
                _circulation = value;
            }
        }

        public virtual object DeepCopy()
        {
            return new Edition(Name, IssueDate, Circulation);
        }

        public override string ToString()
        {
            return $"Name: {Name}, IssueDate: {IssueDate:d}, Circulation: {Circulation}";
        }

        public virtual string ToShortString()
        {
            return ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Edition other)
                return false;

            return Name == other.Name &&
                   IssueDate == other.IssueDate &&
                   Circulation == other.Circulation;
        }

        public static bool operator ==(Edition? e1, Edition? e2)
        {
            if (ReferenceEquals(e1, e2))
                return true;

            if (e1 is null || e2 is null)
                return false;

            return e1.Equals(e2);
        }

        public static bool operator !=(Edition? e1, Edition? e2)
        {
            return !(e1 == e2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, IssueDate, Circulation);
        }
    }
}
