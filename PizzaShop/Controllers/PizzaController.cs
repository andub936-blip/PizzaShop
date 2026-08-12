using Microsoft.AspNetCore.Mvc;
using PizzaShop.Domain.Model;

namespace PizzaShop.Controllers
{
    public class PizzaController : Controller
    {
        private List<Pizza> _pizzas = new()
        {
            new Pizza { Id = 1, Name = "Margherita", Price = 15.12m, Description = "Pizza with tomatoes, mozzarella, basil, extra virgin olive oil" },
            new Pizza { Id = 2, Name = "Pepperoni", Price = 12.99m, Description = "Pizza with pepperoni and mozzarella" },
            new Pizza { Id = 3, Name = "Four Cheese", Price = 15.12m, Description = "Pizza with Mozzarella, Gorgonzola, Fontina, Parmesan" }
        };
        [HttpGet("pizzas")]
        public IActionResult Index()
        {
            ViewData["PizzaList"] = _pizzas;

            return View();
        }

        [HttpGet("pizzas/{id}")]
        public IActionResult Details(int id)
        {
            var pizza = _pizzas.FirstOrDefault(p => p.Id == id);

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
    }
}
