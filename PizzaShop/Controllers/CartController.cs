using Microsoft.AspNetCore.Mvc;
using PizzaShop.ViewModels.CartVM;

namespace PizzaShop.Controllers
{
    public class CartController : Controller
    {
        [HttpPost("cart/add")]
        public IActionResult Add([FromForm] AddToCartViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            return Ok(viewModel);
        }
    }
}
