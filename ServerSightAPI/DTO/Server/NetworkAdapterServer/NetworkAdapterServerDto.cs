using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server.NetworkAdapterServer
{
    public class NetworkAdapterServerDto
    {
        public string Id { get; set; }
        public string AdapterName { get; set; }
        // regex for valid ip check
        public string Ip { get; set; }
    }
}