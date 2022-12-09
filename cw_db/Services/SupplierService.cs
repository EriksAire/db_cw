using cw_db.Interfaces;
using cw_db.Models;

namespace cw_db.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Add(Supplier supplier)
        {
            unitOfWork.Repo<Supplier>().Add(supplier);

            await unitOfWork.Repo<Supplier>().SaveChangesAsync();
        }

        public async Task AddOrderToSupplier(int id, Order order)
        {
            var supplier = await unitOfWork.Repo<Supplier>().GetByIdAsync(id);

            if (supplier.Orders != null)
            {
                supplier.Orders.Add(order);
            }

            await unitOfWork.Repo<Supplier>().SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await unitOfWork.Repo<Supplier>().GetByIdAsync(id);

            unitOfWork.Repo<Supplier>().Delete(order);

            await unitOfWork.Repo<Supplier>().SaveChangesAsync();
        }

        public async Task Edit(int id, Supplier supplier)
        {
            var tmpSupplier = await unitOfWork.Repo<Supplier>().GetByIdAsync(id);

            unitOfWork.Repo<Supplier>().SetValues(tmpSupplier, supplier);
            await unitOfWork.Repo<Supplier>().SaveChangesAsync();
        }

        public async Task<Supplier> Get(int id)
        {
            return await unitOfWork.Repo<Supplier>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await unitOfWork.Repo<Supplier>().GetAllAsync();
        }
    }
}
