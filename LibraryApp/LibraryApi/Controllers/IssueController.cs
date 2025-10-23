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

        public IssuesController(IIssueService issueService, ICustomerService customerService)
        {
            _issueService = issueService;
            _customerService = customerService;
        }

        [HttpPost("issue")]
        public async Task<ActionResult<Issues>> IssueBook([FromBody] IssueRequest request)
        {
            var issue = await _issueService.IssueBookAsync(request.CustomerId, request.BookKey, request.ReturnUntil);
            return Ok(issue);
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
