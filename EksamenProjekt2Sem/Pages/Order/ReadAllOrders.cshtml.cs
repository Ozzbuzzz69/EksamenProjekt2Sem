using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    public class ReadAllOrdersModel : PageModel
    {
        private UserService _userService { get; set; }
        private OrderService _orderService;
        public ReadAllOrdersModel(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }
        [BindProperty]
        public List<Order>? Orders { get; set; }
        [BindProperty]
        public string? SearchString { get; set; }
        // Other search criteria properties can be added here:
        //

        public IActionResult OnGet(int id)
        {
            Orders = _orderService.ReadAllOrdersByUser(_userService.GetUserByEmail(HttpContext.User.Identity.Name));
            return Page();
        }
    }
}
