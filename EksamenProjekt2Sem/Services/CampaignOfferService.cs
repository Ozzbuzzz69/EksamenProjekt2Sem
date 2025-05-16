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

        //public async Task SeedCampaignAsync()
        //{
        //    _campaignOffers = new List<CampaignOffer>();
        //    var campaign = MockOffer.GetCampaignOffers();
        //    await _dbService.SaveObjects(campaign);
        //}
        public void CreateCampaignOffer(CampaignOffer campaignOffer)
        {
            _campaignOffers.Add(campaignOffer);
			_dbService.AddObjectAsync(campaignOffer);
		}
        public CampaignOffer ReadCampaignOffer(int id)
        {
            return _dbService.GetObjectByIdAsync(id).Result;
		}
        public List<CampaignOffer> ReadAllCampaignOffers()
        {
            return _dbService.GetObjectsAsync().Result.ToList();
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
                _dbService.SaveObjects(_campaignOffers);
            }
        }
        public CampaignOffer DeleteCampaignOffer(int? id)
        {
           CampaignOffer campaignOfferToBeDeleted = null;
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
                _dbService.SaveObjects(_campaignOffers);
            }
            return campaignOfferToBeDeleted;
        }
    }
}
