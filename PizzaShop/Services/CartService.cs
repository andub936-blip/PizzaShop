using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.CartVM;
using System.Text.Json;

namespace PizzaShop.Services
{
    public class CartService : ICartService
    {
        private const string CartKey = "Cart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPizzaService _pizzaService;

        public CartService(IHttpContextAccessor httpContextAccessor, 
                            IPizzaService pizzaService)
        {
            _httpContextAccessor = httpContextAccessor;
            _pizzaService = pizzaService;
        }

        public void Add(CartItem item)
        {
            var cart = GetCart();

            var existingItem = cart.Items.FirstOrDefault(i => i.PizzaId == item.PizzaId);

            if(existingItem is null)
            {
                cart.Items.Add(item);
            }
            else
            {
                existingItem.Quantity += item.Quantity;
            }

            SaveCart(cart);
        }

        public async Task<CartViewModel> GetCartViewModelAsync()
        {
            var cart = GetCart();

            var pizzaIds = cart.Items.Select(i => i.PizzaId);
            var pizzas = await _pizzaService.GetByIdsAsync(pizzaIds);

            var cartItems = cart.Items.Join(pizzas,
                item => item.PizzaId,
                pizza => pizza.Id,
                (item, pizza) => new CartItemViewModel
                {
                    PizzaId = item.PizzaId,
                    PizzaName = pizza.Name,
                    Price = pizza.Price,
                    Quantity = item.Quantity
                })
                .ToList();

            return new CartViewModel
            {
                Items = cartItems
            };
        }

        public void UpdateQuantity(int pizzaId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);

            if(item is null)
            {
                return;
            }

            item.Quantity = quantity;
            SaveCart(cart);
        }

        public void Remove(int pizzaId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.PizzaId == pizzaId);

            if (item is null)
            {
                return;
            }

            cart.Items.Remove(item);
            SaveCart(cart);
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            var json = session.GetString(CartKey);

            if (json is null) return new Cart();

            return JsonSerializer.Deserialize<Cart>(json) ?? new Cart();
        }

        private void SaveCart(Cart cart)
        {
            var session = _httpContextAccessor.HttpContext!.Session;
            session.SetString(CartKey, JsonSerializer.Serialize(cart));
        }
    }
}
