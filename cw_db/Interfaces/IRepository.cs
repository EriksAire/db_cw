using cw_db.Data;
using System.Linq.Expressions;

namespace cw_db.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(string id);

        Task<T> GetByIdAsync(int id);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T item);

        void Update(T item);

        void SetValues(T OldItem, T NewItem);

        void Delete(T item);

        Task SaveChangesAsync();

        ApplicationDbContext GetContext();
    }
}
