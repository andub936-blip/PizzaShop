using PizzaShop.Domain.Interfaces;
using PizzaShop.Domain.Model;
using System.Text.Json;

namespace PizzaShop.Services
{
    public class CartService : ICartService
    {
        private const string CartKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Add(CartItem item)
        {
            var session = _httpContextAccessor.HttpContext!.Session;

            var json = session.GetString(CartKey);

            var cart = json is null
                ? new Cart()
                : JsonSerializer.Deserialize<Cart>(json) ?? new Cart();

            var existingItem = cart.Items.FirstOrDefault(i => i.PizzaId == item.PizzaId);

            if(existingItem is null)
            {
                cart.Items.Add(item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
            }
            
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext!.Session;

            var json = session.GetString(CartKey);

            if (json is null) return new Cart();

            return JsonSerializer.Deserialize<Cart>(json) ?? new Cart();
        }
    }
}
