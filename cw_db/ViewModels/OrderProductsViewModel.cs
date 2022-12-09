using cw_db.Models;

namespace cw_db.ViewModels
{
    public class OrderProductsViewModel
    {
        public Order Order { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
