using Microsoft.AspNetCore.Mvc;
using PizzaShop.ViewModels.CartVM;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;

namespace PizzaShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _cartService.GetCartViewModelAsync();

            return View(viewModel);
        }

        [HttpPost("cart/add")]
        public IActionResult Add([FromForm] AddToCartViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new CartItem
            {
                PizzaId = viewModel.PizzaId,
                Quantity = viewModel.Quantity
            };
            _cartService.Add(item);

            return RedirectToAction("Index", "Cart");
        }
    }
}
