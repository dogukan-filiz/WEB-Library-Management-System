using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Data;
using WEB_Library_Management_System.Models;

namespace WEB_Library_Management_System.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksApiController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksApiController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/BooksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] string? search = null)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => 
                    b.Title.Contains(search) || 
                    (b.Author != null && b.Author.Contains(search)) ||
                    (b.Category != null && b.Category.Contains(search)));
            }

            var books = await query.OrderBy(b => b.Title).ToListAsync();
            return Ok(books);
        }

        // GET: api/BooksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            return Ok(book);
        }

        // POST: api/BooksApi/rent
        [HttpPost("rent")]
        public async Task<ActionResult<BookRental>> RentBook([FromBody] RentBookRequest request)
        {
            // Kitabı bul
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            // Kullanıcıyı bul
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Kullanıcının aktif kiralama sayısını kontrol et (maksimum 3)
            var activeRentalCount = await _context.BookRentals
                .CountAsync(r => r.UserId == request.UserId && r.Status == "Active");

            if (activeRentalCount >= 3)
            {
                return BadRequest(new { message = "Maksimum 3 kitap kiralayabilirsiniz. Önce mevcut kitaplarınızdan birini iade edin." });
            }

            // Kullanıcının bu kitabı zaten aktif olarak kiralamış mı kontrol et
            var existingRental = await _context.BookRentals
                .FirstOrDefaultAsync(r => r.UserId == request.UserId && 
                                         r.BookId == request.BookId && 
                                         r.Status == "Active");

            if (existingRental != null)
            {
                return BadRequest(new { message = "Bu kitabı zaten kiralamışsınız. Önce iade etmelisiniz." });
            }

            // Müsait kopya var mı kontrol et
            if (book.AvailableCopies <= 0)
            {
                return BadRequest(new { message = "Bu kitabın müsait kopyası yok." });
            }

            // Kiralama kaydı oluştur
            var rental = new BookRental
            {
                UserId = request.UserId,
                BookId = request.BookId,
                RentalDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14), // 14 gün kiralama süresi
                Status = "Active"
            };

            _context.BookRentals.Add(rental);

            // Müsait kopya sayısını azalt
            book.AvailableCopies--;
            
            // IsAvailable durumunu güncelle
            if (book.AvailableCopies == 0)
            {
                book.IsAvailable = false;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Kitap başarıyla kiralandı!",
                rentalId = rental.Id,
                dueDate = rental.DueDate.ToString("dd.MM.yyyy")
            });
        }

        // POST: api/BooksApi/return
        [HttpPost("return")]
        public async Task<ActionResult> ReturnBook([FromBody] ReturnBookRequest request)
        {
            var rental = await _context.BookRentals
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == request.RentalId && r.Status == "Active");

            if (rental == null)
            {
                return NotFound(new { message = "Aktif kiralama kaydı bulunamadı." });
            }

            // İade işlemi
            rental.ReturnDate = DateTime.UtcNow;
            rental.Status = "Returned";

            // Müsait kopya sayısını artır
            if (rental.Book != null)
            {
                rental.Book.AvailableCopies++;
                
                // IsAvailable durumunu güncelle
                if (rental.Book.AvailableCopies > 0)
                {
                    rental.Book.IsAvailable = true;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Kitap başarıyla iade edildi.",
                rentalId = rental.Id
            });
        }

        // GET: api/BooksApi/rentals/user/{userId}
        [HttpGet("rentals/user/{userId}")]
        public async Task<ActionResult> GetUserRentals(int userId)
        {
            var rentals = await _context.BookRentals
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();

            // JSON döngüsünü önlemek için sadece gerekli alanları döndür
            var result = rentals.Select(r => new
            {
                id = r.Id,
                userId = r.UserId,
                bookId = r.BookId,
                rentalDate = r.RentalDate,
                dueDate = r.DueDate,
                returnDate = r.ReturnDate,
                status = r.Status,
                fine = r.Fine,
                book = new
                {
                    id = r.Book?.Id,
                    title = r.Book?.Title,
                    author = r.Book?.Author,
                    isbn = r.Book?.ISBN,
                    availableCopies = r.Book?.AvailableCopies
                }
            });

            return Ok(result);
        }

        // DELETE: api/BooksApi/clear-history
        [HttpDelete("clear-history")]
        public async Task<ActionResult> ClearHistory([FromBody] ClearHistoryRequest request)
        {
            try
            {
                // Sadece "Returned" durumundaki kiralamaları sil
                var oldRentals = await _context.BookRentals
                    .Where(r => r.UserId == request.UserId && r.Status == "Returned")
                    .ToListAsync();

                if (!oldRentals.Any())
                {
                    return Ok(new { message = "Temizlenecek geçmiş kayıt bulunmamaktadır." });
                }

                _context.BookRentals.RemoveRange(oldRentals);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"{oldRentals.Count} adet geçmiş kayıt başarıyla temizlendi.",
                    deletedCount = oldRentals.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Geçmiş temizlenirken bir hata oluştu: " + ex.Message });
            }
        }
    }

    // Request DTOs
    public class RentBookRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }

    public class ReturnBookRequest
    {
        public int RentalId { get; set; }
    }

    public class ClearHistoryRequest
    {
        public int UserId { get; set; }
    }
}
