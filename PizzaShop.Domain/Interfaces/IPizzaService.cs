using PizzaShop.Domain.Model;

namespace PizzaShop.Domain.Interfaces
{
    public interface IPizzaService
    {
        IReadOnlyCollection<Pizza> GetAll();

        Pizza? GetById(int id);

        IEnumerable<Pizza> GetByMinPrice(decimal price);

        IEnumerable<Pizza> GetByName(string name);
    }
}
