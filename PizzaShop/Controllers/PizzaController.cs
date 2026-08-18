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
            var pizzas = await _pizzaService.GetAllAsync();
            var viewModel = new PizzaListViewModel(pizzas);

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

        [HttpGet("pizzas/status")]
        public IActionResult Status()
        {
            return Ok("Status OK");
        }

        [HttpGet("pizzas/error")]
        public IActionResult ErrorTest()
        {
            throw new Exception("Test exception");
        }

        [HttpGet("pizzas/price")]
        public async Task<IActionResult> GetByMinPrice(decimal price)
        {
            var pizzas = await _pizzaService.GetByMinPriceAsync(price);

            var viewModel = new PizzaListViewModel(pizzas);

            return View("Index", viewModel);
        }

        [HttpGet("pizzas/search")]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var pizzas = await _pizzaService.GetByNameAsync(name);
            var viewModel = new PizzaListViewModel(pizzas);

            return View("Index", viewModel);
        }
    }
}
