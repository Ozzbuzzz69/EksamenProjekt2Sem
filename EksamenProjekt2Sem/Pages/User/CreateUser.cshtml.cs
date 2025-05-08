using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.User
{
    public class CreateUserModel : PageModel
    {
        private Services.UserService _userService;
        private PasswordHasher<string> passwordHasher;

        public CreateUserModel(Services.UserService userService)
        {
            _userService = userService;
            passwordHasher = new PasswordHasher<string>();

        }
        [BindProperty]
        public Models.User User { get; set; }
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
            _userService.CreateUser(new Models.User(User.Name, User.Email, User.PhoneNumber, passwordHasher.HashPassword(null, User.Password)));
            return RedirectToPage("/Index");
        }
    }
}
