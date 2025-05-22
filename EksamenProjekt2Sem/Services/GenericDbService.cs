using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
namespace EksamenProjekt2Sem.Services
{
    public class GenericDbService<T> where T : class
    {
        private readonly DbContextOptions<FoodContext> _options;

        public GenericDbService(DbContextOptions<FoodContext> options)
        {
            _options = options;
        }
        public GenericDbService()
        {
            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FoodContentDB;Integrated Security=True;Connect Timeout=30;Encrypt=False")
                .Options;
            // Har indsat en default konstruktør som fungere ligsom FoodContext, men jeg kunne ikke bruge
            // FoodContext i GenericDbService, da den jeg skulle bruge en non-generic konstruktør til tests.
        }


        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            using (var context = new FoodContext(_options))
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }
        public async Task AddObjectAsync(T obj)
        {
            using (var context = new FoodContext(_options))
            {
                context.Set<T>().Add(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task SaveObjects(List<T> objs)
        {
            using (var context = new FoodContext(_options))
            {
                context.Set<T>().AddRange(objs);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteObjectAsync(T obj)
        {
            using (var context = new FoodContext(_options))
            {
                context.Set<T>().Remove(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateObjectAsync(T obj)
        {
            using (var context = new FoodContext(_options))
            {
                context.Set<T>().Update(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task<T> GetObjectByIdAsync(int id)
        {
            using (var context = new FoodContext(_options))
            {
                var result = await context.Set<T>().FindAsync(id);
                if (result == null)
                {
                    throw new Exception($"Object with id {id} not found.");
                }
                return result;
            }
        }

#region Sorting functions
        /// <summary>
        /// Given a list, it sorts the objects by their Id property.
        /// Any null values are ignored.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<T> SortById(List<T> list)
        {
            return list
                .Where(l => l?.GetType().GetProperty("Id")?.GetValue(l, null) != null)
                .OrderBy(l => l.GetType().GetProperty("Id")?.GetValue(l, null))
                .ToList();
        }

        /// <summary>
        /// Given a list, it sorts the objects by the given criteria.
        /// Criteria is a string which is the name of the property to sort by (remember caps).
        /// </summary>
        /// <param name="list"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<T> SortByCriteria(List<T> list, string criteria)
        {
            List<T> sortedList = new List<T>();
            List<T> unsortedList = new List<T>();

            foreach (T l in list)
            {
                if (l?.GetType().GetProperty(criteria)?.GetValue(l, null) != null)
                {
                    sortedList.Add(l);
                }
                else
                {
                    unsortedList.Add(l);
                }
            }
            sortedList = sortedList
                .OrderBy(l => l.GetType().GetProperty(criteria)?.GetValue(l, null))
                .ToList();

            sortedList.AddRange(unsortedList);

            return sortedList;

            /*
            return list
                .Where(l => l?.GetType().GetProperty(criteria)?.GetValue(l, null) != null)
                .OrderBy(l => l.GetType().GetProperty(criteria)?.GetValue(l, null))
                .ToList();
            */
        }
#endregion
    }
}
