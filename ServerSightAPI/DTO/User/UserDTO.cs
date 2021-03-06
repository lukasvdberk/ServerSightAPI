using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.User
{
    public class UserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}