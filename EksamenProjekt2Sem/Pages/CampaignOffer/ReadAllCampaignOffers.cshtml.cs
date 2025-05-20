using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.CampaignOffer
{
    public class ReadAllCampaignOffersModel : PageModel
    {
        private CampaignOfferService _campaignOfferService;
        private OrderService _orderService;
        public ReadAllCampaignOffersModel(CampaignOfferService campaignOfferService, OrderService orderService)
        {
            _campaignOfferService = campaignOfferService;
            _orderService = orderService;
        }
        public List<Models.CampaignOffer> CampaignOffers { get; private set; }

        [BindProperty]
        public string SearchString { get; set; }
        //Other search criteria properties can be added here:
        //
        [BindProperty]
        public Models.Order Cart => _orderService.ReadCart();
        [BindProperty]
        public Models.CampaignOffer CampaignOffer { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public OrderLine? OrderLine { get; set; }

        public void OnGet()
        {
            // Get all campaign offers
            CampaignOffers = _campaignOfferService.ReadAllCampaignOffers();
            if (CampaignOffers == null)
            {
                // Handle not found case
                RedirectToPage("./Index");
            }
        }
        // Hvis der skal laves søgninger på campaignoffers, så skal services laves først til det
        //


        public IActionResult OnPostAddOfferToCart()
        {
            CampaignOffer = _campaignOfferService.ReadCampaignOffer(Id);

            _orderService.AddOfferToCart(CampaignOffer, Quantity);

            CampaignOffers = _campaignOfferService.ReadAllCampaignOffers();

            return Page();
        }

        public IActionResult OnPostDeleteOrderLine(int orderLineOfferId, int quantity)
        {
            OrderLine = _orderService.ReadOrderLine(orderLineOfferId, quantity);

            if (OrderLine != null)
            {
                _orderService.DeleteOrderLine(OrderLine);
            }

            CampaignOffers = _campaignOfferService.ReadAllCampaignOffers();

            return Page();
        }
        //Other onpost methods such as filtering can be added here:
        //
    }
}
