namespace PizzaShop.ViewModels.CartVM
{
    public class CartViewModel
    {
        public IReadOnlyCollection<CartItemViewModel> Items { get; set; } 
            = new List<CartItemViewModel>();

        public decimal Total => Items.Sum(i => i.Total);
    }
}
