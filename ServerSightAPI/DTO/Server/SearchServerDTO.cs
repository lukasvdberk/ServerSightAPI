using System.ComponentModel;

namespace ServerSightAPI.DTO.Server
{
    public class SearchServerDto
    {
        public string Name { get; set; }
        public bool? PowerStatus { get; set; } = null;
    }
}