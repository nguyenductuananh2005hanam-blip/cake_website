using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Controllers
{
    // Controller quản lý danh mục (Khớp Hình 3.18)
    public class QuanLyDanhMucController : Controller
    {
        private readonly BakeryContext _context;

        public QuanLyDanhMucController(BakeryContext context)
        {
            _context = context;
        }

        // GET: Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            // Kiểm tra đăng nhập Admin (Giả sử session đã lưu)
            if (HttpContext.Session.GetString("Username") != "admin")
                return RedirectToAction("Login", "Account");

            return View(await _context.DanhMucs.ToListAsync());
        }

        // GET: Tạo mới
        public IActionResult Create() => View();

        // POST: Tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMuc danhMuc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhMuc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // GET: Sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null) return NotFound();
            return View(danhMuc);
        }

        // POST: Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DanhMuc danhMuc)
        {
            if (id != danhMuc.MaDanhMuc) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhMuc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DanhMucs.Any(e => e.MaDanhMuc == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(danhMuc);
        }

        // GET: Xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var danhMuc = await _context.DanhMucs.FindAsync(id);
            if (danhMuc == null) return NotFound();

            // Thực hiện xóa luôn hoặc hiện form xác nhận (Ở đây làm nhanh xóa luôn)
            _context.DanhMucs.Remove(danhMuc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}