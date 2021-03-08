using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class CreateServerDto
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}