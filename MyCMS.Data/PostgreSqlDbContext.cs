using Microsoft.EntityFrameworkCore;
using MyCMS.Core.Entities;
using MyCMS.Data.Data;

namespace MyCMS.Data
{
    public class PostgreSqlDbContext : AppDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default themes
            modelBuilder.Entity<Theme>().HasData(ThemeSeedData.GetDefaultThemes());
        }
    }
}
