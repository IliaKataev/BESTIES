using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Repositories;

namespace LibraryApi.Services
{
    public interface IIssueService
    {
        Task<Issues> IssueBookAsync(long customerId, string bookKey, DateOnly returnUntil);
        Task ReturnBookAsync(long issueId);
        Task<List<IssueDto>> GetActiveIssuesAsync();
        Task<List<IssueDto>> GetIssueHistoryAsync(long customerId);
        Task<Issues> RenewIssueAsync(long issueId);
    }


    public class IssueService : IIssueService
    {
        private readonly IIssueRepository _issueRepo;

        public IssueService(IIssueRepository issueRepo)
        {
            _issueRepo = issueRepo;
        }

        public async Task<Issues> IssueBookAsync(long customerId, string bookKey, DateOnly returnUntil)
        {
            var issue = new Issues
            {
                Customerid = customerId,
                Bookkey = bookKey,
                Dateofissue = DateOnly.FromDateTime(DateTime.Now),
                Returnuntil = returnUntil
            };
            return await _issueRepo.AddAsync(issue);
        }

        public async Task<Issues> RenewIssueAsync(long issueId)
        {
            var issue = await _issueRepo.GetByIdAsync(issueId);
            if (issue == null)
                throw new Exception("Issue not found");

            if (issue.Returndate != null)
                throw new Exception("Cannot renew a returned book");

            // Проверка: если Returnuntil уже больше чем Dateofissue + 21 дней, значит продление уже было
            if (issue.Returnuntil > issue.Dateofissue.AddDays(21))
                throw new Exception("Book has already been renewed once");

            // Продление на 7 дней
            issue.Returnuntil = issue.Returnuntil.AddDays(7);

            await _issueRepo.UpdateAsync(issue);

            return issue;
        }


        public async Task ReturnBookAsync(long issueId)
        {
            var issue = await _issueRepo.GetByIdAsync(issueId);
            if (issue == null) throw new Exception("Issue not found");
            issue.Returndate = DateOnly.FromDateTime(DateTime.Now);
            await _issueRepo.UpdateAsync(issue);
        }

        public async Task<List<IssueDto>> GetIssueHistoryAsync(long customerId)
        {
            var issues = await _issueRepo.GetByCustomerIdWithDetailsAsync(customerId);

            return issues.Select(i => new IssueDto
            {
                Issueid = i.Issueid,
                BookTitle = i.BookkeyNavigation?.Title ?? "[No Title]",
                CustomerName = i.Customer?.Name ?? "[No Customer]",
                DateOfIssue = i.Dateofissue,
                ReturnUntil = i.Returnuntil,
                ReturnDate = i.Returndate
            }).ToList();
        }

        public async Task<List<IssueDto>> GetActiveIssuesAsync()
        {
            var issues = await _issueRepo.GetActiveIssuesWithDetailsAsync();

            return issues.Select(i => new IssueDto
            {
                Issueid = i.Issueid,
                BookTitle = i.BookkeyNavigation?.Title ?? "[No Title]",
                CustomerName = i.Customer?.Name ?? "[No Customer]",
                DateOfIssue = i.Dateofissue,
                ReturnUntil = i.Returnuntil,
                ReturnDate = i.Returndate
            }).ToList();
        }

    }

}
