# 📚 Kütüphane Yönetim Sistemi (Library Management System)

Modern ve kullanıcı dostu bir web tabanlı kütüphane yönetim sistemi. ASP.NET Core MVC ile geliştirilmiş, PostgreSQL veritabanı kullanan full-stack bir uygulama.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-blue)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## 🎯 Proje Özeti

Bu proje, kütüphane işlemlerini dijitalleştiren, kitap kiralama, oturma yeri rezervasyonu ve admin yönetim paneli içeren kapsamlı bir web uygulamasıdır. Öğrenciler kitap kiralayabilir, oturma yeri rezerve edebilir; yöneticiler ise tüm sistem üzerinde kontrol sahibi olabilir.

## ✨ Özellikler

### 👥 Kullanıcı Özellikleri
- ✅ **Güvenli Giriş Sistemi**: Email ve şifre ile oturum yönetimi
- ✅ **Kitap Kiralama**: Maksimum 3 kitap kiralama limiti
- ✅ **Kitap Arama**: Kitap adı, yazar veya kategoriye göre arama
- ✅ **Kitap İade**: Kolay iade işlemi ve geçmiş temizleme
- ✅ **Oturma Yeri Rezervasyonu**: 60 koltuk (3 kat × 20 koltuk)
- ✅ **Rezervasyon Yönetimi**: Aktif rezervasyonları görüntüleme ve iptal etme
- ✅ **Profil Sayfası**: Kiralama ve rezervasyon geçmişi
- ✅ **Responsive Tasarım**: Mobil, tablet ve masaüstü uyumlu

### 👨‍💼 Admin Özellikleri
- ✅ **Dashboard**: Anlık istatistikler (toplam kitap, kullanıcı, aktif kiralama)
- ✅ **Kullanıcı Yönetimi**: Kullanıcıları aktif/pasif yapma, silme
- ✅ **Kitap Yönetimi**: CRUD işlemleri (Ekleme, Güncelleme, Silme)
- ✅ **Kiralama Yönetimi**: Tüm kiralamaları görüntüleme, manuel iade
- ✅ **Rezervasyon Yönetimi**: Filtreleme, iptal etme, geçmiş temizleme
- ✅ **Güvenlik**: Admin kullanıcıları korumalı, aktif işlemler kontrollü

### 🔧 Teknik Özellikler
- ✅ **RESTful API**: Modern API mimarisi
- ✅ **Swagger/OpenAPI**: API dokümantasyonu (`/api-docs`)
- ✅ **Session Yönetimi**: Güvenli oturum kontrolü
- ✅ **Entity Framework Core**: Code-First yaklaşım
- ✅ **AJAX**: Sayfa yenilemeden dinamik güncellemeler
- ✅ **Scroll Özelliği**: Uzun listelerde kaydırma desteği
- ✅ **Sticky Footer**: Her zaman sayfanın altında kalan footer

## 🛠️ Kullanılan Teknolojiler

### Backend
- **ASP.NET Core MVC 9.0**: Web framework
- **Entity Framework Core 9.0**: ORM
- **PostgreSQL 16**: Veritabanı
- **Npgsql 9.0.4**: PostgreSQL provider
- **Swashbuckle.AspNetCore 10.0.1**: Swagger/OpenAPI

### Frontend
- **Bootstrap 5**: UI framework
- **Font Awesome 6**: İkonlar
- **JavaScript (ES6+)**: Dinamik işlemler
- **HTML5 & CSS3**: Yapı ve stil

### Geliştirme Araçları
- **Visual Studio Code**: IDE
- **Git & GitHub**: Versiyon kontrolü
- **.NET SDK 9.0**: Development kit

## 📋 Ön Gereksinimler

Projeyi çalıştırmadan önce sisteminizde aşağıdaki araçların yüklü olması gerekmektedir:

1. **.NET SDK 9.0 veya üzeri**
   ```bash
   # Versiyon kontrolü
   dotnet --version
   ```

