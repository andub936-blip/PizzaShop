using PizzaShop.Domain.Model;
using PizzaShop.ViewModels.PizzaVM;

namespace PizzaShop.Interfaces
{
    public interface IPizzaService
    {
        Task<PizzaListViewModel> GetPizzaListViewModelAsync();

        Task<Pizza?> GetByIdAsync(int id);

        Task<IReadOnlyList<PizzaListItemViewModel>> GetByMinPriceAsync(decimal price);

        Task<IReadOnlyList<PizzaListItemViewModel>> GetByNameAsync(string name);

        Task<IReadOnlyCollection<Pizza>> GetByIdsAsync(IEnumerable<int> pizzaIds);
    }
}
