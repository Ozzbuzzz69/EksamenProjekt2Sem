using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;

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

        [BindProperty]
        public Models.Order Cart => _orderService.ReadCart();

        [BindProperty]
        public Models.Sandwich Sandwich { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public OrderLine? OrderLine { get; set; }



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
        
        public IActionResult OnPostAddFoodToCart()
        {
            Sandwich = _sandwichService.ReadSandwich(Id);

            _orderService.AddFoodToCart(Sandwich, Quantity);

            Sandwiches = _sandwichService.ReadAllSandwiches();

            return Page();
        }

        public IActionResult OnPostDeleteOrderLine(int orderLineFoodId, int quantity)
        {
            OrderLine = _orderService.ReadOrderLine(orderLineFoodId, quantity);

            if (OrderLine != null)
            {
                _orderService.DeleteOrderLine(OrderLine);
            }

            Sandwiches = _sandwichService.ReadAllSandwiches();

            return Page();
        }
    }
}
