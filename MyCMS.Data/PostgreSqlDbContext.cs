using Microsoft.EntityFrameworkCore;

namespace MyCMS.Data
{
    public class PostgreSqlDbContext : AppDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
