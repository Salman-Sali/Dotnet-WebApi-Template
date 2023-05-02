using Microsoft.AspNetCore.Mvc;
using Application.Auth.Commands.Login;
using Configurations.Extentions;

namespace WebApi.Controllers
{
    public class AuthController : ApiControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginCommand command)
        {
            var token = await Mediator.Send(command);
            return this.OkResult(new { token =  token });
        }
    }
}
