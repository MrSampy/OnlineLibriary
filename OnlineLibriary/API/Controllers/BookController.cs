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
    public class BookController
    {
        private readonly ICrud<BookDTO> _bookService;

        public BookController(ICrud<BookDTO> bookService)
        {
            _bookService = bookService;
        }

        // GET: api/book
        // GET: api/book?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                return new OkObjectResult(await _bookService.GetAllAsync(new PaginationModel { PageNumber = pageNumber, PageSize = pageSize }));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        //GET: api/book/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            try
            {
                return new OkObjectResult(await _bookService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // POST: api/book
        [HttpPost]
        public async Task<ActionResult<BookDTO>> AddBook([FromBody] BookDTO book)
        {
            try
            {
                return new OkObjectResult(await _bookService.AddAsync(book));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // PUT: api/book
        [HttpPut]
        public async Task<ActionResult<BookDTO>> UpdateBook([FromBody] BookDTO book)
        {
            try
            {
                return new OkObjectResult(await _bookService.UpdateAsync(book));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // DELETE: api/book/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<BookDTO>> DeleteBook(int id)
        {
            try
            {
                return new OkObjectResult(await _bookService.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
