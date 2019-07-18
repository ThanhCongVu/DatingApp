using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 3, ErrorMessage = "You must verify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}

// We have 2 options to validate our username, either through User model or UserForRegisterDto
// however, since in User model we have 4 parameters while only Username need to be validated
// So that it makes more sense if we validate Username in UserForRegisterDto