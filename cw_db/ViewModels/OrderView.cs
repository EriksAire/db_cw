using cw_db.Models;
using System.ComponentModel.DataAnnotations;

namespace cw_db.ViewModels
{
    public class OrderView
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string CustomerId { get; set; }
    }
}
