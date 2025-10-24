using Microsoft.AspNetCore.Mvc;
using LibraryApi.Models;

namespace LibraryApi.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        // GET: api/reports/reminders
        [HttpGet("reminders")]
        public IActionResult GetReminders()
        {
            // для начала просто вернем пустой список
            return Ok(new List<object>());
        }

        // GET: api/reports/book-history
        [HttpGet("book-history")]
        public IActionResult GetBookHistory()
        {
            return Ok(new List<object>());
        }
    }
}
