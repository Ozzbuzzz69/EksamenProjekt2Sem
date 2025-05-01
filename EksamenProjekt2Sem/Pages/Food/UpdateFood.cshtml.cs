using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food
{
    public class UpdateFoodModel : PageModel
    {
        private Services.FoodService _foodService;
        public UpdateFoodModel(Services.FoodService foodService)
        {
            _foodService = foodService;
        }
        [BindProperty]
        public Models.Food Food { get; set; }
        public IActionResult OnGet()
        {
            // Get the food by id
            Food = _foodService.ReadFood(Food.Id);
            if (Food == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _foodService.UpdateFood(Food.Id, Food);
            return RedirectToPage("./Index"); // Redirect to the index page after updating
        }
    }
}
