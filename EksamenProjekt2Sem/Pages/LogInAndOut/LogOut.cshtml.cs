using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace EksamenProjekt2Sem.Pages.LogInAndOut
{
    public class LogOutPageModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {

           var claims = new List<Claim>();
           var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToPage("/index");
        }

    }
}
