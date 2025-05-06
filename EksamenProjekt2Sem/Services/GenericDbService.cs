namespace EksamenProjekt2Sem.Services
{
    using EksamenProjekt2Sem.AppDbContext;
    using Microsoft.EntityFrameworkCore;

    public class GenericDbService<T> where T : class
    {
        
        public GenericDbService() { } // Placeholder for the constructor

        public Task<IEnumerable<T>> GetObjectsAsync()
        {
            // Placeholder for the method to get objects from the database
            return Task.FromResult<IEnumerable<T>>(new List<T>());
        }
        public Task AddObjectAsync(T obj)
        {
            // Placeholder for the method to add an object to the database
            return Task.CompletedTask;
        }
        public Task SaveObjectsAsync(List<T> objects)
        {
            // Placeholder for the method to save objects to the database
            return Task.CompletedTask;
        }
        public Task DeleteObjectAsync(T obj)
        {
            // Placeholder for the method to delete an object from the database
            return Task.CompletedTask;
        }
        public Task UpdateObjectAsync(T obj)
        {
            // Placeholder for the method to update an object in the database
            return Task.CompletedTask;
        }
        public Task<T> GetObjectByIdAsync(int id)
        {
            // Placeholder for the method to get an object by ID from the database
            return Task.FromResult(default(T));
        }

    }
}
