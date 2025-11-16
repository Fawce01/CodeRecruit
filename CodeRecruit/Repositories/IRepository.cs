using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CodeRecruit.Repositories
{
    public interface IRepository<T> where T: class
    {
        // This interface should be of a class type - cannot be primitive (i.e, int, string)
        // CRUD operations

        // Create
        Task AddAsync(T entity);
        // Read
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAllAsync(int id);
        // Update
        Task UpdateAsync(T entity);
        // Delete
        Task DeleteAsync(int id);
    }
}
