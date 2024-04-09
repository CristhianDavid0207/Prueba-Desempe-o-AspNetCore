using Microsoft.EntityFrameworkCore;
using solucion.Models;

namespace solucion.Data
{
    public class PruebaContext : DbContext
    {
        public PruebaContext(DbContextOptions<PruebaContext> options) : base(options){}

        public DbSet<Employee> Employees {get; set;}
        public DbSet<Job> Jobs {get; set;}
    }
}
