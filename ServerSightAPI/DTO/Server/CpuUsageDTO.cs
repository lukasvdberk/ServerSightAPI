using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class CpuUsageDto
    {
        [Required]
        public double AverageCpuUsagePastMinute { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}