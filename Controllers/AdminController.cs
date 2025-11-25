using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Data;

namespace WEB_Library_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly LibraryDbContext _context;

        public AdminController(LibraryDbContext context)
        {
            _context = context;
        }

        // Admin kontrolü
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return role == "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            // İstatistikler
            ViewBag.TotalBooks = await _context.Books.CountAsync();
            ViewBag.TotalUsers = await _context.Users.CountAsync();
            ViewBag.ActiveRentals = await _context.BookRentals.CountAsync(r => r.Status == "Active");

            return View();
        }

        // Kullanıcı Yönetimi
        public async Task<IActionResult> Users()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        // Kitap Yönetimi
        public async Task<IActionResult> Books()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var books = await _context.Books
                .OrderBy(b => b.Title)
                .ToListAsync();

            return View(books);
        }

        // Kiralama Yönetimi
        public async Task<IActionResult> Rentals()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var rentals = await _context.BookRentals
                .Include(r => r.User)
                .Include(r => r.Book)
                .OrderByDescending(r => r.RentalDate)
                .ToListAsync();

            return View(rentals);
        }

        // Rezervasyon Yönetimi
        public async Task<IActionResult> Reservations()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var reservations = await _context.SeatReservations
                .Include(r => r.User)
                .Include(r => r.Seat)
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();

            return View(reservations);
        }
    }
}
