using EksamenProjekt2Sem.Models;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
namespace EksamenProjekt2Sem.Services
{
    public class GenericDbService<T> where T : class
    {
        public async Task<IEnumerable<T>> GetObjectsAsync()
        {
            using (var context = new FoodContext())
            {
                return await context.Set<T>().AsNoTracking().ToListAsync();
            }
        }
        public async Task AddObjectAsync(T obj)
        {
            using (var context = new FoodContext())
            {
                context.Set<T>().Add(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task SaveObjects(List<T> objs)
        {
            using (var context = new FoodContext())
            {
                foreach (T obj in objs)
                {
                    context.Set<T>().Add(obj);
                    context.SaveChanges();
                }

                context.SaveChanges();
            }
        }
        public async Task DeleteObjectAsync(T obj)
        {
            using (var context = new FoodContext())
            {
                context.Set<T>().Remove(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateObjectAsync(T obj)
        {
            using (var context = new FoodContext())
            {
                context.Set<T>().Update(obj);
                await context.SaveChangesAsync();
            }
        }
        public async Task<T> GetObjectByIdAsync(int id)
        {
            using (var context = new FoodContext())
            {
                return await context.Set<T>().FindAsync(id);
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
            return list
                .Where(l => l?.GetType().GetProperty(criteria)?.GetValue(l, null) != null)
                .OrderBy(l => l.GetType().GetProperty(criteria)?.GetValue(l, null))
                .ToList();
        }
        #endregion
    }
}
