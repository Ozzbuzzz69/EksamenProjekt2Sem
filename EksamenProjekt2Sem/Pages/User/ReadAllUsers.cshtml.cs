using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EksamenProjekt2Sem.Pages.User
{
    public class ReadAllUsersModel : PageModel
    {
        private Services.UserService _userService;
        public ReadAllUsersModel(Services.UserService userService)
        {
            _userService = userService;
        }
        public List<Models.User> Users { get; set; }
        [BindProperty]
        public string SearchString { get; set; }

        public async Task OnGetAsync()
        {
            // Get all users
            Users = await _userService.ReadAllUsersAsync();
            if (Users == null)
            {
                RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Handle search input
            var allUsers = await _userService.ReadAllUsersAsync();
            if (!string.IsNullOrEmpty(SearchString))
            {
                Users = allUsers
                    .Where(u => u.Name.Contains(SearchString, System.StringComparison.OrdinalIgnoreCase)
                             || u.Email.Contains(SearchString, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Users = allUsers;
            }
            return Page();
        }
    }
}