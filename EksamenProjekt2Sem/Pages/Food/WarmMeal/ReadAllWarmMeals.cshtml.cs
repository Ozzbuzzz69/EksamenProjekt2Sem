using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food.WarmMeal
{
    public class ReadAllWarmMealsModel : PageModel
    {
        private WarmMealService _warmMealService;
        [BindProperty]
        public int Count { get; set; } = 0;

        public ReadAllWarmMealsModel(WarmMealService warmMealService)
        {
            _warmMealService = warmMealService;
        }

        public List<Models.WarmMeal>? WarmMeals { get; private set; }

        public void OnGet()
        {
            WarmMeals = _warmMealService.ReadAllWarmMeals();
        }
    }
}
