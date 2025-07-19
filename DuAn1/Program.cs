using DuAn1.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình DbContext để kết nối với cơ sở dữ liệu
builder.Services.AddDbContext<Duan1Context>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Cấu hình lưu trữ session trong bộ nhớ
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Thời gian hết hạn session (30 phút)
    options.Cookie.HttpOnly = true;  // Bảo mật hơn, chỉ có thể truy cập cookie từ phía server
    options.Cookie.IsEssential = true;  // Đảm bảo cookie này sẽ được lưu trữ trong mọi trường hợp
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sử dụng session
app.UseSession();  // Phải gọi UseSession trước UseAuthorization

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangBanSanPhams}/{action=Index}/{id?}");

app.Run();
// Tạo tài khoản admin gốc nếu chưa có
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<Duan1Context>();

    // Kiểm tra xem đã có tài khoản admin chưa (tránh load cả bảng)
    var exists = context.QuanLies.Any(q => q.Username == "admin");
    if (!exists)
    {
        var admin = new QuanLy
        {
            MaQuanLy = "QL001",
            TenQuanLy = "Admin hệ thống",
            NgaySinh = new DateOnly(1980, 1, 1),
            DiaChi = "Hà Nội",
            GioiTinh = "Nam",
            TrangThai = "Hoạt động",
            Username = "admin",
            Password = "admin123" // Sau nên hash mật khẩu
        };

        context.QuanLies.Add(admin);
        context.SaveChanges();
    }
}
