using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ServerSightAPI.Models.Server
{
    public class Server
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        
        [ForeignKey(nameof(User))]
        public string OwnedById { get; set; }
        public User OwnedBy { get; set; }
        
        // is server on or off
        public bool PowerStatus { get; set; }
        public string Description { get; set; }

        [Column(TypeName="timestamp")]
        public DateTime CreatedAt { get; set; }
        
        // path to image rather than the actual file
        public string ImagePath { get; set; }
    }
}