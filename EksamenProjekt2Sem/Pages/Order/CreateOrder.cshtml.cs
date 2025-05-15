using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    using EksamenProjekt2Sem.Services;

    public class CreateOrderModel : PageModel
    {
        private OrderService _orderService;

        public CreateOrderModel(OrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public DateTime PickupTime { get; set; }

        [BindProperty]
        public Order Cart => _orderService.ReadCart();

        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Temporary hardcoded user
            User user = new("name", "email", "phoneNumber", "password");

            Cart.User = user;

            Order order = new Order(user, PickupTime);
            order.OrderLines.AddRange(Cart.OrderLines);

            _orderService.CreateOrder(order);

            return RedirectToPage("/Food/Sandwich/ReadAllSandwiches");
        }
    }
}
