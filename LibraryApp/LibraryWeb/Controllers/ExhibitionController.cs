using LibraryWeb.Models;
using LibraryWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWeb.Controllers
{
    public class ExhibitionsController : Controller
    {
        private readonly ExhibitionsApi _api;
        public ExhibitionsController(ExhibitionsApi api) => _api = api;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadAll()
        {
            var exhibitions = await _api.GetAllAsync();
            return Json(exhibitions);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var exhibition = await _api.GetByIdAsync(id);
            if (exhibition == null) return NotFound();
            return Json(exhibition);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExhibitionDto dto)
        {
            var success = await _api.CreateAsync(dto);
            return Json(new { success });
        }

        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateExhibitionDto dto)
        {
            var success = await _api.UpdateAsync(id, dto);
            return Json(new { success });
        }
    }
}
