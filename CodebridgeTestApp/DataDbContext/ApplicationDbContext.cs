using CodebridgeTestApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CodebridgeTestApp.DataDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Dog> Dogs => Set<Dog>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dog>().HasData(
                new Dog { name = "Neo", color = "red & amber", tail_length = 22, weight = 32 },
                new Dog { name = "Jessy", color = "black & white", tail_length = 7, weight = 14 }
                );
        }
    }
}
