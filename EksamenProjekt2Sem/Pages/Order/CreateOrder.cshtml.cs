using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    using EksamenProjekt2Sem.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.IdentityModel.Tokens;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderModel : PageModel
    {
        private OrderService _orderService;
        private UserService _userService;

        public CreateOrderModel(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [BindProperty]
        public DateTime PickupTime { get; set; } = DateTime.Now.Date.AddDays(1);

        public Order Cart { get; set; }

        public User User { get; set; }

        public void OnGet()
        {
            Cart = _orderService.ReadCart();
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            Cart.User = User;
        }

        public IActionResult OnPost()
        {
            Cart = _orderService.ReadCart();
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            Cart.User = User;

            // Validate the input
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order order = new Order(User, PickupTime);

            foreach (var orderline in Cart.OrderLines)
            {
                order.OrderLines.Add(orderline);
            }

            _orderService.CreateOrder(order);

            return RedirectToPage("ReadAllOrders");
        }
    }
}
