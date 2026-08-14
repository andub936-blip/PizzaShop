using PizzaShop.Domain.Model;

namespace PizzaShop.Domain.Interfaces
{
    public interface IPizzaService
    {
        Task<IReadOnlyCollection<Pizza>> GetAllAsync();

        Task<Pizza?> GetByIdAsync(int id);

        Task<IReadOnlyCollection<Pizza>> GetByMinPriceAsync(decimal price);

        Task<IReadOnlyCollection<Pizza>> GetByNameAsync(string name);
    }
}
