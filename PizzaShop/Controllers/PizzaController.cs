using Microsoft.AspNetCore.Mvc;
using PizzaShop.Domain.Interfaces;

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
            ViewData["PizzaList"] = await _pizzaService.GetAllAsync();

            return View();
        }

        [HttpGet("pizzas/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var pizza = await _pizzaService.GetByIdAsync(id);

            if (pizza is null) return NotFound();

            ViewData["SelectedPizza"] = pizza;

            return View();
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

            ViewData["PizzaList"] = pizzas;

            return View("Index");
        }

        [HttpGet("pizzas/search")]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var pizzas = await _pizzaService.GetByNameAsync(name);
            ViewData["PizzaList"] = pizzas;

            return View("Index");
        }
    }
}
