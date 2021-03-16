using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class PortServer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        
        public int Port { get; set; }
        
        [ForeignKey(nameof(Server))]
        public string ServerId { get; set; }
        public Server Server { get; set; }
    }
}