using CRUD.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{
    public class CrudDbContext : DbContext
    {
        public CrudDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
