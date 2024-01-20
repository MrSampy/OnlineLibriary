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
    public class UserController
    {
        private readonly ICrud<UserDTO> _userService;

        public UserController(ICrud<UserDTO> userService)
        {
            _userService = userService;
        }

        // GET: api/user
        // GET: api/user?pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            try
            {
                return new OkObjectResult(await _userService.GetAllAsync(new PaginationModel { PageNumber = pageNumber, PageSize = pageSize }));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        //GET: api/user/id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            try
            {
                return new OkObjectResult(await _userService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // POST: api/user
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserDTO user)
        {
            try
            {
                return new OkObjectResult(await _userService.AddAsync(user));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // PUT: api/user
        [HttpPut]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromBody] UserDTO user)
        {
            try
            {
                return new OkObjectResult(await _userService.UpdateAsync(user));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // DELETE: api/user/id
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int id)
        {
            try
            {
                return new OkObjectResult(await _userService.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
