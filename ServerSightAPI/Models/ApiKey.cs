using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models
{
    public class ApiKey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Key { get; set; }

        [ForeignKey(nameof(User))] public string OwnedById { get; set; }

        public User OwnedBy { get; set; }

        [Column(TypeName = "timestamp")] public DateTime CreatedAt { get; set; }
    }
}