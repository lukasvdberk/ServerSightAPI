using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    /*
     * When a notification should be sent if a certain resource reaches a threshold (for example send notification on 80% cpu usage)
     */
    public class NotificationResourceThreshold
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int RamUsageThresholdInPercentage { get; set; }
        public int CpuUsageThresholdInPercentage { get; set; }
        public int HardDiskUsageThresholdInPercentage { get; set; }
        
        [ForeignKey(nameof(User))] public string OwnedById { get; set; }

        public User OwnedBy { get; set; }
    }
}