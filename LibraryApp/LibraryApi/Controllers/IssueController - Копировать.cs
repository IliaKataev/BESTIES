using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService; 
        private readonly ICustomerService _customerService;
        private readonly IBookService _bookService;

        public IssuesController(IIssueService issueService, ICustomerService customerService, IBookService bookService)
        {
            _issueService = issueService;
            _customerService = customerService;
            _bookService = bookService;
        }

        [HttpPost("issue")]
        public async Task<ActionResult<IssueDto>> IssueBook([FromBody] IssueRequest request)
        {
            // Создаем новую выдачу книги
            var issue = await _issueService.IssueBookAsync(
                request.CustomerId,
                request.BookKey,
                request.ReturnUntil
            );

            if (issue == null)
                return BadRequest("Не удалось оформить выдачу книги.");

            var book = await _bookService.GetBookDetailsAsync(request.BookKey);
            // Получаем данные о клиенте
            var customer = await _customerService.GetCustomerByIdAsync(request.CustomerId);

            // Преобразуем в DTO (чистая модель для JSON)
            var dto = new IssueDto
            {
                Issueid = issue.Issueid,
                Customerid = issue.Customerid,
                BookKey = issue.Bookkey,
                BookTitle = book?.Title ?? "[Неизвестная книга]",
                CustomerName = customer?.Name ?? "[Неизвестный читатель]",
                DateOfIssue = issue.Dateofissue.ToDateTime(TimeOnly.MinValue),
                ReturnUntil = issue.Returnuntil.ToDateTime(TimeOnly.MinValue),
                ReturnDate = issue.Returndate?.ToDateTime(TimeOnly.MinValue),
                Renewed = false
            };

            return Ok(dto);
        }


        [HttpPost("return/{issueId}")]
        public async Task<ActionResult> ReturnBook(long issueId)
        {
            await _issueService.ReturnBookAsync(issueId);
            return NoContent();
        }

        [HttpPost("renew/{issueId}")]
        public async Task<IActionResult> Renew(long issueId)
        {
            try
            {
                var renewedIssue = await _issueService.RenewIssueAsync(issueId);
                return Ok(renewedIssue);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("active")]
        public async Task<ActionResult<List<IssueDto>>> GetActiveIssues()
        {
            var issues = await _issueService.GetActiveIssuesAsync();
            return Ok(issues);
        }

        [HttpGet("history/{customerId}")]
        public async Task<ActionResult<List<IssueDto>>> GetHistory(long customerId)
        {
            var history = await _issueService.GetIssueHistoryAsync(customerId);
            return Ok(history);
        }


        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult> GetCustomerCirculation(long customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound("Customer not found");

            var activeIssues = await _issueService.GetActiveIssuesAsync();
            var history = await _issueService.GetIssueHistoryAsync(customerId);

            return Ok(new
            {
                Customer = customer,
                CurrentIssues = activeIssues.Where(i => i.Customerid == customerId),
                History = history
            });
        }


    }

    public class IssueRequest
    {
        public long CustomerId { get; set; }
        public string BookKey { get; set; } = string.Empty;
        public DateOnly ReturnUntil { get; set; }
    }
}
