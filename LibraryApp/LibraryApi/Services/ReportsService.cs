using LibraryApi.DTOs;
using LibraryApi.Repositories;

namespace LibraryApi.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IIssueRepository _issueRepo;
        private readonly IBookRepository _bookRepo;

        public ReportsService(IIssueRepository issueRepo, IBookRepository bookRepo)
        {
            _issueRepo = issueRepo;
            _bookRepo = bookRepo;
        }

        /// <summary>
        /// Получить все текущие просроченные выдачи
        /// </summary>
        public async Task<IEnumerable<IssueDto>> GetOverdueIssuesAsync()
        {
            var issues = await _issueRepo.GetActiveIssuesWithDetailsAsync();

            return issues
                .Where(i => i.Returnuntil.ToDateTime(TimeOnly.MinValue) < DateTime.Now)
                .OrderBy(i => i.Returnuntil.ToDateTime(TimeOnly.MinValue))
                .Select(MapIssueToDto)
                .ToList();
        }

        /// <summary>
        /// Получить историю выдачи книги по ключу или названию
        /// </summary>
        public async Task<IEnumerable<IssueDto>> GetBookHistoryAsync(string bookKeyOrTitle)
        {
            var book = await FindBookAsync(bookKeyOrTitle);
            if (book == null) return new List<IssueDto>();

            var issues = await _issueRepo.GetByBookKeyWithDetailsAsync(book.Key);
            return issues
                .OrderByDescending(i => i.Dateofissue.ToDateTime(TimeOnly.MinValue))
                .Select(MapIssueToDto)
                .ToList();
        }

        /// <summary>
        /// Получить Title и Subtitle книги по ключу или названию
        /// </summary>
        public async Task<(string Title, string? Subtitle)?> GetBookInfoAsync(string bookKeyOrTitle)
        {
            var book = await FindBookAsync(bookKeyOrTitle);
            if (book == null) return null;
            return (book.Title, book.Subtitle);
        }

        /// <summary>
        /// Вспомогательный метод для поиска книги по Key или Title
        /// </summary>
        private async Task<Models.Books?> FindBookAsync(string keyOrTitle)
        {
            var books = await _bookRepo.GetAllAsync();
            return books.FirstOrDefault(b =>
                b.Key.Equals(keyOrTitle, StringComparison.OrdinalIgnoreCase) ||
                b.Title.Contains(keyOrTitle, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Вспомогательный метод для маппинга Issues в IssueDto
        /// </summary>
        private IssueDto MapIssueToDto(Models.Issues i) => new IssueDto
        {
            Issueid = i.Issueid,
            Customerid = i.Customerid,
            CustomerName = i.Customer?.Name ?? "[No Customer]",
            BookKey = i.Bookkey,
            BookTitle = i.BookkeyNavigation?.Title ?? "[No Title]",
            DateOfIssue = i.Dateofissue.ToDateTime(TimeOnly.MinValue),
            ReturnUntil = i.Returnuntil.ToDateTime(TimeOnly.MinValue),
            ReturnDate = i.Returndate?.ToDateTime(TimeOnly.MinValue),
            Renewed = i.Returnuntil > i.Dateofissue.AddDays(21)
        };
    }
}
