using System;

namespace ServerSightAPI.DTO.ApiKey
{
    public class ApiKeyDto
    {
        public string Key { get; set; }
        public Models.User OwnedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}