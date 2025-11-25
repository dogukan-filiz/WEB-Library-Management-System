using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Library_Management_System.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Column("Author")]
        [StringLength(100)]
        public string? Author { get; set; }

        [Column("ISBN")]
        [StringLength(20)]
        public string? ISBN { get; set; }

        [Column("PublishDate")]
        public DateTime? PublishDate { get; set; }

        [Column("Publisher")]
        [StringLength(100)]
        public string? Publisher { get; set; }

        [Column("Category")]
        [StringLength(50)]
        public string? Category { get; set; }

        [Column("PageCount")]
        public int? PageCount { get; set; }

        [Column("Description")]
        [StringLength(1000)]
        public string? Description { get; set; }

        [Column("CoverImageUrl")]
        public string? CoverImageUrl { get; set; }

        [Column("TotalCopies")]
        public int TotalCopies { get; set; } = 1;

        [Column("AvailableCopies")]
        public int AvailableCopies { get; set; } = 1;

        [Column("IsAvailable")]
        public bool IsAvailable { get; set; } = true;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ICollection<BookRental>? BookRentals { get; set; }
    }
}
