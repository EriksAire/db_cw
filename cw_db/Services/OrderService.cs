using cw_db.Interfaces;

namespace cw_db.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Add(Order order)
        {
            unitOfWork.Repo<Order>().Add(order);

            await unitOfWork.Repo<Order>().SaveChangesAsync();
        }

        public async Task AddProductToOrder(int id, Product product)
        {
            var order = await unitOfWork.Repo<Order>().GetByIdAsync(id);

            if(order.Products != null)
            {
                order.Products.Add(product);
            }

            await unitOfWork.Repo<Order>().SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var order = await unitOfWork.Repo<Order>().GetByIdAsync(id);

            unitOfWork.Repo<Order>().Delete(order);

            await unitOfWork.Repo<Order>().SaveChangesAsync();
        }

        public async Task Edit(int id, Order Order)
        {
            var tmpOrder = await unitOfWork.Repo<Order>().GetByIdAsync(id);

            unitOfWork.Repo<Order>().SetValues(tmpOrder, Order);
            await unitOfWork.Repo<Order>().SaveChangesAsync();
        }

        public async Task<Order> Get(int id)
        {
            return await unitOfWork.Repo<Order>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await unitOfWork.Repo<Order>().GetAllAsync();
        }
    }
}
