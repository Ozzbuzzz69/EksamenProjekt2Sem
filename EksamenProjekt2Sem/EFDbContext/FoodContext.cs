using EksamenProjekt2Sem.Models;
using Microsoft.EntityFrameworkCore;
using EksamenProjekt2Sem.Secrets;
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
       

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(
                $@"Data Source=mssql10.unoeuro.com;
                Initial Catalog=koebmandenellevej_dk_db_ellevej_database;
                User ID=koebmandenellevej_dk;
                Password={Passwords.DbPassword};
                Connect Timeout=30;
                Encrypt=True;
                Trust Server Certificate=True;
                Application Intent=ReadWrite;
                Multi Subnet Failover=False");
            }
        }
        public DbSet<CampaignOffer> CampaignOffers { get; set; }
        public DbSet<Sandwich> Sandwiches { get; set; }
        public DbSet<WarmMeal> WarmMeals { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
