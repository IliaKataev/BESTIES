using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ClientController(LibraryContext context)
        {
            _context = context;
        }

        // POST: api/client/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return BadRequest("Телефон обязателен");

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == phone);

            if (customer == null)
                return Unauthorized("Неверный номер телефона");

            // возвращаем минимальные данные клиента
            return Ok(new
            {
                customer.Customerid,
                customer.Name,
                customer.Phone
            });
        }
    }
}
