using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAuthorsAsync();
        Task<AuthorDto?> GetAuthorByKeyAsync(string key);
        Task<List<AuthorDto>> SearchAuthorsAsync(string name);
    }

    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Authors> _authorRepo;

        public AuthorService(IGenericRepository<Authors> authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<List<AuthorDto>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepo.GetAllAsync();
            return authors.Select(MapToDto).ToList();
        }

        public async Task<AuthorDto?> GetAuthorByKeyAsync(string key)
        {
            var author = (await _authorRepo.FindAsync(a => a.Key == key)).FirstOrDefault();
            return author == null ? null : MapToDto(author);
        }

        public async Task<List<AuthorDto>> SearchAuthorsAsync(string name)
        {
            var authors = await _authorRepo.FindAsync(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return authors.Select(MapToDto).ToList();
        }

        private static AuthorDto MapToDto(Authors a) => new AuthorDto
        {
            Key = a.Key,
            Name = a.Name,
            Bio = a.Bio,
            BirthDate = a.Birthdate,
            DeathDate = a.Deathdate,
            Wikipedia = a.Wikipedia
        };
    }
}

