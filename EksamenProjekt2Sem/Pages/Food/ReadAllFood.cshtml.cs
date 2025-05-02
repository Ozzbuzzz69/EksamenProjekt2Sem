using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food
{
    public class ReadAllFoodModel : PageModel
    {
        private Services.FoodService _foodService;
        public ReadAllFoodModel(Services.FoodService foodService)
        {
            _foodService = foodService;
        }
        public List<Models.Food> Foods { get; set; }
        [BindProperty]
        public string SearchString { get; set; }
        //Other search criteria properties can be added here:
        //
        public void OnGet()
        {
            // Get all foods
            Foods = _foodService.ReadAllFood();
            if (Foods == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }
        }
        //Other onget methods such as sorting order can be added here:
        //

        public IActionResult OnPost()
        {
            // Handle search input
            if (!string.IsNullOrEmpty(SearchString))
            {
                Foods = _foodService.ReadAllFood().ToList();
            }
            return Page();
        }
        //Other onpost methods such as filtering can be added here:
        //

    }
}
