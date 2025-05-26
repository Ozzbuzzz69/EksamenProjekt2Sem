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
            if (_campaignOffers == null)
            {
                _campaignOffers = new();
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
            var campaign = MockOffer.GetCampaignOffers();
            await _dbService.SaveObjects(campaign);
        }

        public async Task CreateCampaignOffer(CampaignOffer offer)
        {
            await _dbService.AddObjectAsync(offer);
        }

        public CampaignOffer ReadCampaignOffer(int id)
        {
            var offer = _dbService.GetObjectsAsync().Result.FirstOrDefault(o => o.Id == id);
            if (offer == null)
                throw new Exception($"CampaignOffer with id {id} not found.");
            return offer;
        }

        public List<CampaignOffer> ReadAllCampaignOffers()
        {
            return _dbService.GetObjectsAsync().Result.ToList();
        }

        public void UpdateCampaignOffer(CampaignOffer campaignOffer)
        {
            if (campaignOffer == null)
                return;

            var existingOffer = _campaignOffers.FirstOrDefault(w => w.Id == campaignOffer.Id);
            if (existingOffer == null)
                return;

            existingOffer.Name = campaignOffer.Name;
            existingOffer.ImageLink = campaignOffer.ImageLink;
            existingOffer.Price = campaignOffer.Price;
            existingOffer.StartTime = campaignOffer.StartTime;
            existingOffer.EndTime = campaignOffer.EndTime;

            _dbService.UpdateObjectAsync(existingOffer).Wait(); // Only update if the meal exists
        }

        public CampaignOffer? DeleteCampaignOffer(int? id)
        {
            var offerToBeDeleted = _dbService.GetObjectsAsync().Result.FirstOrDefault(s => s.Id == id);
            if (offerToBeDeleted == null) return null;

            _dbService.DeleteObjectAsync(offerToBeDeleted).Wait();
            return offerToBeDeleted;

            /*
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var offerToBeDeleted = _dbService.GetObjectsAsync().Result.FirstOrDefault(o => o.Id == id);
            if (offerToBeDeleted == null)
                throw new Exception($"CampaignOffer with id {id} not found.");

            _dbService.DeleteObjectAsync(offerToBeDeleted).Wait();
            return offerToBeDeleted;
            */
        }

        public void SetOfferValidities()
        {
            List<CampaignOffer> temp = ReadAllCampaignOffers();
            foreach (CampaignOffer offer in temp)
            {
                if (CheckOfferValidity(offer))
                {
                    offer.IsActive = true;
                    _dbService.UpdateObjectAsync(offer).Wait();
                    continue;
                }
                else
                {
                    offer.IsActive = false;
                    _dbService.UpdateObjectAsync(offer).Wait();
                }
            }
        }

        public bool CheckOfferValidity(CampaignOffer offer)
        {
            if (offer.StartTime == null || offer.StartTime < DateTime.Now)
            {
                if (offer.EndTime > DateTime.Now)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
