using System.ComponentModel.DataAnnotations;

namespace JwtTokenAuthentication.Models
{
    public class UserAuthenticationModel
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}