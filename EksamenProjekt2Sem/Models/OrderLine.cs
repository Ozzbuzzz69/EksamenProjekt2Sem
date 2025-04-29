namespace EksamenProjekt2Sem.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Food Food { get; set; }
        public CampaignOffer CampaignOffer { get; set; }

        public OrderLine()
        { }

        /// <summary>
        /// Constructor for OrderLine class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="food"></param>
        /// <param name="campaignOffer"></param>
        public OrderLine(int id, int quantity, double price, Food food, CampaignOffer campaignOffer)
        {
            Id = id;
            Quantity = quantity;
            Price = price;
            Food = food;
            CampaignOffer = campaignOffer;
        }
    }
}
