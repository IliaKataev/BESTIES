using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services
{
    public class BookService
    {
        private readonly BookRepository _repo;

        public BookService(BookRepository repo)
        {
            _repo = repo;
        }

        // возвращаем все книги как DTO
        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var books = await _repo.GetAllAsync();
            return books.Select(MapToDto).ToList();
        }

        // детальная информация по одной книге
        public async Task<BookDto?> GetBookDetailsAsync(string key)
        {
            var book = await _repo.GetByKeyAsync(key);
            if (book == null) return null;
            return MapToDto(book);
        }

        // поиск книг
        public async Task<List<BookDto>> SearchBooksAsync(string? title, string? author)
        {
            var books = await _repo.SearchAsync(title, author);
            return books.Select(MapToDto).ToList();
        }

        // маппинг в дто
        private static BookDto MapToDto(Books b)
        {
            return new BookDto
            {
                Key = b.Key,
                Title = b.Title,
                Subtitle = b.Subtitle,
                FirstPublishDate = b.Firstpublishdate,
                Description = b.Description,
                Covers = b.Bookcovers.Select(c => new BookCoverDto
                {
                    Id = c.Id,
                    CoverFile = c.Coverfile
                }).ToList(),
                Subjects = b.Booksubjects.Select(s => new BookSubjectDto
                {
                    Id = s.Id,
                    Subject = s.Subject
                }).ToList(),
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
    }
}
