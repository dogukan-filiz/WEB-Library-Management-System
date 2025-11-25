using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_Library_Management_System.Models
{
    [Table("SeatReservations")]
    public class SeatReservation
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [Column("SeatId")]
        public int SeatId { get; set; }

        [Column("ReservationDate")]
        public DateTime ReservationDate { get; set; } = DateTime.UtcNow;

        [Column("StartTime")]
        public DateTime StartTime { get; set; }

        [Column("EndTime")]
        public DateTime EndTime { get; set; }

        [Column("Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active, Completed, Cancelled

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("SeatId")]
        public Seat? Seat { get; set; }
    }
}
