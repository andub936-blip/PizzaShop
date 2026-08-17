using PizzaShop.Domain.Model;

namespace PizzaShop.Domain.Interfaces
{
    public interface ICartService
    {
        void Add(CartItem item);

        Cart GetCart();
    }
}
