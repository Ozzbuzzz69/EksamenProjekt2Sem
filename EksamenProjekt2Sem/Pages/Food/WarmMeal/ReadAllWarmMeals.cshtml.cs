using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food.WarmMeal
{
    public class ReadAllWarmMealsModel : PageModel
    {
        private WarmMealService _warmMealService;
        private OrderService _orderService;

        public ReadAllWarmMealsModel(WarmMealService warmMealService, OrderService orderService)
        {
            _warmMealService = warmMealService;
            _orderService = orderService;
        }

        public List<Models.WarmMeal> WarmMeals { get; private set; }

        [BindProperty]
        public string SearchMeatType { get; set; }

        [BindProperty]
        public Models.Order Cart => _orderService.ReadCart();

        [BindProperty]
        public Models.WarmMeal WarmMeal { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public OrderLine OrderLine { get; set; }



        public void OnGet()
        {
            WarmMeals = _warmMealService.ReadAllWarmMeals();
        }

        public IActionResult OnPostSearchMeatType()
        {
            WarmMeals = _warmMealService.SearchWarmMealByMeatType(SearchMeatType).ToList();
            return Page();
        }

        //public IActionResult OnPostAddFoodToCart()
        //{
        //    WarmMeal = _warmMealService.ReadWarmMeal(Id);

        //    _orderService.AddFoodToCart(WarmMeal, Quantity);

        //    WarmMeals = _warmMealService.ReadAllWarmMeals();

        //    return Page();
        //}

        public IActionResult OnPostDeleteOrderLine(int orderLineFoodId, int quantity)
        {
            OrderLine = _orderService.ReadOrderLine(orderLineFoodId, quantity);

            if (OrderLine != null)
            {
                _orderService.DeleteOrderLine(OrderLine);
            }

            WarmMeals = _warmMealService.ReadAllWarmMeals();

            return Page();
        }
    }
}
