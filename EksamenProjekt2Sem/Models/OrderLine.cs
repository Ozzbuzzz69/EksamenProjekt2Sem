namespace EksamenProjekt2Sem.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Food Food { get; set; }
        public CampaignOffer CampaignOffer { get; set; }
    }
}
