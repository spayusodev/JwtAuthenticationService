using System.ComponentModel.DataAnnotations;

namespace JWTAuthenticationFlow.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
