using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services
{
    public interface IExhibitionService
    {
        Task<List<ExhibitionDto>> GetAllAsync();
        Task<ExhibitionDto?> GetByIdAsync(int id);
        Task<ExhibitionDto> CreateAsync(CreateExhibitionDto dto);
        Task<ExhibitionDto?> UpdateAsync(int id, UpdateExhibitionDto dto);
        Task<bool> DeleteAsync(int id);
    }
    public class ExhibitionService : IExhibitionService
    {
        private readonly IExhibitionRepository _repo;
        private readonly LibraryContext _context;

        public ExhibitionService(IExhibitionRepository repo, LibraryContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<ExhibitionDto>> GetAllAsync()
        {
            var exhibitions = await _repo.GetAllAsync();
            return exhibitions.Select(MapToDto).ToList();
        }

        public async Task<ExhibitionDto?> GetByIdAsync(int id)
        {
            var exhibition = await _repo.GetByIdAsync(id);
            return exhibition == null ? null : MapToDto(exhibition);
        }

        public async Task<ExhibitionDto> CreateAsync(CreateExhibitionDto dto)
        {
            var exhibition = new Exhibitions
            {
                Name = dto.Name,
                CoverBookKey = dto.CoverBookKey ?? dto.BookKeys.FirstOrDefault(),
                ExhibitionBooks = dto.BookKeys.Select((k, idx) => new ExhibitionBooks
                {
                    BookKey = k,
                    OrderNumber = idx + 1
                }).ToList(),
                CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
            };

            await _repo.AddAsync(exhibition);
            return MapToDto(exhibition);
        }

        public async Task<ExhibitionDto?> UpdateAsync(int id, UpdateExhibitionDto dto)
        {
            var exhibition = await _repo.GetByIdAsync(id);
            if (exhibition == null) return null;

            exhibition.Name = dto.Name;
            exhibition.CoverBookKey = dto.CoverBookKey ?? dto.BookKeys.FirstOrDefault();
            exhibition.CreatedAt = DateTime.SpecifyKind(exhibition.CreatedAt, DateTimeKind.Utc);

            // очищаем старые книги и добавляем новые
            exhibition.ExhibitionBooks.Clear();
            exhibition.ExhibitionBooks = dto.BookKeys.Select((k, idx) => new ExhibitionBooks
            {
                BookKey = k,
                OrderNumber = idx + 1
            }).ToList();

            await _repo.UpdateAsync(exhibition);
            return MapToDto(exhibition);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exhibition = await _repo.GetByIdAsync(id);
            if (exhibition == null) return false;
            await _repo.DeleteAsync(exhibition);
            return true;
        }

        private static ExhibitionDto MapToDto(Exhibitions e)
        {
            return new ExhibitionDto
            {
                ExhibitionId = e.ExhibitionId,
                Name = e.Name,
                CreatedAt = e.CreatedAt,
                // Если есть CoverBook, берем CoverFile, иначе null
                CoverBookKey = e.CoverBook != null ? e.CoverBook.Bookcovers.FirstOrDefault()?.Coverfile.ToString() : null,
                Books = e.ExhibitionBooks
                         .OrderBy(eb => eb.OrderNumber)
                         .Select(eb => new ExhibitionBookDto
                         {
                             BookKey = eb.BookKey,
                             OrderNumber = eb.OrderNumber
                         }).ToList()
            };
        }

    }
}
