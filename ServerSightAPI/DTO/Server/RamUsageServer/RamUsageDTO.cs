using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.DTO.Server
{
    public class RamUsageDto
    {
        public string Id { get; set; }
        public double UsageInBytes { get; set; }
        public double TotalAvailableInBytes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}