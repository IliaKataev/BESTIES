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
}
