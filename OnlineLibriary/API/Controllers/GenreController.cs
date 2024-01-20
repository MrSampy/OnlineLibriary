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
    public class GenreController
    {
        private readonly ICrud<GenreDTO> _genreService;

        public GenreController(ICrud<GenreDTO> genreService)
        {
            _genreService = genreService;
        }

        // GET: api/genre
        // GET: api/genre?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                return new OkObjectResult(await _genreService.GetAllAsync(new PaginationModel { PageNumber = pageNumber, PageSize = pageSize }));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        //GET: api/genre/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            try
            {
                return new OkObjectResult(await _genreService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // POST: api/genre
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> AddGenre([FromBody] GenreDTO genre)
        {
            try
            {
                return new OkObjectResult(await _genreService.AddAsync(genre));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // PUT: api/genre
        [HttpPut]
        public async Task<ActionResult<GenreDTO>> UpdateGenre([FromBody] GenreDTO genre)
        {
            try
            {
                return new OkObjectResult(await _genreService.UpdateAsync(genre));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // DELETE: api/author/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenreDTO>> DeleteGenre(int id)
        {
            try
            {
                return new OkObjectResult(await _genreService.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
