using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStoreApp.UseCases.Interfaces;

namespace OnlineStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _login;
        public LoginController(ILoginService login)
        {
            _login = login;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginModel)
        {

            AuthenticationResultDTO result = null;
            try
            {
                if (!ModelState.IsValid)
                    return Unauthorized();

                result = await _login.LoginAsync(loginModel);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(result);
            }

        }
    }
}
