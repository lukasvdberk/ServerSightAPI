using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class NotificationResourceThresholdDTO
    {
        public string Id { get; set; }
        
        [Required]
        [Range(0, 100)] // needs to be in percentage
        public int RamUsageThresholdInPercentage { get; set; }
        [Required]
        [Range(0, 100)] // needs to be in percentage
        public int CpuUsageThresholdInPercentage { get; set; }
        [Required]
        [Range(0, 100)] // needs to be in percentage
        public int HardDiskUsageThresholdInPercentage { get; set; }
    }
}