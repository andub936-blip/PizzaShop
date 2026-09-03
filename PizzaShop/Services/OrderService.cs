using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartService _cartService;
        private readonly IPizzaService _pizzaService;
        private readonly PizzaShopDbContext _dbContext;

        public OrderService(ICartService cartService,
                            IPizzaService pizzaService,
                            PizzaShopDbContext dbContext)
        {
            _cartService = cartService;
            _pizzaService = pizzaService;
            _dbContext = dbContext;
        }
        public async Task<int> CreateOrderAsync(CheckoutViewModel viewModel)
        {
            var cart = _cartService.GetCart();

            if (!cart.Items.Any())
            {
                throw new InvalidOperationException("Cannot create an order from an empty cart.");
            }

            if (cart.Items.Any(i => i.Quantity < 1 || i.Quantity > 20))
            {
                throw new InvalidOperationException("Cart contains an invalid quantity.");
            }

            var pizzaIds = cart.Items
                .Select(i => i.PizzaId)
                .Distinct()
                .ToList();

            var pizzas = await _pizzaService.GetByIdsAsync(pizzaIds);

            if (pizzas.Count != pizzaIds.Count)
            {
                throw new InvalidOperationException("Some pizzas in the cart no longer exist.");
            }

            var orderItems = cart.Items.Join(pizzas,
                cartItem => cartItem.PizzaId,
                pizza => pizza.Id,
                (cartItem, pizza) => new OrderItem
                {
                    PizzaId = cartItem.PizzaId,
                    PizzaName = pizza.Name,
                    Price = pizza.Price,
                    Quantity = cartItem.Quantity
                });

            var order = new Order
            {
                CustomerName = viewModel.CustomerName,
                Address = viewModel.Address,
                CreatedAt = DateTime.UtcNow,
                Total = orderItems.Sum(i => i.Total),
                Items = orderItems.ToList()
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            _cartService.Clear();

            return order.Id;
        }

        public async Task<OrderDetailsViewModel?> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                return null;
            }

            var viewModel = new OrderDetailsViewModel
            {
                OrderId = order.Id,
                CreatedAt = order.CreatedAt,
                CustomerName = order.CustomerName,
                Address = order.Address,
                Total = order.Total,
                Items = order.Items.Select(
                    i => new OrderDetailsItemViewModel
                    {
                        PizzaId = i.PizzaId,
                        PizzaName = i.PizzaName,
                        Price = i.Price,
                        Quantity = i.Quantity,
                        Total = i.Total,
                    })
                .ToList()
            };

            return viewModel;
        }

        public async Task<OrderListViewModel> GetOrderListViewModelAsync()
        {
            var items =  await _dbContext.Orders
                .Select(
                i => new OrderListItemViewModel
                {
                    OrderId = i.Id,
                    CreatedAt = i.CreatedAt,
                    Total = i.Total
                })
                .OrderByDescending(o => o.CreatedAt)
                .AsNoTracking()
                .ToListAsync();

            return new OrderListViewModel
            {
                Items = items
            };
        }
    }
}
