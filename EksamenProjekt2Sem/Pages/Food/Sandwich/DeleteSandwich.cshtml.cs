using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Food.Sandwich
{
    [Authorize(Roles = "admin")]

    public class DeleteSandwichModel : PageModel
    {
        private SandwichService _sandwichService;

        public DeleteSandwichModel(SandwichService sandwichService)
        {
            _sandwichService = sandwichService;
        }

        [BindProperty]
        public Models.Sandwich Sandwich { get; set; }

        public IActionResult OnGet(int id)
        {
            Sandwich = _sandwichService.ReadSandwich(id);
            if (Sandwich == null)
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            Models.Sandwich deletedSandwich = _sandwichService.DeleteSandwich(Sandwich.Id);
            if (deletedSandwich == null)
            {
                return RedirectToPage("NotFound");
            }
            return RedirectToPage("ReadAllSandwiches");
        }
    }
}
