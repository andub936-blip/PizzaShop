using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.Model;

namespace PizzaShop.Data
{
    public class PizzaShopDbContext : DbContext
    {
        public PizzaShopDbContext(DbContextOptions<PizzaShopDbContext> options) : base(options){}

        public DbSet<Pizza> Pizzas { get; set; }
    }
}
