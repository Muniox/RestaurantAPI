using Microsoft.EntityFrameworkCore;

namespace RestaurantAPI.Entities
{
    public class RestaurantDbContext : DbContext
    {
        //var connString = builder.Configuration.GetConnectionString("DatabaseStoreContext");
        private string _connectionString = "Server=127.0.0.1;Database=RestaurantDB;User=root;Password=<3database^_^";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        //Można użyć fluent api albo annotation, w fluent można zrobić więcej rzeczy

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Restaurant>()
        //        .Property(r => r.Name)
        //        .IsRequired()
        //        .HasMaxLength(25);

        //    modelBuilder.Entity<Dish>()
        //        .Property(d => d.Name)
        //        .IsRequired();

        //    modelBuilder.Entity<Address>()
        //        .Property(a => a.City)
        //        .IsRequired()
        //        .HasMaxLength(50);

        //    modelBuilder.Entity<Address>()
        //        .Property(a => a.Street)
        //        .IsRequired()
        //        .HasMaxLength(50);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //UseSqlServer // driver dla mssql
            //UseMySql // driver dla mysql
            optionsBuilder.UseMySQL(_connectionString);
        }
    }
}