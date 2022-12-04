
using System.ComponentModel.DataAnnotations;

namespace cw_db.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Content { get; set; } = string.Empty;

        [Range(0, 5, ErrorMessage = "Please enter correct value")]
        public int Rating { get; set; }

        public DateTime date { get; set; } = DateTime.Now;
    }
}