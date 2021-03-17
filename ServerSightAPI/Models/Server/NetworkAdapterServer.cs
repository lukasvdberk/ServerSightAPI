using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class NetworkAdapterServer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string AdapterName { get; set; }
        public string Ip { get; set; }

        [ForeignKey(nameof(Server))] public string ServerId { get; set; }

        public Server Server { get; set; }
    }
}