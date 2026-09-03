namespace PizzaShop.ViewModels.OrderVM
{
    public class OrderListViewModel
    {
        public IReadOnlyList<OrderListItemViewModel> Items { get; set; } = [];
    }
}
