global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NomNomAPI.Data
{
    public class DataContext : IdentityDbContext//identity padeda sukurti daug lenteliu ir reikiamos informacijos kaip roles, logins ir tt.
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
               
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=NomNomdb;Trusted_Connection=true;TrustServerCertificate=true");
        }

        public DbSet<FoodItem> foodItems { get; set; }
        public DbSet<Store> Stores { get; set; }
    }
}
