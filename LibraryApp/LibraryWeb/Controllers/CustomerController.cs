using Microsoft.AspNetCore.Mvc;
using LibraryWeb.Models;
using LibraryWeb.Services;

namespace LibraryWeb.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerApi _customerApi; // сервис для работы с клиентами

        public CustomersController(CustomerApi customerApi)
        {
            _customerApi = customerApi;
        }

        public async Task<IActionResult> Index(string searchId = "", string searchName = "")
        {
            var customers = await _customerApi.GetCustomersAsync();

            if (!string.IsNullOrWhiteSpace(searchId))
                customers = customers
                    .Where(c => c.Customerid.ToString().Contains(searchId, StringComparison.OrdinalIgnoreCase))
                    .ToList();


            if (!string.IsNullOrWhiteSpace(searchName))
                customers = customers.Where(c => c.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerDto customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _customerApi.UpdateCustomerAsync(customer);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CustomerDto customer)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _customerApi.AddCustomerAsync(customer);
            return RedirectToAction("Index");
        }
    }
}
