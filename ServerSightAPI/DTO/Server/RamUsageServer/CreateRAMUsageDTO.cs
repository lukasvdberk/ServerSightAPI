using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class CreateRamUsageDto
    {
        [Required]
        public double UsageInBytes { get; set; }
        [Required]
        public double TotalAvailableInBytes { get; set; }
    }
}