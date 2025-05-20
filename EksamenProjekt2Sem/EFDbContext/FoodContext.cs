using EksamenProjekt2Sem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EksamenProjektTest.EFDbContext
{
    public class FoodContext : DbContext
    {
        /// <summary>
        /// Kontruktør tilføjet så man optionally kan ændre config.
        /// Skal bruges for at kunne teste databasen i sit eget.
        /// 
        /// Bør IKKE ændre noget i resten af projektet.
        /// </summary>
        /// <param name="options"></param>
        public FoodContext(DbContextOptions<FoodContext> options) : base(options) { }

        /// <summary>
        /// Base kontruktør.
        /// Tilføjet for den optional til databasen ikke har en indflydelse på funktionalitet af databasen.
        /// </summary>
       

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FoodContentDB;Integrated Security=True;Connect Timeout=30;Encrypt=False");
            }
        }
        public DbSet<CampaignOffer> CampaignOffers { get; set; }
        public DbSet<Sandwich> Sandwiches { get; set; }
        public DbSet<WarmMeal> WarmMeals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
