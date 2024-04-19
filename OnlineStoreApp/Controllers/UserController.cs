using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminOrUser")]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdminOrUser")]
        [HttpGet("[action]/{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            if (id <= 0)
                return NotFound();

            var userDto = await _userService.GetAsync(id);
            return Ok(userDto);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(UserCreateDTO userCreationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                ResultService resultService = await _userService.InsertAsync(userCreationDto);

                if (!resultService.IsSuccess)
                    return BadRequest(resultService);

                return Ok(resultService);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultService
                {
                    IsSuccess = false,
                    Data = null,
                    Message = ex.Message
                });
            }
        }


    }
}
