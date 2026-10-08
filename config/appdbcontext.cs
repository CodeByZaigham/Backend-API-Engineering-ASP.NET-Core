using Microsoft.EntityFrameworkCore;
using shopforge_backend.models;

namespace shopforge_backend.config
{
    public class appdbcontext : DbContext
    {
        public appdbcontext(DbContextOptions<appdbcontext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(x => x.Role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}


// we need to create models and then add them in appdbcontext then
// initialize migrations and update db