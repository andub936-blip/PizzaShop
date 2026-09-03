namespace PizzaShop.ViewModels.PizzaVM
{
    public class PizzaListItemViewModel
    {
        public int PizzaId { get; set; }

        public string PizzaName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
    }
}
