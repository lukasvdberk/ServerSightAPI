using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server.NetworkAdapterServer
{
    public class CreateNetworkAdapterServerDto
    {
        public string AdapterName { get; set; }
        // regex for valid ip check
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$")]
        public string Ip { get; set; }
    }
}