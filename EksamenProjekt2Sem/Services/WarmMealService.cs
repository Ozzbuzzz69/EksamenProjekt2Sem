using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;

namespace EksamenProjekt2Sem.Services
{
    public class WarmMealService : GenericDbService<WarmMeal>
    {

        private List<WarmMeal> _warmMeals;
        private GenericDbService<WarmMeal> _dbService;

        public WarmMealService(GenericDbService<WarmMeal> dbService)
        {
            _dbService = dbService;
            if (_warmMeals == null)
            {
                _warmMeals = MockFood.GetWarmMeals();
            }
            else
                _warmMeals = _dbService.GetObjectsAsync().Result.ToList();
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
            return _warmMeals.Find(w => w.Id == id);
        }

        /// <summary>
        /// Reads all warm meals.
        /// </summary>
        /// <returns>
        /// Returns the list of warm meals.
        /// </returns>
        public List<WarmMeal> ReadAllWarmMeals()
        {
            return _warmMeals;
        }

        /// <summary>
        /// Updates the warm meal's properties to the same as the warm meal given in argument.
        /// </summary>
        /// <param name="warmMeal"></param>
        public void UpdateWarmMeal(WarmMeal warmMeal)
        {
            if (warmMeal != null)
            {
                foreach (WarmMeal w in _warmMeals)
                {
                    if (w.Id == warmMeal.Id)
                    {
                        w.Ingredients = warmMeal.Ingredients;
                        w.InSeason = warmMeal.InSeason;
                        w.MeatType = warmMeal.MeatType;
                        w.Price = warmMeal.Price;
                        w.MinPersonAmount = warmMeal.MinPersonAmount;
                    }
                }
                _dbService.UpdateObjectAsync(warmMeal).Wait();
            }
        }

        /// <summary>
        /// Deletes the warm meal with same id as in argument.
        /// </summary>
        /// <param name="id"></param>
        public WarmMeal DeleteWarmMeal(int? id)
        {
            WarmMeal warmMealToBeDeleted = null;

            foreach (WarmMeal warmMeal in _warmMeals)
            {
                if (warmMeal.Id == id)
                {
                    warmMealToBeDeleted = warmMeal;
                    break;
                } 
            }
            if (warmMealToBeDeleted == null)
            {
                throw new Exception($"WarmMeal with id {id} not found.");
            }


            if (warmMealToBeDeleted != null)
            {
                _warmMeals.Remove(warmMealToBeDeleted);
                _dbService.DeleteObjectAsync(warmMealToBeDeleted).Wait();
            }
            return warmMealToBeDeleted;
        }

#region Sorting/Filtering functions
    #region Filtering functions

        /// <summary>
        /// Filters warm meals by specified minimum person amount.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByMinPersonLower(int criteria)
        {
            return _warmMeals.FindAll(w => w.MinPersonAmount >= criteria);
        }

        /// <summary>
        /// Filters warm meals by specified maximum person amount.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByMinPersonUpper(int criteria)
        {
            return _warmMeals.FindAll(w => w.MinPersonAmount <= criteria);
        }

        /// <summary>
        /// Filters warm meals by specified person amount range (including upper/lower).
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByMinPersonRange(int lower, int upper)
        {
            return _warmMeals.FindAll(w => w.MinPersonAmount >= lower && w.MinPersonAmount <= upper);
        }

        /// <summary>
        /// Filters food by specified ingredients.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns>
        /// Returns a list of warm meals that matches the criteria.
        /// </returns>
        public List<WarmMeal> FilterWarmMealByIngredient(string criteria)
        {
            return _warmMeals.FindAll(s => s.Ingredients.ToLower().Contains(criteria.ToLower()));
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


        /// <summary>
        /// Finds all warm meals that cost more than the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByPriceLower(double criteria)
        {
            return _warmMeals.FindAll(s => s.Price >= criteria);
        }

        /// <summary>
        /// Finds all warm meals that cost less than the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByPriceUpper(double criteria)
        {
            return _warmMeals.FindAll(s => s.Price <= criteria);
        }

        /// <summary>
        /// Finds all Warm Meals that cost between the two criteria (including the criteria).
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public List<WarmMeal> FilterWarmMealByPriceRange(double lower, double upper)
        {
            return _warmMeals.FindAll(s => s.Price >= lower && s.Price <= upper);
        }
    #endregion
    #region Sorting functions
        /// <summary>
        /// Gets all warm meals sorted by id.
        /// </summary>
        /// <returns></returns>
        public List<WarmMeal> GetWarmMealsSortedById()
        {
            return SortById(_warmMeals);
        }

        /// <summary>
        /// Gets all warm meals sorted by price.
        /// </summary>
        /// <returns></returns>
        public List<WarmMeal> GetWarmMealsSortedByPrice()
        {
            return SortByCriteria(_warmMeals, "Price");
        }
    #endregion
#endregion
    }
}