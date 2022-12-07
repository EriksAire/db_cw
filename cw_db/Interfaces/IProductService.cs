using cw_db.Models;

namespace cw_db.Interfaces
{
    public interface IProductService
    {
        Task Add(Product product);

        Task<IEnumerable<Product>> GetAll();

        Task<Product> Get(int id);

        Task Edit(int id, Product product);

        Task Delete(int id);
    }
}
