using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    public class ReadAllOrdersModel : PageModel
    {
        public UserService UserService { get; set; }
        private OrderService _orderService;
        public ReadAllOrdersModel(OrderService orderService, UserService userService)
        {
            _orderService = orderService;
            UserService = userService;
        }
        public List<Order> Orders { get; set; }
        [BindProperty]
        public string SearchString { get; set; }
        // Other search criteria properties can be added here:
        //

        public void OnGet()
        {
            // Get all orders
            Orders = _orderService.ReadAllOrders();
            if (Orders == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }
        }
        /// <summary>
        /// Search for ingredients in the order's lines
        /// </summary>
        /// <returns>Updated list which fits the searchstring</returns>
        public IActionResult OnPost()
        {
            // Handle search input
            if (!string.IsNullOrEmpty(SearchString))
            {
                List<Order> temp = new();
                foreach (Order o in Orders)
                {
                    foreach (OrderLine ol in o.OrderLines)
                    {
                        if (ol.Food.Ingredients.ToLower().Contains(SearchString.ToLower()))
                        {
                            temp.Add(o);
                            break;
                        }
                    }
                }
                Orders = temp;
            }
            return Page();
        }
        // Other onpost methods such as filtering can be added here:
        //

    }
}
