using EksamenProjekt2Sem.Models;
using EksamenProjekt2Sem.Secrets;
using EksamenProjektTest.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            //_options = new DbContextOptionsBuilder<FoodContext>()
            //    .UseSqlServer(
            //    $@"Data Source=mssql10.unoeuro.com;
            //    Initial Catalog=koebmandenellevej_dk_db_ellevej_database;
            //    User ID=koebmandenellevej_dk;
            //    Password={Passwords.DbPassword};
            //    Connect Timeout=30;
            //    Encrypt=True;
            //    Trust Server Certificate=True;
            //    Application Intent=ReadWrite;
            //    Multi Subnet Failover=False")
            //    .Options;

            _options = new DbContextOptionsBuilder<FoodContext>()
                .UseSqlServer($@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False").Options;
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
    }
}