2. **PostgreSQL 16**
   - Port: 5433 (varsayılan)
   - Database: librarydb
   - Username: libraryuser
   - Password: 123456

3. **Git**
   ```bash
   # Versiyon kontrolü
   git --version
   ```

## 🚀 Kurulum

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/dogukan-filiz/WEB-Library-Management-System.git
cd WEB-Library-Management-System
```

### 2. PostgreSQL Veritabanını Oluşturun
```sql
-- PostgreSQL'e bağlanın
psql -U postgres

-- Kullanıcı oluşturun
CREATE USER libraryuser WITH PASSWORD '123456';

-- Veritabanı oluşturun
CREATE DATABASE librarydb OWNER libraryuser;

-- Yetki verin
GRANT ALL PRIVILEGES ON DATABASE librarydb TO libraryuser;
```

### 3. Bağlantı Ayarlarını Yapılandırın
`appsettings.json` dosyasını kontrol edin:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=librarydb;Username=libraryuser;Password=123456"
  }
}
```

> **Not**: Farklı port veya kimlik bilgileri kullanıyorsanız bu ayarları güncelleyin.

### 4. Veritabanını Migrate Edin
```bash
# Migration oluşturun (ilk kez)
dotnet ef migrations add InitialCreate

# Veritabanını güncelleyin
dotnet ef database update
```

### 5. Seed Data Ekleyin (Opsiyonel)
Veritabanına örnek veriler eklemek için:
```sql
-- Test kullanıcıları
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password", "Role", "IsActive", "CreatedAt")
VALUES 
('Admin', 'User', 'admin@library.com', 'Admin123!', 'Admin', true, NOW()),
('Test', 'User', 'user@library.com', 'User123!', 'Student', true, NOW());

-- Örnek kitaplar
INSERT INTO "Books" ("Title", "Author", "Category", "TotalCopies", "AvailableCopies", "IsAvailable", "CreatedAt")
VALUES 
('1984', 'George Orwell', 'Distopya', 5, 5, true, NOW()),
('Suç ve Ceza', 'Fyodor Dostoyevski', 'Roman', 3, 3, true, NOW());

-- Oturma yerleri (60 koltuk)
-- Bu sorgu otomatik olarak çalışır (veya manuel eklenebilir)
```

### 6. Projeyi Çalıştırın
```bash
dotnet run
```

Uygulama başarıyla başladığında:
```
Now listening on: http://localhost:5297
```

## 🌐 Erişim Adresleri

### Ana Uygulama
- **Ana Sayfa**: http://localhost:5297
- **Giriş Sayfası**: http://localhost:5297/Account/Login
- **Kitaplar**: http://localhost:5297/Books
- **Oturma Yerleri**: http://localhost:5297/Seats
- **Profil**: http://localhost:5297/Account/Profile
- **Admin Paneli**: http://localhost:5297/Admin

### API Dokümantasyonu
- **Swagger UI**: http://localhost:5297/api-docs
- **OpenAPI JSON**: http://localhost:5297/swagger/v1/swagger.json

## 👤 Test Hesapları

### Admin Hesabı
- **Email**: `admin@library.com`
- **Şifre**: `Admin123!`
- **Yetkiler**: Tüm admin özellikleri

### Öğrenci Hesabı
- **Email**: `user@library.com`
- **Şifre**: `User123!`
- **Yetkiler**: Kitap kiralama, oturma yeri rezervasyonu

## 📁 Proje Yapısı

