using PizzaShop.Domain.Model;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(CheckoutViewModel viewModel);

        Task<OrderDetailsViewModel?> GetByIdAsync(int id);

        Task<OrderListViewModel> GetOrderListViewModelAsync();
    }
}
