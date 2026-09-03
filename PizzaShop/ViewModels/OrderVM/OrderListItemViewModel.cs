namespace PizzaShop.ViewModels.OrderVM
{
    public class OrderListItemViewModel
    {
        public int OrderId { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Total { get; set; }
    }
}
