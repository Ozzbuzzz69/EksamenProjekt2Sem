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
            if (_campaignOffers == null)
            {
                _campaignOffers = MockOffer.GetCampaignOffers();
            }
            else
                _campaignOffers = _dbService.GetObjectsAsync().Result.ToList();
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
            if (campaignOffer != null)
            {
                var campaignOffers = _dbService.GetObjectsAsync().Result.ToList();
                foreach (CampaignOffer c in campaignOffers)
                {
                    if (c.Id == campaignOffer.Id)
                    {
                        c.Name = campaignOffer.Name;
                        c.ImageLink = campaignOffer.ImageLink;
                        c.Price = campaignOffer.Price;
                        c.StartTime = campaignOffer.StartTime;
                        c.EndTime = campaignOffer.EndTime;
                    }
                }
                _dbService.SaveObjects(campaignOffers);
            }
        }

        public CampaignOffer DeleteCampaignOffer(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var offerToBeDeleted = _dbService.GetObjectsAsync().Result.FirstOrDefault(o => o.Id == id);
            if (offerToBeDeleted == null)
                throw new Exception($"CampaignOffer with id {id} not found.");

            _dbService.DeleteObjectAsync(offerToBeDeleted).Wait();
            return offerToBeDeleted;
        }

        public void CheckOfferValidities()
        {
            foreach (CampaignOffer offer in _campaignOffers)
            {
                if (offer.StartTime == null || offer.StartTime < DateTime.Now)
                {
                    if (offer.EndTime > DateTime.Now)
                    {
                        offer.IsActive = true;
                        continue;
                    }
                }
                offer.IsActive = false;
            }
        }

        public bool CheckOfferValidity(int id)
        {
            CampaignOffer? offer = _dbService.GetObjectsAsync().Result.FirstOrDefault(o => o.Id == id);
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));
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
