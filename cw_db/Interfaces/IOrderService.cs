namespace cw_db.Interfaces
{
    public interface IOrderService
    {
        Task Add(Order order);

        Task AddProductToOrder(int id, Product product);

        Task<IEnumerable<Order>> GetAll();

        Task<Order> Get(int id);

        Task Edit(int id, Order order);

        Task Delete(int id);
    }
}
