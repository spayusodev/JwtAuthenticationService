using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JWTAuthenticationFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly Services.IAuthenticationService _service;
        
        public AuthenticationController(Services.IAuthenticationService authService)
        {
            this._service = authService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> InsertUser([FromBody] ViewModels.RegisterViewModel model)
        {
            var userName = await this._service.RegisterUser(model);
            return Ok(new { UserName = userName});
        }
        
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] ViewModels.LoginViewModel model)
        {
            var token = await this._service.LoginUser(model);

            if(string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}