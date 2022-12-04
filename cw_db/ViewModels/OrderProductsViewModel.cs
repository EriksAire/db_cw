namespace cw_db.ViewModels
{
    //TODO: Useslss ATM
    public class OrderProductsViewModel
    {
        public Order Order { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
