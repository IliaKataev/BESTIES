using System.Text.Json.Serialization;

namespace LibraryWeb.Models
{
    public class BookDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string? FirstPublishDate { get; set; }
        public string? Description { get; set; }

        public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
        public List<BookSubjectDto> Subjects { get; set; } = new List<BookSubjectDto>();
        public List<BookCoverDto> Covers { get; set; } = new List<BookCoverDto>();
    }

    public class AuthorDto
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? BirthDate { get; set; }
        public string? DeathDate { get; set; }
        public string? Wikipedia { get; set; }
    }

    public class BookSubjectDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
    }

    public class BookCoverDto
    {
        public long Id { get; set; }
        public int CoverFile { get; set; }

        public string Url => $"/images/covers/{CoverFile}.png";
    }

    public class CustomerDto
    {
        public long Customerid { get; set; } // numeric id, совпадает с БД
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }


    public class IssueDto
    {
        public long Issueid { get; set; }
        public long Customerid { get; set; }
        public string BookKey { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime DateOfIssue { get; set; }
        public DateTime ReturnUntil { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool Renewed { get; set; }
    }
}
