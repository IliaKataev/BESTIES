using LibraryApi.DTOs;

namespace LibraryApi.Services
{
    public interface IReportsService
    {
        /// <summary>
        /// Получить все текущие просроченные выдачи для вкладки "Напоминания"
        /// </summary>
        Task<IEnumerable<IssueDto>> GetOverdueIssuesAsync();

        /// <summary>
        /// Получить историю выдачи книги по Key или Title
        /// </summary>
        Task<IEnumerable<IssueDto>> GetBookHistoryAsync(string bookKeyOrTitle);

        /// <summary>
        /// Получить информацию о книге (Title и Subtitle) по Key или названию книги
        /// </summary>
        Task<(string Title, string? Subtitle)?> GetBookInfoAsync(string bookKeyOrTitle);
    }
}
