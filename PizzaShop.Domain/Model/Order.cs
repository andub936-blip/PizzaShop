namespace PizzaShop.Domain.Model
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}
