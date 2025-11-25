using Microsoft.EntityFrameworkCore;
using WEB_Library_Management_System.Models;

namespace WEB_Library_Management_System.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BookRental> BookRentals { get; set; }
        public DbSet<SeatReservation> SeatReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PostgreSQL tablo isimlerini veritabanındaki gibi yapılandır
            modelBuilder.Entity<Book>().ToTable("Books");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Seat>().ToTable("Seats");
            modelBuilder.Entity<BookRental>().ToTable("BookRentals");
            modelBuilder.Entity<SeatReservation>().ToTable("SeatReservations");
        }
    }
}
