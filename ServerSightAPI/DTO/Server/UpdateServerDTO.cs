using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class UpdateServerDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public bool PowerStatus { get; set; } = true;
    }
}