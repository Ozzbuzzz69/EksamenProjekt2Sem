using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Pages.Food.Sandwich
{
    public class ReadAllSandwichesModel : PageModel
    {
        private SandwichService _sandwichService;
        private OrderService _orderService;

        public ReadAllSandwichesModel(SandwichService sandwichService, OrderService orderService)
        {
            _sandwichService = sandwichService;
            _orderService = orderService;
        }

        public List<Models.Sandwich> Sandwiches { get; private set; }

        [BindProperty]
        public string SearchMeatType { get; set; }

        [BindProperty]
        public string SearchCategory { get; set; }

        public static Models.Order Cart = new Models.Order(); //{ OrderLines = new List<OrderLine>() };

        public Models.Order Order => Cart;
 
        [BindProperty]
        public Models.Sandwich Sandwich { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public void OnGet()
        {
            Sandwiches = _sandwichService.ReadAllSandwiches();
        }

        public IActionResult OnPostSearchMeatType()
        {
            Sandwiches = _sandwichService.SearchSandwichByMeatType(SearchMeatType).ToList();
            return Page();
        }

        public IActionResult OnPostSearchCategory()
        {
            Sandwiches = _sandwichService.SearchSandwichByCategory(SearchCategory).ToList();
            return Page();
        }

        public IActionResult OnPostAddSandwichToCart()
        {
            Sandwich = _sandwichService.ReadSandwich(Id);
            
            if (Sandwich != null && Quantity > 0 && Quantity <= 50)
            {
                Cart.OrderLines.Add(new OrderLine
                {
                    Quantity = Quantity,
                    Food = Sandwich
                });
            }
            
            Sandwiches = _sandwichService.ReadAllSandwiches();

            return Page();
        }
    }
}
