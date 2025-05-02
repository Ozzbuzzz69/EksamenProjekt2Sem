using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class FoodService : GenericDbService<Food>
    {
        private List<Food> _sandwiches;
        private GenericDbService<Food> _dbService;

        public FoodService(FoodService foodService)
        {
            _sandwiches = new List<Food>();
            _dbService = foodService;
        }
        public FoodService()
        {
            _sandwiches = new List<Food>();
            _dbService = new GenericDbService<Food>();
        }
        public void CreateFood(Food food)
        {
            // Add food to Database
        }
        public Food ReadFood(int id)
        {
            // Read food from Database
            return new Sandwich(); // Placeholder return
        }
        public List<Food> ReadAllFood()
        {
            // Read all food from Database
            return new List<Food>(); // Placeholder return
        }
        public void UpdateFood(int id, Food food)
        {
            // Update food by id in Database
        }
        public Food DeleteFood(int id)
        {
            // Delete food from Database
            return new Sandwich(); // Placeholder return
        }

        //+ FilterFoodByType(string str) : List<Food>
        //+ SpecialOffer(Food food, double price, DateTime dateFrom, DateTime dateTo) : void  
        //+ FilterFoodByCriteria(?) : List<Food>
    }
}
