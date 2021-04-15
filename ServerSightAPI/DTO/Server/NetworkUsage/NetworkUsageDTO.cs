using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server.NetworkUsage
{
    public class NetworkUsageDTO
    {
        [Required]
        public double DownloadInBytes { get; set; }
        [Required]
        public double UploadInBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}