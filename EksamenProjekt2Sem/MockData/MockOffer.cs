namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    public class MockOffer
    {
        private static List<CampaignOffer> _campaignOffers = new List<CampaignOffer>()
        {
            new(1, "Buy potato, get potato", "https://rb.gy/ejmnqi", 15),
            new(2, "Half price Monster Burger", "https://rb.gy/ejmnqi", 99.95),
            new(3, "9-Gold Flakes, 10th free", "https://rb.gy/ejmnqi", 539.55)
        };
        /// <summary>
        /// Returns a list of mock campaign offers.
        /// </summary>
        /// <returns></returns>
        public static List<CampaignOffer> GetCampaignOffers()
        {
            return _campaignOffers;
        }
    }
}
