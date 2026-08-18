namespace PizzaShop.ViewModels.CartVM
{
    public class CartItemViewModel
    {
        public int PizzaId { get; set; }

        public string PizzaName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}
