using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    using EksamenProjekt2Sem.Services;

    public class CreateOrderModel : PageModel
    {
        private Services.OrderService _orderService;

        public CreateOrderModel(Services.OrderService orderService)
        {
            _orderService = orderService;
        }
        [BindProperty]
        public Order Order { get; set; }
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

            // Find the user by logged in user

            // Temporary hardcoded user
            User user = new("name", "email", "phoneNumber", "password");

            // Create the order if the user is found
            if (user == null)
            {
                return Page();
            }
            Order = new Order(user, Order.PickupTime);
            _orderService.CreateOrder(Order);


            return RedirectToPage("./Index");
        }
    }
}
