using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Apex.Catering.Data
{
    public class CateringDbContext : DbContext
    {
        public DbSet<Menu> Menus { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodBooking> FoodBookings { get; set; }
        public DbSet<MenuFoodItem> MenuFoodItems { get; set; }
        private String DbPath { get; set; } = string.Empty;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuFoodItem>()
                   .HasKey(mf => new { mf.MenuId, mf.FoodItemId });
            modelBuilder.Entity<MenuFoodItem>()
                   .HasOne(mf => mf.Menu)
                   .WithMany(m => m.MenuFoodItems)
                   .HasForeignKey(mf => mf.MenuId);
            modelBuilder.Entity<MenuFoodItem>()
                   .HasOne(mf => mf.FoodItem)
                   .WithMany(f => f.MenuFoodItems)
                   .HasForeignKey(mf => mf.FoodItemId);
        }

        public CateringDbContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "Catering.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
