using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class CampaignOfferService : GenericDbService<CampaignOffer>
    {
        private GenericDbService<CampaignOffer> _dbService;

        public CampaignOfferService(CampaignOfferService campaignOfferService)
        {
            // Constructor logic here
            _dbService = campaignOfferService;
        }
        public void CreateCampaignOffer(CampaignOffer campaignOffer)
        {
            // Add campaign offer to Database
        }
        public CampaignOffer ReadCampaignOffer(int id)
        {
            // Read campaign offer from Database
            return new CampaignOffer(); // Placeholder return
        }
        public List<CampaignOffer> ReadAllCampaignOffers()
        {
            // Read all campaign offers from Database
            return new List<CampaignOffer>(); // Placeholder return
        }
        public void UpdateCampaignOffer(int id, CampaignOffer campaignOffer)
        {
            // Update campaign offer by id in Database
        }
        public CampaignOffer DeleteCampaignOffer(int id)
        {
            // Delete campaign offer from Database
            return new CampaignOffer(); // Placeholder return
        }
    }
}
