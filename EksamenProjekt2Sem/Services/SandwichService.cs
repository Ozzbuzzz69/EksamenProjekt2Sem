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
            if (_sandwiches == null)
            {
                _sandwiches = new();
            }
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
            var sandwich = _dbService.GetObjectsAsync().Result.FirstOrDefault(s => s.Id == id);
            if (sandwich == null)
            {
                throw new Exception($"Sandwich with id {id} not found.");
            }
            return sandwich;
        }

        /// <summary>
        /// Reads all sandwiches.
        /// </summary>
        /// <returns>
        /// Returns the list of sandwiches.
        /// </returns>
        public List<Sandwich> ReadAllSandwiches()
        {
            return _dbService.GetObjectsAsync().Result.ToList();
        }

        /// <summary>
        /// Updates the sandwiches properties, so it matches the sandwich in the argument.
        /// </summary>
        /// <param name="sandwich"></param>
        public void UpdateSandwich(Sandwich sandwich)
        {
            if (sandwich == null)
                return;

            var existingSandwich = _sandwiches.FirstOrDefault(w => w.Id == sandwich.Id);
            if (existingSandwich == null)
                return;

            existingSandwich.Ingredients = sandwich.Ingredients;
            existingSandwich.InSeason = sandwich.InSeason;
            existingSandwich.MeatType = sandwich.MeatType;
            existingSandwich.Price = sandwich.Price;
            existingSandwich.Category = sandwich.Category;

            _dbService.UpdateObjectAsync(existingSandwich).Wait(); // Only update if the meal exists
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
            var sandwichToBeDeleted = _dbService.GetObjectsAsync().Result.FirstOrDefault(s => s.Id == id);
            if (sandwichToBeDeleted == null) return null;

            _dbService.DeleteObjectAsync(sandwichToBeDeleted).Wait();
            return sandwichToBeDeleted;
        }


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
            List<Sandwich> temp = new();

            foreach (Sandwich s in _sandwiches)
            {
                if (s == null)
                {
                    continue;
                }
                if (s.MeatType == null)
                {
                    temp.Add(s);
                    continue;
                }
                if (s.MeatType.ToLower().Contains(criteria.ToLower()))
                {
                    temp.Add(s);
                }
            }
            return temp;

            //return _sandwiches.FindAll(s => string.IsNullOrEmpty(criteria) || s.MeatType.ToLower().Contains(criteria.ToLower()) || s.MeatType.IsNullOrEmpty());
        }


    }
}

