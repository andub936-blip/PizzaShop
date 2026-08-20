using PizzaShop.Domain.Model;
using PizzaShop.ViewModels.CartVM;

namespace PizzaShop.Interfaces
{
    public interface ICartService
    {
        void Add(CartItem item);

        Cart GetCart();

        Task<CartViewModel> GetCartViewModelAsync();

        void UpdateQuantity(int pizzaId, int quantity);

        void Remove(int pizzaId);

        void Clear();
    }
}
