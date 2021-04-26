using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.User
{
    public class CreateFirebaseDTO
    {
        [Required]
        public string DeviceKey { get; set; }
    }
}