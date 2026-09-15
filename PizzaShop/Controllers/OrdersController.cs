using Microsoft.AspNetCore.Mvc;
using PizzaShop.Interfaces;

namespace PizzaShop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("orders")]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _orderService.GetOrderListViewModelAsync();

            return View(viewModel);
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> Details([FromRoute] int id)
        {
            var viewModel = await _orderService.GetByIdAsync(id);

            if(viewModel is null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
