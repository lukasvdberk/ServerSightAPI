using System.ComponentModel;

namespace ServerSightAPI.DTO.Server
{
    public class SearchServerDto
    {
        public string Title { get; set; }
        [DefaultValue(true)]
        public bool PowerStatus { get; set; }
    }
}