```
WEB-Library-Management-System/
├── Controllers/
│   ├── AccountController.cs        # Giriş/Çıkış işlemleri
│   ├── AdminController.cs          # Admin panel
│   ├── BooksController.cs          # Kitap sayfaları
│   ├── HomeController.cs           # Ana sayfa
│   ├── SeatsController.cs          # Oturma yerleri
│   └── API/
│       ├── AdminApiController.cs   # Admin API'leri
│       ├── BooksApiController.cs   # Kitap API'leri
│       └── SeatsApiController.cs   # Koltuk API'leri
├── Models/
│   ├── User.cs                     # Kullanıcı modeli
│   ├── Book.cs                     # Kitap modeli
│   ├── BookRental.cs               # Kiralama modeli
│   ├── Seat.cs                     # Koltuk modeli
│   └── SeatReservation.cs          # Rezervasyon modeli
├── Data/
│   └── LibraryDbContext.cs         # EF Core DbContext
├── Views/
│   ├── Account/                    # Giriş/Profil view'ları
│   ├── Admin/                      # Admin panel view'ları
│   ├── Books/                      # Kitap view'ları
│   ├── Home/                       # Ana sayfa view'ları
│   ├── Seats/                      # Oturma yeri view'ları
│   └── Shared/
│       └── _Layout.cshtml          # Master layout
├── wwwroot/
│   ├── css/
│   │   ├── custom.css              # Özel stiller
│   │   └── site.css                # Genel stiller
│   └── js/
│       └── site.js                 # Genel JavaScript
├── Program.cs                      # Uygulama giriş noktası
├── appsettings.json                # Ayarlar
└── README.md                       # Bu dosya
```

## 🔌 API Endpoint'leri

### Authentication
- `POST /Account/Login` - Kullanıcı girişi
- `GET /Account/Logout` - Çıkış yapma

### Books API
- `GET /api/BooksApi` - Tüm kitapları listele
- `POST /api/BooksApi/rent` - Kitap kirala
- `POST /api/BooksApi/return` - Kitap iade et
- `GET /api/BooksApi/user-rentals/{userId}` - Kullanıcı kiralamaları
- `DELETE /api/BooksApi/clear-history` - Geçmişi temizle

### Seats API
- `GET /api/SeatsApi` - Tüm koltukları listele
- `GET /api/SeatsApi/user/{userId}` - Kullanıcı rezervasyonları
- `POST /api/SeatsApi/reserve` - Rezervasyon yap
- `POST /api/SeatsApi/cancel` - Rezervasyon iptal
- `POST /api/SeatsApi/delete-old` - Eski kayıtları temizle

### Admin API
- `POST /api/AdminApi/toggle-user-status` - Kullanıcı durumu değiştir
- `DELETE /api/AdminApi/delete-user/{id}` - Kullanıcı sil
- `POST /api/AdminApi/add-book` - Kitap ekle
- `PUT /api/AdminApi/update-book/{id}` - Kitap güncelle
- `GET /api/AdminApi/get-book/{id}` - Kitap detayı
- `DELETE /api/AdminApi/delete-book/{id}` - Kitap sil

> **Detaylı API dokümantasyonu için**: http://localhost:5297/api-docs

## 💡 Kullanım Örnekleri

### Kitap Kiralama (AJAX)
```javascript
const response = await fetch('/api/BooksApi/rent', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ 
        userId: 5, 
        bookId: 345 
    })
});
const result = await response.json();
console.log(result.message);
```

### Oturma Yeri Rezervasyonu
```javascript
const response = await fetch('/api/SeatsApi/reserve', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
        userId: 5,
        seatId: 12,
        startTime: "2024-11-17T14:00:00Z",
        endTime: "2024-11-17T16:00:00Z"
    })
});
```

## 🎨 Özellik Detayları

### Kitap Yönetimi
- **Maksimum 3 Kitap**: Kullanıcılar aynı anda en fazla 3 kitap kiralayabilir
- **Duplikat Kontrolü**: Aynı kitap iki kez kiralanamaz
- **Stok Takibi**: Otomatik stok güncelleme (AvailableCopies)
- **Geçmiş Temizleme**: İade edilen kitapları geçmişten temizleme

### Oturma Yeri Sistemi
- **60 Koltuk**: 3 kat × 20 koltuk (1A-01 formatında)
- **Accordion Yapısı**: Her kat için ayrı genişleyebilir bölüm
- **Renk Kodlaması**: 
  - 🟢 Yeşil: Müsait
  - 🔴 Kırmızı: Dolu
  - 🔵 Mavi: Kendi rezervasyonunuz
