using PizzaShop.Domain.Interfaces;
using PizzaShop.Domain.Model;

namespace PizzaShop.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly ILogger<PizzaService> _logger;
        private List<Pizza> _pizzas = new()
        {
            new Pizza { Id = 1, Name = "Margherita", Price = 15.12m, Description = "Pizza with tomatoes, mozzarella, basil, extra virgin olive oil" },
            new Pizza { Id = 2, Name = "Pepperoni", Price = 12.99m, Description = "Pizza with pepperoni and mozzarella" },
            new Pizza { Id = 3, Name = "Four Cheese", Price = 15.12m, Description = "Pizza with Mozzarella, Gorgonzola, Fontina, Parmesan" }
        };

        public PizzaService(ILogger<PizzaService> logger)
        {
            _logger = logger;
        }

        public IReadOnlyCollection<Pizza> GetAll()
        {
            _logger.LogInformation("Getting all pizzas");
            return _pizzas.ToList();
        }

        public Pizza? GetById(int id)
        {
            _logger.LogInformation("Getting pizza with id {PizzaId}", id);
            return _pizzas.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pizza> GetByMinPrice(decimal price)
        {
            Console.WriteLine("Creating query");

            return _pizzas.Where(p => {
                Console.WriteLine($"Checking {p.Name}");
                return p.Price >= price;
            });
        }

        public IEnumerable<Pizza> GetByName(string name)
        {
            return _pizzas.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
