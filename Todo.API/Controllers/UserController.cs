using Microsoft.AspNetCore.Mvc;
using Todo.API.Commands;
using Todo.API.Services;

namespace Todo.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUser command)
        {
            var id = await _userService.Register(command);
            return Ok(id);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUser(LoginUser command)
        {
            var jwt = await _userService.Login(command);
            return Ok(jwt);
        }
    }
}