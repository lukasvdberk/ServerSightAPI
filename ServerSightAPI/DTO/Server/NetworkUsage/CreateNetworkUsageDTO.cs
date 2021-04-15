using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server.NetworkUsage
{
    public class CreateNetworkUsageDto
    {
        [Required]
        public double DownloadInBytes { get; set; }
        [Required]
        public double UploadInBytes { get; set; }
    }
}