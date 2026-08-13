using PizzaShop.Domain.Model;

namespace PizzaShop.Domain.Interfaces
{
    public interface IPizzaService
    {
        Task<IReadOnlyCollection<Pizza>> GetAllAsync();

        Task<Pizza?> GetByIdAsync(int id);

        Task<IEnumerable<Pizza>> GetByMinPriceAsync(decimal price);

        Task<IEnumerable<Pizza>> GetByNameAsync(string name);
    }
}
