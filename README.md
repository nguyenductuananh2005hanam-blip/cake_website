# Cake_Shop Web
Cake_shop Web là một ứng dụng thương mại điện tử cung cấp các loại bánh ngọt khác nhau, được xây dựng trên nền tảng Asp.net Core với kiến trúc MVC.

## 🚀 Công Nghệ Sử Dụng

*   **Backend:** Asp.net Core 10.0, C#
*   **Database:** SQL Server
*   **ORM:** Entity Framework Core 10.0.12 (Microsoft.EntityFrameworkCore.SqlServer),Code * *   First + Migrations
*   **Frontend:** HTML, CSS, Bootstrap, Razor, jQuery
*   **Build Tool:** Visual Studio MSBuild

## 📋 Yêu Cầu Môi Trường (Prerequisites)

Để chạy được dự án này, máy tính của bạn cần cài đặt sẵn:
1.  **.NET SDK 6.0 hoặc cao hơn**
2.  **Visual Studio 2022 hoặc Visual Studio Code**
3.  **SQL Server Management Studio (SSMS)** 

## ⚙️ Hướng Dẫn Cài Đặt & Chạy Chương Trình
### Bước 1: Cấu hình kết nối Database
    1. Mở file `appsettings.json` trong thư mục gốc của dự án.
    2. Tìm đến phần `"ConnectionStrings"` và thay đổi giá trị của `"DefaultConnection"` thành chuỗi kết nối của bạn, ví dụ:
    ```json
    {
        "ConnectionStrings": {
            "DefaultConnection": "Server=.\\SQLEXPRESS;Database=AnhHoaBakeryDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        }
    }
    ```
    3. Lưu file `appsettings.json`.
    4. Mở Visual Studio, nhấn chuột phải vào dự án và chọn "Manage NuGet Packages". Cài đặt các gói cần thiết nếu chưa có:
       - `Microsoft.EntityFrameworkCore.SqlServer`
       - `Microsoft.EntityFrameworkCore.Tools`
       - `Microsoft.EntityFrameworkCore.Design`
    6. Đăng ký DbContext trong `Program.cs`:
       ```csharp
        builder.Services.AddDbContext<AnhHoaBakery.Data.BakeryContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
       ```
    7. Tạo cơ sở dữ liệu
    ```
        dotnet ef database update
    ```
### Bước 3: Chạy ứng dụng
    Visual Studio: mở AnhHoaBakery.sln, chọn profile https (hoặc http) ở thanh công cụ, nhấn F5 (chạy có debug) hoặc Ctrl + F5 (chạy không debug).

## 📂 Cấu Trúc Thư Mục Chính
    ```
        AnhHoaBakery/                          ← thư mục solution (gốc)
        ├── AnhHoaBakery.sln                   File solution của Visual Studio
        └── AnhHoaBakery/                      ← thư mục project (chứa AnhHoaBakery.csproj)
            │
            ├── Controllers/                   Xử lý logic và điều hướng
            │   ├── HomeController.cs              Trang chủ, danh mục, chi tiết bánh, tìm kiếm, lọc
            │   ├── AccountController.cs           Đăng ký, đăng nhập, hồ sơ, lịch sử đơn hàng
            │   ├── CartController.cs              Giỏ hàng (Session) và đặt hàng
            │   ├── CuaHangController.cs           Danh sách cửa hàng (công khai)
            │   ├── AdminProductController.cs      Quản lý bánh
            │   ├── QuanLyDanhMucController.cs     Quản lý danh mục
            │   ├── QuanLyCuaHangController.cs     Quản lý cửa hàng
            │   └── AdminThongKeController.cs      Thống kê doanh thu
            │
            ├── Models/                        Các lớp dữ liệu (ánh xạ bảng trong CSDL)
            │   ├── Banh.cs, DanhMuc.cs, ThanhVien.cs
            │   ├── DonHang.cs, ChiTietDonHang.cs, CuaHang.cs
            │   └── CartItem.cs, ErrorViewModel.cs
            │
            ├── Data/
            │   └── BakeryContext.cs           DbContext của Entity Framework Core
            │
            ├── Migrations/                    3 migration và ModelSnapshot, dùng để tạo CSDL
            │
            ├── Views/                         Giao diện Razor (.cshtml)
            │   ├── Home/                          Index, Categories, Details, Privacy
            │   ├── Account/                       Login, Register, Profile, History
            │   ├── Cart/                          Index, Cart, Checkout
            │   ├── CuaHang/                       Index
            │   ├── AdminProduct/                  Index, Create, Edit
            │   ├── QuanLyDanhMuc/                 Index, Create, Edit
            │   ├── QuanLyCuaHang/                 Index, Create, Edit
            │   ├── AdminThongKe/                  Index
            │   └── Shared/                        _Layout, Error, _ValidationScriptsPartial
            │
            ├── wwwroot/                       Tệp tĩnh
            │   ├── css/site.css
            │   ├── js/site.js
            │   ├── images/                        Ảnh bánh (banh1…8.jpg) và cửa hàng (store1…4.jpg)
            │   └── lib/                           bootstrap, jquery, jquery-validation
            │
            ├── Properties/launchSettings.json Cổng và profile khi chạy
            ├── appsettings.json               Chuỗi kết nối CSDL
            ├── appsettings.Development.json
            ├── Program.cs                     Điểm khởi động, cấu hình DbContext và Session
            └── AnhHoaBakery.csproj            Khai báo .NET 10 và các gói NuGet
    ```


