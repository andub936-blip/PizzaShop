using System.ComponentModel.DataAnnotations;

namespace PizzaShop.ViewModels.OrderVM
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your address.")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; } = string.Empty;
    }
}
