using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Data;
using WEB_Library_Management_System.Models;

namespace WEB_Library_Management_System.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsApiController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public SeatsApiController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/SeatsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSeats()
        {
            var seats = await _context.Seats
                .OrderBy(s => s.Floor)
                .ThenBy(s => s.SeatNumber)
                .Select(s => new
                {
                    id = s.Id,
                    seatNumber = s.SeatNumber,
                    floor = s.Floor,
                    section = s.Section,
                    type = s.Type,
                    isAvailable = s.IsAvailable
                })
                .ToListAsync();

            return Ok(seats);
        }

        // GET: api/SeatsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seat>> GetSeat(int id)
        {
            var seat = await _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return seat;
        }

        // POST: api/SeatsApi/reserve
        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatRequest request)
        {
            var userId = request.UserId;

            // Kullanıcının aktif rezervasyonu var mı kontrol et
            var hasActiveReservation = await _context.SeatReservations
                .AnyAsync(r => r.UserId == userId && r.Status == "Active");

            if (hasActiveReservation)
            {
                return BadRequest(new { message = "Zaten aktif bir rezervasyonunuz bulunmaktadır. Önce mevcut rezervasyonunuzu iptal etmelisiniz." });
            }

            // Koltuğu kontrol et
            var seat = await _context.Seats.FindAsync(request.SeatId);
            if (seat == null)
            {
                return NotFound(new { message = "Oturma yeri bulunamadı." });
            }

            if (!seat.IsAvailable)
            {
                return BadRequest(new { message = "Bu oturma yeri şu anda müsait değil." });
            }

            // Rezervasyon oluştur
            var reservation = new SeatReservation
            {
                UserId = userId,
                SeatId = request.SeatId,
                ReservationDate = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
                StartTime = DateTime.SpecifyKind(request.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(request.EndTime, DateTimeKind.Utc),
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            _context.SeatReservations.Add(reservation);
            
            // Koltuğu müsait değil olarak işaretle
            seat.IsAvailable = false;
            
            await _context.SaveChangesAsync();

            return Ok(new { message = "Oturma yeri başarıyla rezerve edildi!" });
        }

        // POST: api/SeatsApi/cancel
        [HttpPost("cancel")]
        public async Task<IActionResult> CancelReservation([FromBody] CancelReservationRequest request)
        {
            var reservation = await _context.SeatReservations
                .Include(r => r.Seat)
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId);

            if (reservation == null)
            {
                return NotFound(new { message = "Rezervasyon bulunamadı." });
            }

            // Rezervasyonu iptal et
            reservation.Status = "Cancelled";
            
            // Koltuğu tekrar müsait yap
            if (reservation.Seat != null)
            {
                reservation.Seat.IsAvailable = true;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Rezervasyon iptal edildi." });
        }

        // GET: api/SeatsApi/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserReservations(int userId)
        {
            var reservations = await _context.SeatReservations
                .Include(r => r.Seat)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.ReservationDate)
                .Select(r => new
                {
                    id = r.Id,
                    seatNumber = r.Seat!.SeatNumber,
                    floor = r.Seat.Floor,
                    date = r.ReservationDate,
                    startTime = r.StartTime,
                    endTime = r.EndTime,
                    status = r.Status
                })
                .ToListAsync();

            return Ok(reservations);
        }

        // POST: api/SeatsApi/delete-old
        [HttpPost("delete-old")]
        public async Task<IActionResult> DeleteOldReservations([FromBody] DeleteOldReservationsRequest request)
        {
            // Kullanıcının tamamlanmış veya iptal edilmiş rezervasyonlarını sil
            var oldReservations = await _context.SeatReservations
                .Where(r => r.UserId == request.UserId && 
                           (r.Status == "Completed" || r.Status == "Cancelled"))
                .ToListAsync();

            if (oldReservations.Count == 0)
            {
                return NotFound(new { message = "Silinecek geçmiş rezervasyon bulunamadı." });
            }

            _context.SeatReservations.RemoveRange(oldReservations);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"{oldReservations.Count} adet geçmiş rezervasyon silindi." });
        }
    }

    public class ReserveSeatRequest
    {
        public int UserId { get; set; }
        public int SeatId { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class CancelReservationRequest
    {
        public int ReservationId { get; set; }
    }

    public class DeleteOldReservationsRequest
    {
        public int UserId { get; set; }
    }
}
