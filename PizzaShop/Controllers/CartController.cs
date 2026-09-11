using Microsoft.AspNetCore.Mvc;
using PizzaShop.ViewModels.CartVM;
using PizzaShop.Domain.Model;
using PizzaShop.Interfaces;
using PizzaShop.Domain.Enums;

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
        public async Task<IActionResult> Add([FromForm] AddToCartViewModel viewModel)
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
            var result = await _cartService.AddAsync(item);

            switch (result)
            {
                case AddToCartResult.PizzaNotFound:
                    TempData["Error"] = $"This pizza is no longer available.";
                    return RedirectToAction("Index", "Pizza");
                case AddToCartResult.QuantityLimitExceeded:
                    TempData["Error"] = $"You cannot have more than 20 of the same pizza.";
                    return RedirectToAction("Index", "Pizza");
                case AddToCartResult.Success:
                    TempData["Success"] = "Pizza added to cart.";
                    return RedirectToAction("Index", "Cart");
                default:
                    throw new InvalidOperationException("Unknown add-to-cart result.");
            }
        }

        [HttpPost("cart/update")]
        public IActionResult Update([FromForm] UpdateCartItemViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _cartService.UpdateQuantity(viewModel.PizzaId, 
                                        viewModel.Quantity);

            return RedirectToAction("Index");
        }

        [HttpPost("cart/remove")]
        public IActionResult Remove(int pizzaId)
        {
            _cartService.Remove(pizzaId);

            return RedirectToAction("Index");
        }
    }
}
