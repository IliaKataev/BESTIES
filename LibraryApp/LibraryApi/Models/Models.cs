namespace LibraryApi.Models
{
    public class Book
    {
        public string Key { get; set; }   // PK
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string? FirstPublishDate { get; set; }
        public string? Description { get; set; }

        public ICollection<BookCover> Covers { get; set; }
        public ICollection<BookSubject> Subjects { get; set; }
        public ICollection<BookAuthor> BookAuthors { get; set; }
        public ICollection<Issue> Issues { get; set; }
    }

    public class BookCover
    {
        public long Id { get; set; }      // BIGSERIAL
        public int CoverFile { get; set; }

        public string BookKey { get; set; }
        public Book Book { get; set; }
    }

    public class BookSubject
    {
        public long Id { get; set; }
        public string Subject { get; set; }

        public string BookKey { get; set; }
        public Book Book { get; set; }
    }

    public class Author
    {
        public string Key { get; set; }   // PK
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? BirthDate { get; set; }
        public string? DeathDate { get; set; }
        public string? Wikipedia { get; set; }

        public ICollection<BookAuthor> BookAuthors { get; set; }
    }

    // many-to-many
    public class BookAuthor
    {
        public string BookKey { get; set; }
        public Book Book { get; set; }

        public string AuthorKey { get; set; }
        public Author Author { get; set; }
    }

    public class Customer
    {
        public long CustomerID { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Zip { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public ICollection<Issue> Issues { get; set; }
    }

    public class Issue
    {
        public long IssueID { get; set; }
        public long CustomerID { get; set; }
        public Customer Customer { get; set; }

        public string BookKey { get; set; }
        public Book Book { get; set; }

        public DateTime DateOfIssue { get; set; }
        public DateTime ReturnUntil { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
