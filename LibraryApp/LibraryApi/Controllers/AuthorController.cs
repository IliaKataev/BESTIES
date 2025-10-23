using LibraryApi.DTOs;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuthorDto>>> GetAll()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<AuthorDto>> GetByKey(string key)
        {
            var author = await _authorService.GetAuthorByKeyAsync(key);
            if (author == null) return NotFound();
            return Ok(author);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<AuthorDto>>> Search([FromQuery] string name)
        {
            var authors = await _authorService.SearchAuthorsAsync(name);
            return Ok(authors);
        }
    }
}
