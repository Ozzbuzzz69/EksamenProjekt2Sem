namespace EksamenProjekt2Sem.Models
{
    public class CampaignOffer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }

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
        /// <summary>
        /// Function to set otherwise private id
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
        }
        /// <summary>
        /// Function to set otherwise private name
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Function to set otherwise private description
        /// </summary>
        /// <param name="description"></param>
        public void SetDescription(string description)
        {
            Description = description;
        }
        /// <summary>
        /// Function to set otherwise private price
        /// </summary>
        /// <param name="price"></param>
        public void SetPrice(double price)
        {
            Price = price;
        }
    }
}
