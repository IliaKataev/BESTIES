namespace LibraryApi.DTOs
{
    public class BookDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string? FirstPublishDate { get; set; }
        public string? Description { get; set; }

        public List<BookCoverDto> Covers { get; set; }
        public List<BookSubjectDto> Subjects { get; set; }
        public List<AuthorDto> Authors { get; set; }
    }

    public class BookListItemDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
    }

    public class BookCoverDto
    {
        public long Id { get; set; }
        public int CoverFile { get; set; }
        public string Url => $"/images/covers/{CoverFile}.png";
    }

    public class BookSubjectDto
    {
        public long Id { get; set; }
        public string Subject { get; set; }
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

    public class ExhibitionDto
    {
        public int ExhibitionId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string? CoverBookKey { get; set; }

        public List<ExhibitionBookDto> Books { get; set; } = new();
    }

    public class ExhibitionBookDto
    {
        public string BookKey { get; set; } = null!;
        public int OrderNumber { get; set; }
    }
    public class CreateExhibitionDto
    {
        public string Name { get; set; } = null!;
        public string? CoverBookKey { get; set; }
        public List<string> BookKeys { get; set; } = new();
    }

    public class UpdateExhibitionDto
    {
        public string Name { get; set; } = null!;
        public string? CoverBookKey { get; set; }
        public List<string> BookKeys { get; set; } = new();
    }

}
