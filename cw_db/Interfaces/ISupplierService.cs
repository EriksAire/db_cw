using cw_db.Models;

namespace cw_db.Interfaces
{
    public interface ISupplierService
    {
        Task Add(Supplier order);

        Task AddOrderToSupplier(int id, Order order);

        Task<IEnumerable<Supplier>> GetAll();

        Task<Supplier> Get(int id);

        Task Edit(int id, Supplier order);

        Task Delete(int id);
    }
}
