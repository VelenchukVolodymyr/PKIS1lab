using System;

namespace Lab_1
{
    public class Article
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

    }
}