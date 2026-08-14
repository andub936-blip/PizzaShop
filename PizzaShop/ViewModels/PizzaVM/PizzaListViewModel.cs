using PizzaShop.Domain.Model;

namespace PizzaShop.ViewModels.PizzaVM
{
    public class PizzaListViewModel
    {
        public PizzaListViewModel(IReadOnlyCollection<Pizza> pizzas)
        {
            Pizzas = pizzas;
        }
        public IReadOnlyCollection<Pizza> Pizzas { get; }
    }
}
