using Microsoft.EntityFrameworkCore;

namespace MyCMS.Data
{
    public class PostgreSqlDbContext : AppDbContext
    {
        public PostgreSqlDbContext(DbContextOptions<PostgreSqlDbContext> options) : base(options)
        {
        }
    }
}
