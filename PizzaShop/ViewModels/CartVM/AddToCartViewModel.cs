using System.ComponentModel.DataAnnotations;

namespace PizzaShop.ViewModels.CartVM
{
    public class AddToCartViewModel
    {
        public int PizzaId { get; set; }

        [Range(1,20)]
        public int Quantity { get; set; }
    }
}
