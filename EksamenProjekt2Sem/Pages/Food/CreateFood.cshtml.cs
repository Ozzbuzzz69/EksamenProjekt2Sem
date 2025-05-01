using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food
{
    public class CreateFoodModel : PageModel
    {
        private Services.FoodService _foodService;
        public CreateFoodModel(Services.FoodService foodService)
        {
            _foodService = foodService;
        }
        [BindProperty]
        public Models.Food Food { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _foodService.CreateFood(Food);
            return RedirectToPage("./Index");
        }
    }
}
