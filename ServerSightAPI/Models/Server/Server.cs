namespace ServerSightAPI.Models.Server
{
    public class Server
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        // is server on or off
        public bool PowerStatus { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
        // path to image rather than the actual file
        public string ImagePath { get; set; }
    }
}