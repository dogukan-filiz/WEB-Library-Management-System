using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Library_Management_System.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("Email")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Column("FirstName")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Column("LastName")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Column("PhoneNumber")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Column("Role")]
        [StringLength(20)]
        public string Role { get; set; } = "User";

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<BookRental>? BookRentals { get; set; }
        public ICollection<SeatReservation>? SeatReservations { get; set; }
    }
}
