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

        public ReadAllSandwichesModel(SandwichService sandwichService)
        {
            _sandwichService = sandwichService;
        }

        public List<Models.Sandwich> Sandwiches { get; private set; }

        [BindProperty]
        public string SearchMeatType { get; set; }

        [BindProperty]
        public string SearchCategory { get; set; }


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
    }
}
