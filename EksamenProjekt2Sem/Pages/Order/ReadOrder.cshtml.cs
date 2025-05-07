using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    public class ReadOrderModel : PageModel
    {
        private Services.OrderService _orderService;
        public ReadOrderModel(Services.OrderService orderService)
        {
            _orderService = orderService;
        }
        public Order? order { get; set; }

        // Other search criteria properties can be added here:
        //

        public IActionResult OnGet()
        {
            //Find the userid of the logged in user


            // Temporary hardcoded id
            int userId = 1;

            // Get the order from the id of the user
            order = _orderService.ReadOrderByUserId(userId);
            
            if (order == null)
            {
                // Handle not found case
                return RedirectToPage("./Index");
            }
            return Page();
        }
        public void OnPost()
        {
        }
        // Sorting of orderlines can be added here:


    }
}
