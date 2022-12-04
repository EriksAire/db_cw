using cw_db.Interfaces;

namespace cw_db.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Add(Product product)
        {
            unitOfWork.Repo<Product>().Add(product);

            await unitOfWork.Repo<Product>().SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await unitOfWork.Repo<Product>().GetByIdAsync(id);

            unitOfWork.Repo<Product>().Delete(product);

            await unitOfWork.Repo<Product>().SaveChangesAsync();
        }

        public async Task Edit(int id, Product product)
        {
            var tmpProduct = await unitOfWork.Repo<Product>().GetByIdAsync(id);

            unitOfWork.Repo<Product>().SetValues(tmpProduct, product);
            await unitOfWork.Repo<Product>().SaveChangesAsync();
        }

        public async Task<Product> Get(int id)
        {
            return await unitOfWork.Repo<Product>().GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await unitOfWork.Repo<Product>().GetAllAsync();
        }
    }
}
