using Microsoft.EntityFrameworkCore;

namespace SEServer.Auth.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var player = modelBuilder.Entity<Player>();
            player.HasKey(x => x.Id);
            player.Property(entity => entity.UserName).HasMaxLength(20);
        }
    }
}
