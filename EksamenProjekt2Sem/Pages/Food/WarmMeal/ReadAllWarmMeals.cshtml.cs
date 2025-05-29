using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food.WarmMeal
{
    public class ReadAllWarmMealsModel : PageModel
    {
        private WarmMealService _warmMealService;

        public ReadAllWarmMealsModel(WarmMealService warmMealService)
        {
            _warmMealService = warmMealService;
        }

        public List<Models.WarmMeal> WarmMeals { get; private set; }

        [BindProperty]
        public string SearchMeatType { get; set; }

        public void OnGet()
        {
            WarmMeals = _warmMealService.ReadAllWarmMeals();
        }

        public IActionResult OnPostSearchMeatType()
        {
            WarmMeals = _warmMealService.SearchWarmMealByMeatType(SearchMeatType).ToList();
            return Page();
        }
    }
}