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
        public IActionResult Index()
        {
            ViewData["PizzaList"] = _pizzaService.GetAll();

            return View();
        }

        [HttpGet("pizzas/{id}")]
        public IActionResult Details(int id)
        {
            var pizza = _pizzaService.GetById(id);

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
        public IActionResult GetByMinPrice(decimal price)
        {
            var pizzas = _pizzaService.GetByMinPrice(price);
            Console.WriteLine("Service returned");
            foreach(var p in pizzas)
            {
                Console.WriteLine(p.Name);
            }

            //ViewData["PizzaList"] = pizzas.ToList();

            //return View("Index");

            return Ok();  // возвращаю временный результат, чтобы провести эксперимент, в будущем будет вывод списка пицц, удовлетворяющих условию
        }

        [HttpGet("pizzas/search")]
        public IActionResult Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }

            var pizzas = _pizzaService.GetByName(name);
            ViewData["PizzaList"] = pizzas.ToList();

            return View("Index");
        }
    }
}
