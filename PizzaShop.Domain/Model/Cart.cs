namespace PizzaShop.Domain.Model
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();
    }
}
