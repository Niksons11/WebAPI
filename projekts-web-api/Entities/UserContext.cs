using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.ComTypes;

namespace WebApi.Entities
{
    public class UserContext : IdentityDbContext<IdentityUser>
    {

        public UserContext(DbContextOptions options) : base(options) { Database.EnsureCreated(); }

        public DbSet<User> userslist { get; set; }
        public DbSet<FileProperties> extensionlist { get; set; }
        public DbSet<LimitedSize> propertylist { get; set; }
        public DbSet<LimitedResolution> resolutionlist { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<User>().HasData(new User { Id = 1, FirstName = "admin", LastName = "admin", Username = "admin", Password = "admin", Role = "admin" });

        }


    }
}
