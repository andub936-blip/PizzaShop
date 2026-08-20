using System.ComponentModel.DataAnnotations;

namespace PizzaShop.ViewModels.OrderVM
{
    public class CheckoutViewModel
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
