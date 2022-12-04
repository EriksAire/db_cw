using System;
using System.ComponentModel.DataAnnotations;

namespace cw_db
{
    public class Order
    {
        public Order()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime CompletionDate { get; set; }

        public List<Product>? Products { get; set; }
    }
}