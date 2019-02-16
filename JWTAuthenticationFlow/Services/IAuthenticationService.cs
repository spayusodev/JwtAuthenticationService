using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthenticationFlow.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterUser(ViewModels.RegisterViewModel model);
        Task<string> LoginUser(ViewModels.LoginViewModel model);
    }
}
