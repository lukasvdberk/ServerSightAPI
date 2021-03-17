using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models.Server
{
    public class HardDiskServer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string DiskName { get; set; }
        public float SpaceAvailable { get; set; }
        public float SpaceTotal { get; set; }

        [ForeignKey(nameof(Server))] public string ServerId { get; set; }

        public Server Server { get; set; }
    }
}