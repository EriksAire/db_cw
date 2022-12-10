using cw_db.Models;

namespace cw_db.ViewModels
{
    public class OrderProductsViewModel
    {
        public int Id { get; set; }
        public Order Order { get; set; }

        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }
    }
}
