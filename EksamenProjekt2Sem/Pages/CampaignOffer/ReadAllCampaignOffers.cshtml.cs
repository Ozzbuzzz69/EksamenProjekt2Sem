using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EksamenProjekt2Sem.Pages.CampaignOffer
{
    public class ReadAllCampaignOffersModel : PageModel
    {
        private CampaignOfferService _campaignOfferService;
        public ReadAllCampaignOffersModel(CampaignOfferService campaignOfferService)
        {
            _campaignOfferService = campaignOfferService;
        }
        public List<Models.CampaignOffer> CampaignOffers { get; private set; }

        [BindProperty]
        public string SearchString { get; set; }


        public void OnGet()
        {
            // Update the validity of Offers
            _campaignOfferService.SetOfferValidities();

            // Get all campaign offers
            CampaignOffers = _campaignOfferService.ReadAllCampaignOffers();
            if (CampaignOffers == null)
            {
                // Handle not found case
                RedirectToPage("Index");
            }
        }
        // Hvis der skal laves søgninger på campaignoffers, så skal services laves først til det
        //
    }
}
