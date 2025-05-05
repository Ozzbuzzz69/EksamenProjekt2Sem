using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EksamenProjekt2Sem.Pages.LogInAndOut
{
    public class LogInModel : PageModel
    {
        private UserService _userService;

        public LogInModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }

        public void OnGet()
        {
        }
    }
}
