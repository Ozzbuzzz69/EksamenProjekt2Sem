using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food.Sandwich
{
    [Authorize(Roles = "admin")]

    public class CreateSandwichModel : PageModel
    {

        private SandwichService _sandwichService;

        public CreateSandwichModel(SandwichService sandwichService)
        {
            _sandwichService = sandwichService;
        }

        [BindProperty]
        public Models.Sandwich Sandwich { get; set; }

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
            _sandwichService.CreateSandwich(Sandwich).Wait();
            return RedirectToPage("ReadAllSandwiches");
        }
    }
}