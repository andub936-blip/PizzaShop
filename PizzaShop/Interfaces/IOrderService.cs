using PizzaShop.Domain.Model;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CheckoutViewModel viewModel);

        Task<Order?> GetByIdAsync(int id);
    }
}
