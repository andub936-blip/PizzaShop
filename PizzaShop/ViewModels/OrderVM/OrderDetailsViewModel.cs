namespace PizzaShop.ViewModels.OrderVM
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public IReadOnlyList<OrderDetailsItemViewModel> Items { get; set; } = [];
        public decimal Total { get; set; }
    }
}
