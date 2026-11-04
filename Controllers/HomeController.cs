using System.Diagnostics;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using AnhHoaBakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Controllers
{
    public class HomeController : Controller
    {
        private readonly BakeryContext _ctx;
        public HomeController(BakeryContext ctx) { _ctx = ctx; }

        //public async Task<IActionResult> Index()
        //{
        //    // Lấy danh sách bánh mới nhất [cite: 308]
        //    var banhs = await _ctx.Banhs.Where(b => b.TrangThai == true)
        //                                .OrderByDescending(b => b.NgaySanXuat)
        //                                .Take(8).ToListAsync();
        //    return View(banhs);
        //}
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            // 1. Lấy danh sách danh mục để hiển thị bên sidebar (hoặc menu)
            ViewBag.DanhMucs = await _ctx.DanhMucs.Where(x => x.TrangThai == true).ToListAsync();

            // 2. Query sản phẩm
            var products = _ctx.Banhs.Include(b => b.DanhMuc).Where(b => b.TrangThai == true).AsQueryable();

            // 3. Xử lý Tìm kiếm (Use case 2.3.4)
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.TenBanh.Contains(searchString));
                ViewBag.SearchString = searchString; // Giữ lại từ khóa ở ô input
            }

            // 4. Xử lý Lọc theo danh mục (Use case 2.3.8)
            if (categoryId.HasValue)
            {
                products = products.Where(x => x.MaDanhMuc == categoryId);
                ViewBag.CurrentCategory = categoryId;
            }

            return View(await products.OrderByDescending(p => p.NgaySanXuat).ToListAsync());
        }

        public async Task<IActionResult> Categories()
        {
            var categories = await _ctx.DanhMucs
                                           .Where(x => x.TrangThai == true)
                                           .ToListAsync();
            return View(categories);
        }

        // Xem chi tiết bánh
        public async Task<IActionResult> Details(int id)
        {
            // 1. Tìm bánh theo ID, kết hợp lấy tên Danh mục
            var banh = await _ctx.Banhs
                                 .Include(b => b.DanhMuc)
                                 .FirstOrDefaultAsync(b => b.MaBanh == id);

            if (banh == null)
            {
                return NotFound(); // Nếu không thấy bánh thì báo lỗi
            }

            // 2. Lấy danh sách "Sản phẩm liên quan" (cùng danh mục, trừ bánh hiện tại)
            ViewBag.RelatedProducts = await _ctx.Banhs
                                                .Where(b => b.MaDanhMuc == banh.MaDanhMuc && b.MaBanh != id)
                                                .Take(4) // Lấy 4 bánh
                                                .ToListAsync();

            return View(banh);
        }
    }
}
