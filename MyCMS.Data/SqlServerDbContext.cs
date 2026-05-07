using Microsoft.EntityFrameworkCore;

namespace MyCMS.Data
{
    public class SqlServerDbContext : AppDbContext
    {
        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options)
        {
        }
    }
}
