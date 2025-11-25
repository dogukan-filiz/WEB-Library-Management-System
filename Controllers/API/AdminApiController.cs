using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Data;
using WEB_Library_Management_System.Models;

namespace WEB_Library_Management_System.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public AdminApiController(LibraryDbContext context)
        {
            _context = context;
        }

        // Admin kontrolü
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        // POST: api/AdminApi/toggle-user-status
        [HttpPost("toggle-user-status")]
        public async Task<ActionResult> ToggleUserStatus([FromBody] ToggleUserStatusRequest request)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Admin kullanıcıları değiştirilemez
            if (user.Role == "Admin")
            {
                return BadRequest(new { message = "Admin kullanıcıları değiştirilemez." });
            }

            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = user.IsActive ? "Kullanıcı aktif edildi." : "Kullanıcı pasif edildi.",
                isActive = user.IsActive
            });
        }

        // DELETE: api/AdminApi/delete-user/{id}
        [HttpDelete("delete-user/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı bulunamadı." });
            }

            // Admin kullanıcıları silinemez
            if (user.Role == "Admin")
            {
                return BadRequest(new { message = "Admin kullanıcıları silinemez." });
            }

            // Aktif kiralaması veya rezervasyonu var mı kontrol et
            var hasActiveRentals = await _context.BookRentals
                .AnyAsync(r => r.UserId == id && r.Status == "Active");

            var hasActiveReservations = await _context.SeatReservations
                .AnyAsync(r => r.UserId == id && r.Status == "Active");

            if (hasActiveRentals || hasActiveReservations)
            {
                return BadRequest(new { message = "Kullanıcının aktif kiralaması veya rezervasyonu var. Önce bunları sonlandırın." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı başarıyla silindi." });
        }

        // POST: api/AdminApi/add-book
        [HttpPost("add-book")]
        public async Task<ActionResult> AddBook([FromBody] BookRequest request)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            // PublishDate string'i DateTime'a çevir
            DateTime? publishDate = null;
            if (!string.IsNullOrEmpty(request.PublishDate))
            {
                if (DateTime.TryParse(request.PublishDate, out DateTime parsedDate))
                {
                    publishDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                }
            }

            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                Category = request.Category,
                Publisher = request.Publisher,
                PublishDate = publishDate,
                PageCount = request.PageCount,
                Description = request.Description,
                TotalCopies = request.TotalCopies,
                AvailableCopies = request.TotalCopies,
                IsAvailable = request.TotalCopies > 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Kitap başarıyla eklendi.",
                bookId = book.Id
            });
        }

        // PUT: api/AdminApi/update-book/{id}
        [HttpPut("update-book/{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromBody] BookRequest request)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            // Mevcut AvailableCopies ile TotalCopies arasındaki farkı hesapla
            int rentedCopies = book.TotalCopies - book.AvailableCopies;

            // Yeni TotalCopies, kiralanan kopya sayısından az olamaz
            if (request.TotalCopies < rentedCopies)
            {
                return BadRequest(new
                {
                    message = $"Toplam kopya sayısı en az {rentedCopies} olmalıdır. (Şu anda {rentedCopies} kopya kiralanmış durumda.)"
                });
            }

            // PublishDate string'i DateTime'a çevir
            DateTime? publishDate = null;
            if (!string.IsNullOrEmpty(request.PublishDate))
            {
                if (DateTime.TryParse(request.PublishDate, out DateTime parsedDate))
                {
                    publishDate = DateTime.SpecifyKind(parsedDate, DateTimeKind.Utc);
                }
            }

            book.Title = request.Title;
            book.Author = request.Author;
            book.ISBN = request.ISBN;
            book.Category = request.Category;
            book.Publisher = request.Publisher;
            book.PublishDate = publishDate;
            book.PageCount = request.PageCount;
            book.Description = request.Description;
            book.TotalCopies = request.TotalCopies;
            book.AvailableCopies = request.TotalCopies - rentedCopies;
            book.IsAvailable = book.AvailableCopies > 0;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Kitap başarıyla güncellendi.",
                bookId = book.Id
            });
        }

        // GET: api/AdminApi/get-book/{id}
        [HttpGet("get-book/{id}")]
        public async Task<ActionResult> GetBook(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            return Ok(new
            {
                id = book.Id,
                title = book.Title,
                author = book.Author,
                isbn = book.ISBN,
                category = book.Category,
                publisher = book.Publisher,
                publishDate = book.PublishDate?.ToString("yyyy-MM-dd"),
                pageCount = book.PageCount,
                description = book.Description,
                totalCopies = book.TotalCopies,
                availableCopies = book.AvailableCopies
            });
        }

        // DELETE: api/AdminApi/delete-book/{id}
        [HttpDelete("delete-book/{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            if (!IsAdmin())
            {
                return Unauthorized(new { message = "Bu işlem için yetkiniz yok." });
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Kitap bulunamadı." });
            }

            // Aktif kiralamada olan kitap silinemez
            var hasActiveRentals = await _context.BookRentals
                .AnyAsync(r => r.BookId == id && r.Status == "Active");

            if (hasActiveRentals)
            {
                return BadRequest(new { message = "Bu kitabın aktif kiralaması var. Önce tüm kiralamaları sonlandırın." });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kitap başarıyla silindi." });
        }
    }

    // Request DTOs
    public class ToggleUserStatusRequest
    {
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }

    public class BookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? Category { get; set; }
        public string? Publisher { get; set; }
        public string? PublishDate { get; set; }
        public int? PageCount { get; set; }
        public string? Description { get; set; }
        public int TotalCopies { get; set; }
    }
}
