using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;

namespace PizzaShop.Services
{
    public class PizzaService : IPizzaService
    {
        private readonly ILogger<PizzaService> _logger;
        private readonly PizzaShopDbContext _dbContext;

        public PizzaService(ILogger<PizzaService> logger, PizzaShopDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<Pizza>> GetAllAsync()
        {
            _logger.LogInformation("Getting all pizzas");

            return await _dbContext.Pizzas
                .ToListAsync();
        }

        public async Task<Pizza?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting pizza with id {PizzaId}", id);

            return await _dbContext.Pizzas
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyCollection<Pizza>> GetByMinPriceAsync(decimal price)
        {
            return await _dbContext.Pizzas
                .Where(p => p.Price >= price)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Pizza>> GetByNameAsync(string name)
        {
            return await _dbContext.Pizzas
                .Where(p => EF.Functions.Like(p.Name, $"{name}%"))
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Pizza>> GetByIdsAsync(IEnumerable<int> pizzaIds)
        {
            return await _dbContext.Pizzas
                .Where(p => pizzaIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
