using MVCCRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace MVCCRUD.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
