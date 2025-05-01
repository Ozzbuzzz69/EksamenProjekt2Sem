namespace EksamenProjekt2Sem.Models
{
    public class OrderLine
    {
        public int Id { get; private set; }
        public int Quantity { get; private set; }
        public double Price { get { return GetOrderlinePrice(); } }
        public Food Food { get; private set; }
        public CampaignOffer CampaignOffer { get; private set; }

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
        public OrderLine(int id, int quantity, Food food, CampaignOffer campaignOffer)
        {
            Id = id;
            Quantity = quantity;
            Food = food;
            CampaignOffer = campaignOffer;
        }
        /// <summary>
        /// Function to set otherwise private id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Function to set otherwise private quantity
        /// Checks if quantity is negative and throws an exception if it is
        /// </summary>
        /// <param name="quantity"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }
            Quantity = quantity;
        }
        /// <summary>
        /// Function to set otherwise private food
        /// </summary>
        /// <param name="food"></param>
        public void SetFood(Food food)
        {
            Food = food;
        }
        /// <summary>
        /// Function to set otherwise private campaign offer
        /// </summary>
        /// <param name="campaignOffer"></param>
        public void SetCampaignOffer(CampaignOffer campaignOffer)
        {
            CampaignOffer = campaignOffer;
        }
        /// <summary>
        /// Function to get the price of the orderline based on the food or campaign offer price and the quantity.
        /// </summary>
        /// <returns></returns>
        public double GetOrderlinePrice()
        {
            if (CampaignOffer != null)
            {
                return CampaignOffer.Price * Quantity;
            }
            else
            {
                return Food.Price * Quantity;
            }
        }
    }
}
