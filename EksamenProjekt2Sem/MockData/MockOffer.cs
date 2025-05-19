namespace EksamenProjekt2Sem.MockData
{
    using EksamenProjekt2Sem.Models;
    public class MockOffer
    {
        private static List<CampaignOffer> _campaignOffers = new List<CampaignOffer>()
        {
            new( "Buy potato, get potato", "https://n3dp.dk/wp-content/uploads/2024/09/Tilbud.png.webp", 15),
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
