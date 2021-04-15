using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class NetworkUsage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public double DownloadInBytes { get; set; }
        public double UploadInBytes { get; set; }
        
        [Column(TypeName = "timestamp")] 
        public DateTime CreatedAt { get; set; }
        
        [ForeignKey(nameof(Server))] 
        public string ServerId { get; set; }
        public Server Server { get; set; }
    }
}