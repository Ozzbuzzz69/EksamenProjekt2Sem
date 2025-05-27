using EksamenProjekt2Sem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        [BindProperty]
        public Models.User User { get; set; }
        public string Message { get; set; } = string.Empty;
        public CreateUserModel(Services.UserService userService)
        {
            _userService = userService;
            passwordHasher = new PasswordHasher<string>();

        }
        
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

            // Prevent registration as admin
            if (User.Email.ToLower() == "admin@admin.com")
            {
                ModelState.AddModelError(string.Empty, "You cannot register as the admin user.");
                return Page();
            }

            var existingUser = await _userService.GetUserByEmailAsync(User.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return Page();
            }

            // Hash the password before saving
            var passwordHasher = new PasswordHasher<string>();
            User.Password = passwordHasher.HashPassword(null, User.Password);
            await _userService.CreateUser(User);


            // Build claims
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, User.Email) };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            return RedirectToPage("/Index");
        }
    }
}
