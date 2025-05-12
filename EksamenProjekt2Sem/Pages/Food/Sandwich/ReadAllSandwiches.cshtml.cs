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



        //[BindProperty]
        //public List<int> AmountSandwich { get; set; }

        //public void OnGet()
        //{
        //    Sandwiches = _sandwichService.ReadAllSandwiches();
        //    AmountSandwich = new List<int>();
        //    for (int i = 0; i < Sandwiches.Count; i++)
        //    {
        //        AmountSandwich.Add(0);
        //    }
        //}

        //public IActionResult OnPostSelectAmountSandwich()
        //{
        //    Models.Order order = new Models.Order();
        //    int j = 0;
        //    foreach (int i in AmountSandwich)
        //    {
        //        // tilføj orderline med disse values:
        //        order.OrderLines[j].Quantity = i;
        //        order.OrderLines[j].Food = Sandwiches[j];
        //        j++;
        //    }
        //    // send til create order page 
        //    return Page();
        //}
    }
}
