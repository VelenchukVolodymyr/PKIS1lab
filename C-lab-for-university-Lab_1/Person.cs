using System;


namespace Lab_1
{
    public class Person
    {
        private string _name = null!;
        private string _surname = null!;
        
        private System.DateTime _dateOfBirth;

        public Person(string name, string surname, System.DateTime dateOfBirth)
        {
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }

        public Person():this("", "", new System.DateTime(1, 1, 1))
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
            set
            {
                _dateOfBirth = new System.DateTime(value, _dateOfBirth.Month, _dateOfBirth.Day);
            }
        }

        
        public override string ToString()
        {
            return $"Name: {Name}, Surname: { Surname}, DateOfBirth: {DateOfBirth:d}";
        }

        
        public virtual string ToShortString()
        {
            return $"{Surname} {Name}";
        }
    }
}