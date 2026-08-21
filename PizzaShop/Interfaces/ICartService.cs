using PizzaShop.Domain.Enums;
using PizzaShop.Domain.Model;
using PizzaShop.ViewModels.CartVM;

namespace PizzaShop.Interfaces
{
    public interface ICartService
    {
        Task<AddToCartResult> AddAsync(CartItem item);

        Cart GetCart();

        Task<CartViewModel> GetCartViewModelAsync();

        void UpdateQuantity(int pizzaId, int quantity);

        void Remove(int pizzaId);

        void Clear();
    }
}
