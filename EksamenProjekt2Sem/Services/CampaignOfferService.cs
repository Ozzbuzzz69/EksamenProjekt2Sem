using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class CampaignOfferService : GenericDbService<CampaignOffer>
    {

        private List<CampaignOffer> _campaignOffers;
        private GenericDbService<CampaignOffer> _dbService;

        public CampaignOfferService(GenericDbService<CampaignOffer> dbService)
        {
            _dbService = dbService;
            try
            {
                _campaignOffers = _dbService.GetObjectsAsync().Result.ToList();
                if (_campaignOffers == null || _campaignOffers.Count() < 1)
                {
                    SeedCampaignAsync().Wait();
                    _campaignOffers = _dbService.GetObjectsAsync().Result.ToList();
                }
            }
            catch (AggregateException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
            }
            /*
            if (_campaignOffers == null)
            {
                _campaignOffers = MockOffer.GetCampaignOffers();
            }
            else
                _campaignOffers = _dbService.GetObjectsAsync().Result.ToList();
            */
        }

        //Getting mock data into the database
        public async Task SeedCampaignAsync()
        {
            _campaignOffers = new List<CampaignOffer>();
            var campaign = MockOffer.GetCampaignOffers();
            await _dbService.SaveObjects(campaign);
        }
        public async Task CreateCampaignOffer(CampaignOffer campaignOffer)
        {
            _campaignOffers.Add(campaignOffer);
			await _dbService.AddObjectAsync(campaignOffer);
		}
        public CampaignOffer ReadCampaignOffer(int id)
        {
            var result = _campaignOffers.Find(s => s.Id == id);
            if (result == null)
            {
                throw new Exception($"CampaignOffer with id {id} not found.");
            }
            return result;
        }
        public List<CampaignOffer> ReadAllCampaignOffers()
        {
            return _campaignOffers;
		}
        public void UpdateCampaignOffer(CampaignOffer campaignOffer)
        {
            if (campaignOffer != null)
            {
                foreach (CampaignOffer c in _campaignOffers)
                {
                    if (c.Id == campaignOffer.Id)
                    {
                        c.Name = campaignOffer.Name;
                        c.ImageLink = campaignOffer.ImageLink;
                        c.Price = campaignOffer.Price;
                    }
                }
                _dbService.UpdateObjectAsync(campaignOffer).Wait();
            }
        }
        public CampaignOffer? DeleteCampaignOffer(int? id)
        {
           CampaignOffer? campaignOfferToBeDeleted = null;
            foreach (CampaignOffer c in _campaignOffers)
            {
                if (c.Id == id)
                {
                    campaignOfferToBeDeleted = c;
                    break;
                }
            }
            if (campaignOfferToBeDeleted != null)
            {
                _campaignOffers.Remove(campaignOfferToBeDeleted);
                _dbService.DeleteObjectAsync(campaignOfferToBeDeleted).Wait();
            }
            return campaignOfferToBeDeleted;
        }
    }
}
