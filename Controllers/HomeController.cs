using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Data;
using WEB_Library_Management_System.Models;

namespace WEB_Library_Management_System.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly LibraryDbContext _context;

    public HomeController(ILogger<HomeController> logger, LibraryDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Gerçek istatistikleri veritabanından al
        ViewBag.TotalBooks = await _context.Books.CountAsync();
        ViewBag.TotalSeats = await _context.Seats.CountAsync();
        ViewBag.TotalUsers = await _context.Users.CountAsync();
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
