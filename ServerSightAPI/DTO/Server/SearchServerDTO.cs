using System.ComponentModel;

namespace ServerSightAPI.DTO.Server
{
    public class SearchServerDto
    {
        public string Title { get; set; }
        public bool PowerStatus { get; set; } = true;
    }
}