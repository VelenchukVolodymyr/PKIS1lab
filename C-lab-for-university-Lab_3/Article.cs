using System;

namespace Lab_1
{
    public class Article : IRateAndCopy
    {
        public Person Author { get; init; }
        public string Title { get; init; }
        public double Rating { get; init; }

        public Article(Person author, string title, double rating)
        {
            Author = author;
            Title = title;
            Rating = rating;
        }

        public Article():this(new Person(), "", 0.0)
        {
        }

        public override string ToString()
        {
            return $"Author: {Author}, Title: {Title}, Rating: {Rating}";
        }

        
        
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Article article)
                return false;

            return Author.Equals(article.Author) &&
                   Title == article.Title &&
                   Rating == article.Rating;
        }

        public static bool operator ==(Article? a1, Article? a2)
        {
            if (ReferenceEquals(a1, a2))
                return true;

            if (a1 is null || a2 is null)
                return false;

            return a1.Equals(a2);
        }
        

        public static bool operator !=(Article? a1, Article? a2)
        {
            return !(a1 == a2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Author, Title, Rating);
        }

        public virtual object DeepCopy()
        {
            Person authorCopy = (Person)Author.DeepCopy();

            return new Article(
                authorCopy,
                Title,
                Rating
            );
        }

    }
}