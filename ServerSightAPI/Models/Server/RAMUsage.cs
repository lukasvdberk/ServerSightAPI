using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class RamUsage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public double UsageInBytes { get; set; }
        public double TotalAvailableInBytes { get; set; }

        [Column(TypeName = "timestamp")] 
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(Server))] 
        public string ServerId { get; set; }
        public Server Server { get; set; }
    }
}