using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthenticationFlow.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<IdentityUser> userManager, 
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<string> RegisterUser(ViewModels.RegisterViewModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException();
            }

            var newUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await this._userManager.CreateAsync(newUser, model.Password);

            if(result.Succeeded)
            {
                await this._userManager.AddToRoleAsync(newUser, "Customer");
            }

            return newUser.UserName;
        }

        public async Task<string> LoginUser(ViewModels.LoginViewModel model)
        {
            var user = await this._userManager.FindByNameAsync(model.UserName);

            if(user != null && await this._userManager.CheckPasswordAsync(user, model.Password))
            {
                var claim = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                };

                var signingKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(this._configuration["Jwt:SigningKey"]));

                var expiryInMinutes = Convert.ToInt32(this._configuration["Jwt:ExpiryInMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: this._configuration["Jwt:Site"],
                    audience: this._configuration["Jwt:Site"],
                    expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        }
    }
}
