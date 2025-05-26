using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.CampaignOffer
{
    [Authorize(Roles = "admin")]

    public class UpdateCampaignOfferModel : PageModel
    {
        private Services.CampaignOfferService _campaignOfferService;
        public UpdateCampaignOfferModel(Services.CampaignOfferService campaignOfferService)
        {
            _campaignOfferService = campaignOfferService;
        }
        [BindProperty]
        public Models.CampaignOffer CampaignOffer { get; set; }
        public IActionResult OnGet(int id)
        {
            CampaignOffer = _campaignOfferService.ReadCampaignOffer(id);
            if (CampaignOffer == null)
            {
                return RedirectToPage("/NotFound");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _campaignOfferService.UpdateCampaignOffer(CampaignOffer);
            return RedirectToPage("ReadAllCampaignOffers");
        }
    }
}
