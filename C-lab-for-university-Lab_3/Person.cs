using System;

namespace Lab_1
{
    public class Person
    {
        private string _name = null!;
        private string _surname = null!;
        private DateTime _dateOfBirth;

        public Person(string name, string surname, DateTime dateOfBirth)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }

        public Person() : this("", "", new DateTime(1, 1, 1))
        {
        }

        public string Name
        {
            get => _name;
            init => _name = value;
        }

        public string Surname
        {
            get => _surname;
            init => _surname = value;
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }

        public int BirthYear
        {
            get => _dateOfBirth.Year;
            set => _dateOfBirth = new DateTime(value, _dateOfBirth.Month, _dateOfBirth.Day);
        }

        public override string ToString()
        {
            return $"Name: {Name}, Surname: {Surname}, DateOfBirth: {DateOfBirth:d}";
        }

        public virtual string ToShortString()
        {
            return $"{Surname} {Name}";
        }

        
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Person person)
                return false;

            return Name == person.Name &&
                   Surname == person.Surname &&
                   DateOfBirth == person.DateOfBirth;
        }

        public static bool operator ==(Person? p1, Person? p2)
        {
            if (ReferenceEquals(p1, p2))
                return true;

            if (p1 is null || p2 is null)
                return false;

            return p1.Equals(p2);
        }

        public static bool operator !=(Person? p1, Person? p2)
        {
            return !(p1 == p2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Surname, DateOfBirth);
        }

        public virtual object DeepCopy()
        {
            return new Person(Name, Surname, DateOfBirth);
        }
    }
}