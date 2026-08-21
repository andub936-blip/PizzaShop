using Microsoft.AspNetCore.Mvc;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.OrderVM;

namespace PizzaShop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public CheckoutController(IOrderService orderService,
                                  ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }
        [HttpGet("checkout")]
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

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
