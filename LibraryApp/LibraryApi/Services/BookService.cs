using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

public interface IBookService
{
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookDetailsAsync(string key);
    Task<List<BookDto>> SearchBooksAsync(string? title, string? author);
}

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepo;
    public BookService(IBookRepository bookRepo) => _bookRepo = bookRepo;

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepo.GetAllAsync();
        return books.Select(MapToDto).ToList();
    }

    public async Task<BookDto?> GetBookDetailsAsync(string key)
    {
        var book = await _bookRepo.GetByKeyWithRelationsAsync(key);
        return book == null ? null : MapToDto(book);
    }

    public async Task<List<BookDto>> SearchBooksAsync(string? title, string? author)
    {
        var books = await _bookRepo.GetAllAsync();
        if (!string.IsNullOrEmpty(title))
            books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!string.IsNullOrEmpty(author))
            books = books.Where(b => b.Authorkey.Any(a => a.Name.Contains(author, StringComparison.OrdinalIgnoreCase))).ToList();
        return books.Select(MapToDto).ToList();
    }

    private static BookDto MapToDto(Books b) => new BookDto
    {
        Key = b.Key,
        Title = b.Title,
        Subtitle = b.Subtitle,
        FirstPublishDate = b.Firstpublishdate,
        Description = b.Description,
        Covers = b.Bookcovers.Select(c => new BookCoverDto { Id = c.Id, CoverFile = c.Coverfile }).ToList(),
        Subjects = b.Booksubjects.Select(s => new BookSubjectDto { Id = s.Id, Subject = s.Subject }).ToList(),
        Authors = b.Authorkey.Select(a => new AuthorDto
        {
            Key = a.Key,
            Name = a.Name,
            Bio = a.Bio,
            BirthDate = a.Birthdate,
            DeathDate = a.Deathdate,
            Wikipedia = a.Wikipedia
        }).ToList()
    };
}
