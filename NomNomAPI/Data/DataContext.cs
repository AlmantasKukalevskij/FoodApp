global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NomNomAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=NomNomdb;Trusted_Connection=true;TrustServerCertificate=true");
        }

        public virtual DbSet<FoodItem> foodItems { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}
