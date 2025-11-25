using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Library_Management_System.Models
{
    [Table("BookRentals")]
    public class BookRental
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [Column("BookId")]
        public int BookId { get; set; }

        [Column("RentalDate")]
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;

        [Column("DueDate")]
        public DateTime DueDate { get; set; }

        [Column("ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        [Column("Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Returned, Overdue

        [Column("Fine")]
        public decimal? Fine { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("BookId")]
        public Book? Book { get; set; }
    }
}
