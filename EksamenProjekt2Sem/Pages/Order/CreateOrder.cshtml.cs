using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.Order
{
    using EksamenProjekt2Sem.Models;
    using EksamenProjekt2Sem.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.IdentityModel.Tokens;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderModel : PageModel
    {
        private OrderService _orderService;
        private UserService _userService;
        private SandwichService _sandwichService;
        private WarmMealService _warmMealService;
        private CampaignOfferService _campaignOfferService;

        public CreateOrderModel(OrderService orderService, UserService userService, SandwichService sandwichService, WarmMealService warmMealService, CampaignOfferService campaignOfferService)
        {
            _orderService = orderService;
            _userService = userService;
            _sandwichService = sandwichService;
            _warmMealService = warmMealService;
            _campaignOfferService = campaignOfferService;
        }

        [BindProperty]
        public DateTime PickupTime { get; set; } = DateTime.Now.Date.AddDays(1);

        public Order Order { get; set; } = new Order();

        public User User { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public Sandwich Sandwich { get; set; }

        public WarmMeal WarmMeal { get; set; }

        public CampaignOffer CampaignOffer { get; set; }

        public void OnGet(int id, string type)
        {
            if (type == "Sandwich") Sandwich = _sandwichService.ReadSandwich(id);
            if (type == "WarmMeal") WarmMeal = _warmMealService.ReadWarmMeal(id);
            if (type == "CampaignOffer") CampaignOffer = _campaignOfferService.ReadCampaignOffer(id);

            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
        }

        public IActionResult OnPostOrderSandwich(int id)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Sandwich = _sandwichService.ReadSandwich(id);
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            Order.UserId = User.Id;
            Order.FoodId = Sandwich.Id;
            Order.Quantity = Quantity;
            Order.PickupTime = PickupTime;

            _orderService.CreateOrder(Order).Wait();

            return RedirectToPage("ReadAllOrders");
        }

        public IActionResult OnPostOrderWarmMeal(int id)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return Page();
            }

            WarmMeal = _warmMealService.ReadWarmMeal(id);
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            Order.UserId = User.Id;
            Order.FoodId = WarmMeal.Id;
            Order.Quantity = Quantity;
            Order.PickupTime = PickupTime;

            _orderService.CreateOrder(Order).Wait();

            return RedirectToPage("ReadAllOrders");
        }

        public IActionResult OnPostOrderCampaignOffer(int id)
        {
            // Validate the input
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CampaignOffer = _campaignOfferService.ReadCampaignOffer(id);
            User = _userService.GetUserByEmail(HttpContext.User.Identity.Name);
            Order.UserId = User.Id;
            Order.CampaignOfferId = CampaignOffer.Id;
            Order.Quantity = Quantity;
            Order.PickupTime = PickupTime;

            _orderService.CreateOrder(Order).Wait();

            return RedirectToPage("ReadAllOrders");
        }
    }
}
