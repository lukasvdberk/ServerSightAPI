using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSightAPI.Models
{
    public class FirebaseDevice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string DeviceKey { get; set; }

        [ForeignKey(nameof(User))] public string OwnedById { get; set; }

        public User OwnedBy { get; set; }
    }
}