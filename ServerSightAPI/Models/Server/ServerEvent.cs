using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class ServerEvent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Description { get; set; }
        public EventType EventType { get; set; }
        
        [Column(TypeName = "timestamp")] 
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(Server))] 
        public string ServerId { get; set; }
        public Server Server { get; set; }
    }

    public enum EventType
    {
        PowerStatus
    }
}