using Microsoft.AspNetCore.Mvc;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;

        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet("checkout")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Create(CheckoutViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", viewModel);
            }

            var orderId = await _orderService.CreateOrderAsync(viewModel);

            return RedirectToAction("Success", new {Id = orderId });
        }

        [HttpGet("checkout/success/{id}")]
        public async Task<IActionResult> Success(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if(order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
