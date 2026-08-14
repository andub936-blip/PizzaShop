using PizzaShop.Domain.Model;

namespace PizzaShop.ViewModels.PizzaVM
{
    public class PizzaDetailsViewModel
    {
        public PizzaDetailsViewModel(Pizza pizza)
        {
            Pizza = pizza;
        }
        public Pizza Pizza { get; }
    }
}
