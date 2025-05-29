using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class WarmMealService : GenericDbService<WarmMeal>
    {

        private List<WarmMeal> _warmMeals;
        private List<Order> _orders;

        private GenericDbService<WarmMeal> _dbService;
        private GenericDbService<Order> _dbOrderService;

        public WarmMealService(GenericDbService<WarmMeal> dbService, GenericDbService<Order> dbOrderService)
        {
            _dbService = dbService;
            _dbOrderService = dbOrderService;
            try
            {
                _warmMeals = _dbService.GetObjectsAsync().Result.ToList();
                if (_warmMeals == null || _warmMeals.Count() < 1)
                {
                    SeedWarmMealAsync().Wait();
                    _warmMeals = _dbService.GetObjectsAsync().Result.ToList();
                }
                _orders = _dbOrderService.GetObjectsAsync().Result.ToList();
            }
            catch (AggregateException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
            }
            if (_warmMeals == null)
            {
                _warmMeals = new();
            }
        }
        //Getting mock data into the database
        public async Task SeedWarmMealAsync()
        {
            _warmMeals = new List<WarmMeal>();
            var warmmeal = MockFood.GetWarmMeals();
            await _dbService.SaveObjects(warmmeal);
        }

        /// <summary>
        /// Creates the warm meal from argument.
        /// </summary>
        /// <param name="warmMeal"></param>
        public async Task CreateWarmMeal(WarmMeal warmMeal)
        {
            _warmMeals.Add(warmMeal);
            await _dbService.AddObjectAsync(warmMeal);
        }

        /// <summary>
        /// Reads the warm meal with same id as argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Returns the matching warm meal.
        /// </returns>
        public WarmMeal ReadWarmMeal(int id)
        {
            var warmMeal = _dbService.GetObjectsAsync().Result.FirstOrDefault(w => w.Id == id);
            if (warmMeal == null)
                throw new Exception($"WarmMeal with id {id} not found.");
            return warmMeal;
        }

        /// <summary>
        /// Reads all warm meals.
        /// </summary>
        /// <returns>
        /// Returns the list of warm meals.
        /// </returns>
        public List<WarmMeal> ReadAllWarmMeals()
        {
            return _dbService.GetObjectsAsync().Result.ToList();
        }

        /// <summary>
        /// Updates the warm meal's properties to the same as the warm meal given in argument.
        /// </summary>
        /// <param name="warmMeal"></param>
        public void UpdateWarmMeal(WarmMeal warmMeal)
        {
            if (warmMeal == null)
                return;

            var existingMeal = _warmMeals.FirstOrDefault(w => w.Id == warmMeal.Id);
            if (existingMeal == null)
                return;

            existingMeal.Ingredients = warmMeal.Ingredients;
            existingMeal.InSeason = warmMeal.InSeason;
            existingMeal.MeatType = warmMeal.MeatType;
            existingMeal.Price = warmMeal.Price;
            existingMeal.MinPersonAmount = warmMeal.MinPersonAmount;

            _dbService.UpdateObjectAsync(existingMeal).Wait(); // Only update if the meal exists
        }

        /// <summary>
        /// Deletes the warm meal with same id as in argument.
        /// </summary>
        /// <param name="id"></param>
        public WarmMeal DeleteWarmMeal(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var warmMealToBeDeleted = _dbService.GetObjectsAsync().Result.FirstOrDefault(w => w.Id == id);
            if (warmMealToBeDeleted == null)
                throw new Exception($"WarmMeal with id {id} not found.");
            if (_orders.Any(o => o.FoodId == id)) return null;

            _dbService.DeleteObjectAsync(warmMealToBeDeleted).Wait();
            return warmMealToBeDeleted;
        }

        /// <summary>
        /// Filters warm meals by specified meat type.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<WarmMeal> SearchWarmMealByMeatType(string criteria)
        {
            return _warmMeals.FindAll(w => string.IsNullOrEmpty(criteria) || w.MeatType.ToLower().Contains(criteria.ToLower()));    
        }
    }
}