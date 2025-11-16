using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExhibitionsController : ControllerBase
{
    private readonly IExhibitionService _service;
    public ExhibitionsController(IExhibitionService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<ExhibitionDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<ExhibitionDto>> GetById(int id)
    {
        var exhibition = await _service.GetByIdAsync(id);
        return exhibition == null ? NotFound() : Ok(exhibition);
    }

    [HttpPost]
    public async Task<ActionResult<ExhibitionDto>> Create([FromBody] CreateExhibitionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.ExhibitionId }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExhibitionDto>> Update(int id, [FromBody] UpdateExhibitionDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
