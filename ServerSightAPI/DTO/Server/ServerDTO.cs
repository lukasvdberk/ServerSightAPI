using System;
using System.ComponentModel.DataAnnotations;

namespace ServerSightAPI.DTO.Server
{
    public class ServerDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool PowerStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImagePath { get; set; }
    }
}