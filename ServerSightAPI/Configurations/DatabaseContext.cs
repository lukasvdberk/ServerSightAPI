using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerSightAPI.Models;
using ServerSightAPI.Models.Server;

namespace ServerSightAPI.Configurations
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        private DbSet<Server> Servers { get; set; }
        private DbSet<ApiKey> ApiKeys { get; set; }
        public DatabaseContext(DbContextOptions options): base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}