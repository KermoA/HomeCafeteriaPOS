using HomeCafeteriaPOS.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeCafeteriaPOS.Data
{
    public class HomeCafeteriaDbContext : DbContext
    {
        public HomeCafeteriaDbContext(DbContextOptions<HomeCafeteriaDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
    }
}
