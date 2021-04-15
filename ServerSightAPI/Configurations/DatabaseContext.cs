using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.Configurations
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        private DbSet<Server> Servers { get; set; }
        private DbSet<ApiKey> ApiKeys { get; set; }
        private DbSet<NetworkAdapterServer> NetworkAdapterServers { get; set; }
        private DbSet<HardDiskServer> HardDiskServers { get; set; }
        private DbSet<PortServer> PortServers { get; set; }
        private DbSet<CpuUsageServer> CpuUsageServers { get; set; }
        private DbSet<RamUsage> RamUsages { get; set; }
        private DbSet<NetworkUsage> NetworkUsage { get; set; }
        private DbSet<ServerEvent> ServerEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ServerEvent>()
                .Property(e => e.EventType)
                .HasConversion(
                    v => v.ToString(),
                    v => (EventType)Enum.Parse(typeof(EventType), v));
            base.OnModelCreating(modelBuilder);
        }
    }
}