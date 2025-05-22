using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace EksamenProjekt2Sem.Models
{
    public class CampaignOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100, ErrorMessage = "Maks 100 tegn")]
        public string Name { get; set; }
        [Required]
        public string ImageLink { get; set; }
        [Required]
        public double Price { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        //Annotation needed
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

        //Annotation needed
        public bool IsActive { get; set; }

        public CampaignOffer()
        { }
        
       
        public CampaignOffer( string name, string imageLink, double price)
        {
            
            Name = name;
            ImageLink = imageLink;
            Price = price;
        }
        public CampaignOffer(string name, string imageLink, double price, DateTime? startTime, DateTime endTime)
        {
            Name = name;
            ImageLink = imageLink;
            Price = price;
            StartTime = startTime;
            EndTime = endTime;
            if (DateTime.Now > startTime || startTime == null)
            {
                if (DateTime.Now > endTime) IsActive = false;
                else IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }
    }
}
