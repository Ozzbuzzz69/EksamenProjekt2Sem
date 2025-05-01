using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food
{
    public class DeleteFoodModel : PageModel
    {
        private Services.FoodService _foodService;
        public DeleteFoodModel(Services.FoodService foodService)
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
            Models.Food deletedFood = _foodService.DeleteFood(Food.Id);
            if (deletedFood == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }

            return RedirectToPage("./Index");
        }
    }
}
