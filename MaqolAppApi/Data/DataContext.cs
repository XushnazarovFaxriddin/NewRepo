using MaqolAppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MaqolAppApi.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
        }

        public DbSet<Maqola> Maqolalar { get; set; }
        public DbSet<Muallif> Mualliflar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maqola>()
                .Property(m => m.YaratilganVaqti)
                .HasDefaultValueSql("getdate()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
