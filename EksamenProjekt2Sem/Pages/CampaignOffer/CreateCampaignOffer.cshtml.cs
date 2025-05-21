using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.CampaignOffer
{
    [Authorize(Roles = "admin")]

    public class CreateCampaignOfferModel : PageModel
    {
        private CampaignOfferService _campaignOfferService;
        public CreateCampaignOfferModel(CampaignOfferService campaignOfferService)
        {
            _campaignOfferService = campaignOfferService;
        }

        [BindProperty]
        public Models.CampaignOffer CampaignOffer { get; set; }
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
            _campaignOfferService.CreateCampaignOffer(CampaignOffer).Wait();
            return RedirectToPage("ReadAllCampaignOffers");
        }
    }
}
