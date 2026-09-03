using PizzaShop.Domain.Model;

namespace PizzaShop.ViewModels.PizzaVM
{
    public class PizzaDetailsViewModel
    {
        public PizzaDetailsViewModel(Pizza pizza)
        {
            PizzaId = pizza.Id;
            PizzaName = pizza.Name;
            Price = pizza.Price;
            Description = pizza.Description;
            ImageUrl = pizza.ImageUrl;
        }

        public int PizzaId { get; set; }

        public string PizzaName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
