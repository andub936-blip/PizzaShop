namespace PizzaShop.ViewModels.OrderVM
{
    public class OrderDetailsItemViewModel
    {
        public int PizzaId { get; set; }
        public string PizzaName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
