using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using BusinessLogic.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController
    {
        private readonly ICrud<AuthorDTO> _authorService;

        public AuthorController(ICrud<AuthorDTO> authorService)
        {
            _authorService = authorService;
        }

        // GET: api/author
        // GET: api/author?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                return new OkObjectResult(await _authorService.GetAllAsync(new PaginationModel { PageNumber = pageNumber, PageSize = pageSize }));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        //GET: api/author/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            try
            {
                return new OkObjectResult(await _authorService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // POST: api/author
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> AddAuthor([FromBody] AuthorDTO author)
        {
            try
            {
                return new OkObjectResult(await _authorService.AddAsync(author));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // PUT: api/author
        [HttpPut]
        public async Task<ActionResult<AuthorDTO>> UpdateAuthor([FromBody] AuthorDTO author)
        {
            try
            {
                return new OkObjectResult(await _authorService.UpdateAsync(author));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // DELETE: api/author/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<AuthorDTO>> DeleteAuthor(int id)
        {
            try
            {
                return new OkObjectResult(await _authorService.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
