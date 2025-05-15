using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

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
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create the user
            await _userService.CreateUser(new Models.User(User.Name, User.Email, User.PhoneNumber, passwordHasher.HashPassword(null, User.Password)));

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, User.Email),
        };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return RedirectToPage("/Index");
        }
    }
}
