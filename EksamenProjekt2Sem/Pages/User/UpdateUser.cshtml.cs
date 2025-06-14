using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.User
{
    public class UpdateUserModel : PageModel
    {
        private Services.UserService _userService;
        public UpdateUserModel(Services.UserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public Models.User User { get; set; }
        public IActionResult OnGet()
        {
            // Get the user by id
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            if (User == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid model state
                return Page();
            }
            _userService.UpdateUser(User);
            return RedirectToPage("./Index"); // Redirect to the index page after updating
        }
    }
}