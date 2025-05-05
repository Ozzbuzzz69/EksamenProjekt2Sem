using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EksamenProjekt2Sem.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public User User { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; } = DateTime.Now;

        [Required(ErrorMessage = "Der skal angives en afhentningstid")]
        [DataType(DataType.DateTime)]
        public DateTime PickupTime
        {
            get; set
            {
                try
                {
                    if (PickupTime < DateTime.Now || PickupTime == DateTime.Now.AddDays(1))
                    {
                        throw new ArgumentNullException("Ugyldig dato");
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }       
            }             
        }

        public double TotalPrice { get; set; }

        private List<OrderLine> orderLines = new List<OrderLine>();


        //Lav orderline funktion
        public Order()
        { }

        
        public Order(User user, DateTime pickupTime)
        {
            User = user;
            PickupTime = pickupTime;
        }
    }
}
