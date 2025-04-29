namespace EksamenProjekt2Sem.Models
{
    public class CampaignOffer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public CampaignOffer()
        { }

        /// <summary>
        /// Constructor for CampaignOffer class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        public CampaignOffer(int id, string name, string description, double price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
