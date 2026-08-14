using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.Model;

namespace PizzaShop.Data
{
    public class PizzaShopDbContext : DbContext
    {
        public PizzaShopDbContext(DbContextOptions<PizzaShopDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>()
                .HasData(new List<Pizza>()
                {
                    new Pizza
                    {
                        Id = 1,
                        Name = "Margherita", 
                        Price = 15.50m, 
                        Description = "Pizza with tomatoes, mozzarella, basil, extra virgin olive oil" 
                    },
                    new Pizza
                    {
                        Id = 2,
                        Name = "Pepperoni",
                        Price = 12.99m,
                        Description = "Pizza with pepperoni and mozzarella"
                    },
                    new Pizza
                    {
                        Id = 3,
                        Name = "Four Cheese",
                        Price = 16.82m,
                        Description = "Pizza with Mozzarella, Gorgonzola, Fontina, Parmesan"
                    },
                });
        }
        public DbSet<Pizza> Pizzas { get; set; }
    }
}