- **Tek Aktif Rezervasyon**: Kullanıcı aynı anda sadece 1 aktif rezervasyon yapabilir

### Admin Paneli
- **Dashboard İstatistikleri**: Anlık sistem durumu
- **Güvenli Silme**: Aktif işlemler kontrol edilir
- **Admin Koruması**: Admin kullanıcılar silinemez/pasif yapılamaz
- **Filtreleme**: Rezervasyonları duruma/kata göre filtreleme

## 🔒 Güvenlik Özellikleri

- ✅ **Session Tabanlı Authentication**: HttpContext.Session kullanımı
- ✅ **Role-Based Authorization**: Admin/Student rol kontrolü
- ✅ **SQL Injection Koruması**: Entity Framework parameterized queries
- ✅ **CSRF Koruması**: ASP.NET Core built-in protection
- ✅ **Input Validation**: Model validations ve frontend checks
- ✅ **Secure Cookies**: HttpOnly ve Essential cookies

## 📊 Veritabanı Şeması

### Users (Kullanıcılar)
| Column | Type | Description |
|--------|------|-------------|
| Id | int (PK) | Kullanıcı ID |
| FirstName | varchar | Ad |
| LastName | varchar | Soyad |
| Email | varchar | Email (unique) |
| Password | varchar | Şifre |
| PhoneNumber | varchar | Telefon |
| Role | varchar | Rol (Admin/Student) |
| IsActive | boolean | Aktif mi? |
| CreatedAt | timestamp | Oluşturulma tarihi |
| UpdatedAt | timestamp | Güncellenme tarihi |

### Books (Kitaplar)
| Column | Type | Description |
|--------|------|-------------|
| Id | int (PK) | Kitap ID |
| Title | varchar | Kitap adı |
| Author | varchar | Yazar |
| ISBN | varchar | ISBN numarası |
| Category | varchar | Kategori |
| Publisher | varchar | Yayınevi |
| PublishDate | timestamp | Yayın tarihi |
| PageCount | int | Sayfa sayısı |
| Description | text | Açıklama |
| TotalCopies | int | Toplam kopya |
| AvailableCopies | int | Müsait kopya |
| IsAvailable | boolean | Müsait mi? |
| CreatedAt | timestamp | Oluşturulma tarihi |

### BookRentals (Kiralama)
| Column | Type | Description |
|--------|------|-------------|
| Id | int (PK) | Kiralama ID |
| UserId | int (FK) | Kullanıcı ID |
| BookId | int (FK) | Kitap ID |
| RentalDate | timestamp | Kiralama tarihi |
| DueDate | timestamp | İade tarihi |
| ReturnDate | timestamp | Gerçek iade |
| Status | varchar | Durum (Active/Returned) |
| Fine | decimal | Ceza tutarı |

### Seats (Koltuklar)
| Column | Type | Description |
|--------|------|-------------|
| Id | int (PK) | Koltuk ID |
| SeatNumber | varchar | Koltuk no (1A-01) |
| Floor | int | Kat (1-3) |
| Section | varchar | Bölüm (A-C) |
| IsAvailable | boolean | Müsait mi? |

### SeatReservations (Rezervasyonlar)
| Column | Type | Description |
|--------|------|-------------|
| Id | int (PK) | Rezervasyon ID |
| UserId | int (FK) | Kullanıcı ID |
| SeatId | int (FK) | Koltuk ID |
| ReservationDate | timestamp | Rezervasyon tarihi |
| StartTime | timestamp | Başlangıç saati |
| EndTime | timestamp | Bitiş saati |
| Status | varchar | Durum (Active/Completed/Cancelled) |
| CreatedAt | timestamp | Oluşturulma tarihi |

## 🐛 Bilinen Sorunlar ve Çözümler

