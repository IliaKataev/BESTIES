using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Customers>> Login([FromBody] LoginRequest request)
        {
            var customer = await _customerService.LoginAsync(request.Phone);
            if (customer == null) return Unauthorized("Invalid phone number");
            return Ok(customer);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customers>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            foreach (var c in customers)
                Console.WriteLine($"API DEBUG: {c.Customerid} - {c.Name}");
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customers>> GetById(long id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Customers customer)
        {
            try
            {
                await _customerService.AddCustomerAsync(customer);
                return CreatedAtAction(nameof(GetById), new { id = customer.Customerid }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message + " | " + ex.InnerException?.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, [FromBody] JsonElement customerJson)
        {
            var customer = JsonSerializer.Deserialize<CustomerDto>(customerJson.GetRawText());
            if (customer == null || customer.Customerid != id) return BadRequest();

            var existing = await _customerService.GetCustomerByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = customer.Name;
            existing.Address = customer.Address;
            existing.Zip = customer.Zip;
            existing.City = customer.City;
            existing.Phone = customer.Phone;
            existing.Email = customer.Email;

            await _customerService.UpdateCustomerAsync(existing);
            return NoContent();
        }


        [HttpPut("test/{id}")]
        public IActionResult TestPut(long id)
        {
            return Ok($"PUT {id} works");
        }




    }

    public class LoginRequest
    {
        public string Phone { get; set; } = string.Empty;
    }
}
