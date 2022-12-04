using System;
using System.ComponentModel.DataAnnotations;

namespace cw_db.Models
{
    public class Supplier
    {
        public Supplier()
        {
            Orders = new List<Order>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;

        public List<Order>? Orders { get; set; }
    }
}