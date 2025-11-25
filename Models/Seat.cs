using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Library_Management_System.Models
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("SeatNumber")]
        public string SeatNumber { get; set; } = string.Empty;

        [Column("Floor")]
        public int Floor { get; set; }

        [Column("Section")]
        public string? Section { get; set; }

        [Column("IsAvailable")]
        public bool IsAvailable { get; set; } = true;

        [Column("Type")]
        public string? Type { get; set; }

        // Navigation property
        public ICollection<SeatReservation>? SeatReservations { get; set; }
    }
}
