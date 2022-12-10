
using System.ComponentModel.DataAnnotations;

namespace cw_db.Models
{
    public class Product
    {

        public Product()
        {
            Orders = new List<Order>();
        }
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        public List<Order>? Orders { get; set; }

        public string Category { get; set; }
    }
}