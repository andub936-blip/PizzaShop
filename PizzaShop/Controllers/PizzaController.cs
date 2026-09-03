using Microsoft.AspNetCore.Mvc;
using PizzaShop.Interfaces;
using PizzaShop.ViewModels.PizzaVM;

namespace PizzaShop.Controllers
{
    public class PizzaController : Controller
    {
        private readonly IPizzaService _pizzaService;

        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet("pizzas")]
        public async Task<IActionResult> Index()
        {
            var viewModel = await _pizzaService.GetPizzaListViewModelAsync();

            return View(viewModel);
        }

        [HttpGet("pizzas/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var pizza = await _pizzaService.GetByIdAsync(id);

            if (pizza is null) return NotFound();

            var viewModel = new PizzaDetailsViewModel(pizza);

            return View(viewModel);
        }

        [HttpGet("pizzas/price")]
        public async Task<IActionResult> GetByMinPrice(decimal price)
        {
            var items = await _pizzaService.GetByMinPriceAsync(price);

            var viewModel = new PizzaListViewModel
            {
                Items = items 
            };

            return View("Index", viewModel);
        }

        [HttpGet("pizzas/search")]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var items = await _pizzaService.GetByNameAsync(name);
            var viewModel = new PizzaListViewModel
            {
                Items = items
            };

            return View("Index", viewModel);
        }
    }
}
