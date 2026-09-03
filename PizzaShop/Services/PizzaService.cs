using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.PizzaVM;

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

        public async Task<PizzaListViewModel> GetPizzaListViewModelAsync()
        {
            _logger.LogInformation("Getting all pizzas");

            var items = await _dbContext.Pizzas
                .Select(p => new PizzaListItemViewModel
                {
                    PizzaId = p.Id,
                    PizzaName = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl= p.ImageUrl
                })
                .ToListAsync();

            return new PizzaListViewModel
            {
                Items = items
            };
        }

        public async Task<Pizza?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Getting pizza with id {PizzaId}", id);

            return await _dbContext.Pizzas
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<PizzaListItemViewModel>> GetByMinPriceAsync(decimal price)
        {
            return await _dbContext.Pizzas
                .Where(p => p.Price >= price)
                .Select(p => new PizzaListItemViewModel
                {
                    PizzaId = p.Id,
                    PizzaName = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<PizzaListItemViewModel>> GetByNameAsync(string name)
        {
            return await _dbContext.Pizzas
                .Where(p => EF.Functions.Like(p.Name, $"{name}%"))
                .Select(p => new PizzaListItemViewModel
                {
                    PizzaId = p.Id,
                    PizzaName = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                })
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
