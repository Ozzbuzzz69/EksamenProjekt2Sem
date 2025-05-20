using EksamenProjekt2Sem.MockData;
using EksamenProjekt2Sem.Models;
using Microsoft.IdentityModel.Tokens;

namespace EksamenProjekt2Sem.Services
{
    public class SandwichService : GenericDbService<Sandwich>
    {

        private List<Sandwich> _sandwiches;

        private GenericDbService<Sandwich> _dbService;

        public SandwichService(GenericDbService<Sandwich> dbService)
        {
            _dbService = dbService;
            try
            {
                _sandwiches = _dbService.GetObjectsAsync().Result.ToList();
                if (_sandwiches == null || _sandwiches.Count() < 1)
                {
                    SeedSandwichAsync().Wait();
                    _sandwiches = _dbService.GetObjectsAsync().Result.ToList();
                }
            }
            catch (AggregateException ex)
            {
                // Handle the exception as needed
                Console.WriteLine($"Error: {ex.InnerException?.Message}");
            }
            /*
            _dbService = dbService;
            if (_sandwiches == null)
            {
                _sandwiches = MockFood.GetSandwiches();
            }
            else
                _sandwiches = _dbService.GetObjectsAsync().Result.ToList();
            */
        }
        //Getting mock data into the database
        public async Task SeedSandwichAsync()
        {
            _sandwiches = new List<Sandwich>();
            var sandwich = MockFood.GetSandwiches();
            await _dbService.SaveObjects(sandwich);
        }

        /// <summary>
        /// Creates the sandwich from the argument.
        /// </summary>
        /// <param name="sandwich"></param>
        public async Task CreateSandwich(Sandwich sandwich)
        {
            _sandwiches.Add(sandwich);
            await _dbService.AddObjectAsync(sandwich);
        }


        /// <summary>
        /// Finds the sandwich with same id as in argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// A sandwich with matching id.
        /// </returns>
        public Sandwich ReadSandwich(int id)
        {
            var result = _sandwiches.Find(s => s.Id == id);
            if (result == null)
            {
                throw new Exception($"Sandwich with id {id} not found.");
            }
            return result;
        }

        /// <summary>
        /// Reads all sandwiches.
        /// </summary>
        /// <returns>
        /// Returns the list of sandwiches.
        /// </returns>
        public List<Sandwich> ReadAllSandwiches()
        {
            return _sandwiches;
        }

        /// <summary>
        /// Updates the sandwiches properties, so it matches the sandwich in the argument.
        /// </summary>
        /// <param name="sandwich"></param>
        public void UpdateSandwich(Sandwich sandwich)
        {
            if (sandwich != null)
            {
                foreach (Sandwich s in _sandwiches)
                {
                    if (s.Id == sandwich.Id)
                    {
                        s.Ingredients = sandwich.Ingredients;
                        s.InSeason = sandwich.InSeason;
                        s.MeatType = sandwich.MeatType;
                        s.Price = sandwich.Price;
                    }
                }
                _dbService.UpdateObjectAsync(sandwich).Wait();
            }
        }

        /// <summary>
        /// Deletes the sandwich with the same id as given in argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Returns the sandwich to be deleted.
        /// </returns>
        public Sandwich? DeleteSandwich(int? id)
        {
            Sandwich? sandwichToBeDeleted = null;
            foreach (Sandwich sandwich in _sandwiches)
            {
                if (sandwich.Id == id)
                {
                    sandwichToBeDeleted = sandwich;
                    break;
                }
            }
            if (sandwichToBeDeleted != null)
            {
                _sandwiches.Remove(sandwichToBeDeleted);
                _dbService.DeleteObjectAsync(sandwichToBeDeleted).Wait();
            }
            return sandwichToBeDeleted;
        }

        //public void AddSandwichToCart(int quantity, int id)
        //{
        //    OrderLine orderLine = new OrderLine();

        //    orderLine.Quantity = quantity;
        //    orderLine.Food = Sandwich;

        //    Order.OrderLines.Add(orderLine);
        //}



        #region Sorting/Filtering functions
        #region Filtering functions

        /// <summary>
        /// Filters sandwiches by specified category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns>
        /// Returns a list of sandwiches with the category as given in argument.
        /// </returns>
        public List<Sandwich> SearchSandwichByCategory(string criteria)
        {
            return _sandwiches.FindAll(s => string.IsNullOrEmpty(criteria) || s.Category.ToLower().Contains(criteria.ToLower()));
        }

        /// <summary>
        /// Filters sandwiches by specified meat type.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Sandwich> SearchSandwichByMeatType(string criteria)
        {
            return _sandwiches.FindAll(s => string.IsNullOrEmpty(criteria) || s.MeatType.ToLower().Contains(criteria.ToLower()));
        }


        /// <summary>
        /// Filters food by specified ingredients.
        /// </summary>
        /// <param name="meatType"></param>
        /// <returns>
        /// Returns a list of matching sandwiches.
        /// </returns>
        public List<Sandwich> FilterSandwichByIngredient(string criteria)
        {
            return _sandwiches.FindAll(s => s.Ingredients.ToLower().Contains(criteria.ToLower()));
        }


        /// <summary>
        /// Finds all sandwiches that cost more than the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Sandwich> FilterSandwichByPriceLower(double criteria)
        {
            return _sandwiches.FindAll(s => s.Price >= criteria);
        }

        /// <summary>
        /// Finds all sandwiches that cost less than the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Sandwich> FilterSandwichByPriceUpper(double criteria)
        {
            return _sandwiches.FindAll(s => s.Price <= criteria);
        }

        /// <summary>
        /// Finds all sandwiches that cost between the two criteria (including the criteria).
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public List<Sandwich> FilterSandwichByPriceRange(double lower, double upper)
        {
            return _sandwiches.FindAll(s => s.Price >= lower && s.Price <= upper);
        }
    #endregion

    #region Sorting functions
        /// <summary>
        /// Gets all sandwiches sorted by id.
        /// </summary>
        /// <returns></returns>
        public List<Sandwich> GetSandwichesSortedById()
        {
            return SortById(_sandwiches);
        }

        /// <summary>
        /// Gets all sandwiches sorted by price.
        /// </summary>
        /// <returns></returns>
        public List<Sandwich> GetSandwichesSortedByPrice()
        {
            return SortByCriteria(_sandwiches, "Price");
        }
    #endregion
#endregion
    }
}