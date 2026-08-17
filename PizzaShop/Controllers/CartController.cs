using Microsoft.AspNetCore.Mvc;
using PizzaShop.Domain.Interfaces;
using PizzaShop.ViewModels.CartVM;
using PizzaShop.Domain.Model;

namespace PizzaShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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

            return Ok(viewModel);
        }
    }
}