### Port Zaten Kullanımda
```bash
# Sorunu çözmek için:
lsof -ti:5297 | xargs kill -9
```

### Migration Hataları
```bash
# Cache'i temizle
dotnet ef database drop --force
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### PostgreSQL Bağlantı Hatası
```bash
# PostgreSQL'in çalıştığından emin olun
sudo service postgresql status

# Port kontrolü
netstat -an | grep 5433
```

## 🧪 Test Etme

### Manuel Test Senaryoları

1. **Kullanıcı Girişi**
   - Admin ve öğrenci hesapları ile giriş yapın
   - Yanlış şifre durumunu test edin

2. **Kitap İşlemleri**
   - 3 kitap kiralayın (limit kontrolü)
   - Aynı kitabı iki kez kiralamaya çalışın (duplikat kontrolü)
   - Kitap iade edin

3. **Rezervasyon İşlemleri**
   - Koltuk rezerve edin
   - İkinci rezervasyon yapmayı deneyin (bloke kontrolü)
   - Rezervasyonu iptal edin

4. **Admin İşlemleri**
   - Yeni kitap ekleyin
   - Kitap düzenleyin
   - Kullanıcıyı pasif yapın

### API Testing (Swagger)
```
http://localhost:5297/api-docs
```
adresinden tüm endpoint'leri test edebilirsiniz.

## 📈 Performans Optimizasyonları

- ✅ **AsNoTracking()**: Sadece okuma işlemlerinde
- ✅ **Eager Loading**: Include() ile ilişkili veri çekme
- ✅ **Lazy Loading Devre Dışı**: Gereksiz N+1 problemlerini önleme
- ✅ **Client-side Filtering**: JavaScript ile hızlı arama
- ✅ **Scroll Container**: Uzun listeler için max-height
- ✅ **CDN Kullanımı**: Bootstrap, jQuery, Font Awesome

## 🔄 Gelecek Güncellemeler (Roadmap)

- [ ] JWT Authentication (API için)
- [ ] Email bildirimleri (geciken iadeler için)
- [ ] QR Code ile kitap tarama
- [ ] Kitap öneri sistemi (AI tabanlı)
- [ ] Mobil uygulama (Flutter)
- [ ] Real-time bildirimler (SignalR)
- [ ] Çoklu dil desteği (i18n)
- [ ] Dark mode
- [ ] Export/Import (Excel, PDF)
- [ ] Advanced reporting

## 🤝 Katkıda Bulunma

Katkılarınızı bekliyoruz! Lütfen şu adımları izleyin:

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/AmazingFeature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some AmazingFeature'`)
4. Branch'inizi push edin (`git push origin feature/AmazingFeature`)
5. Pull Request oluşturun

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

## 👨‍💻 Geliştirici

**Doğukan Filiz**
- Email: dogukan@example.com
- GitHub: [@dogukan-filiz](https://github.com/dogukan-filiz)
- LinkedIn: [Doğukan Filiz](https://linkedin.com/in/dogukan-filiz)

## 🙏 Teşekkürler

- [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) - Web framework
- [PostgreSQL](https://www.postgresql.org/) - Veritabanı
- [Bootstrap](https://getbootstrap.com/) - UI framework
- [Font Awesome](https://fontawesome.com/) - İkonlar
- [Swagger](https://swagger.io/) - API dokümantasyonu

## 📞 İletişim

Sorularınız veya önerileriniz için:
- **Email**: dogukan@example.com
- **GitHub Issues**: [Yeni Issue Aç](https://github.com/dogukan-filiz/WEB-Library-Management-System/issues)

---

**Not**: Bu proje eğitim amaçlı geliştirilmiştir. Production ortamında kullanmadan önce güvenlik ayarlarını gözden geçirin.

**Hazırlayan**: Doğukan Filiz - Bilgisayar Mühendisliği
**Tarih**: Kasım 2025
**Versiyon**: 1.0.0

---

⭐ **Projeyi beğendiyseniz yıldız vermeyi unutmayın!** ⭐
