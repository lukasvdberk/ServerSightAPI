using System;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.DTO.Server.ServerEvents
{
    public class ServerEventDto
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public EventType EventType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}