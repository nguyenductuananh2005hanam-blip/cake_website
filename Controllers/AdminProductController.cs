using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Để dùng SelectList
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly BakeryContext _ctx;
        public AdminProductController(BakeryContext ctx) { _ctx = ctx; }

        // Kiểm tra quyền Admin (Giả sử admin có tên đăng nhập là 'admin')
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Username") == "admin";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            // Include DanhMuc để hiện tên danh mục thay vì ID
            return View(await _ctx.Banhs.Include(b => b.DanhMuc).ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            ViewBag.MaDanhMuc = new SelectList(_ctx.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(Banh banh)
        {
            if (ModelState.IsValid)
            {
                _ctx.Add(banh);
                await _ctx.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MaDanhMuc = new SelectList(_ctx.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View(banh);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");
            var banh = await _ctx.Banhs.FindAsync(id);
            if (banh != null)
            {
                _ctx.Banhs.Remove(banh);
                await _ctx.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --- BỔ SUNG HÀM EDIT (GET) - Để hiển thị dữ liệu lên form ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return NotFound();
            }

            // 1. Tìm bánh trong database theo ID
            var banh = await _ctx.Banhs.FindAsync(id);
            if (banh == null)
            {
                return NotFound();
            }

            // 2. Tạo danh sách danh mục đưa vào ViewBag (để hiển thị dropdown)
            // Lưu ý: tham số thứ 4 là banh.MaDanhMuc để nó tự chọn đúng danh mục cũ
            ViewBag.MaDanhMuc = new SelectList(_ctx.DanhMucs, "MaDanhMuc", "TenDanhMuc", banh.MaDanhMuc);

            // 3. Trả về View kèm dữ liệu
            return View(banh);
        }

        // --- BỔ SUNG ĐOẠN NÀY ---
        // GET: AdminProduct/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    // 1. Kiểm tra ID hợp lệ
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    // 2. Tìm bánh trong Database
        //    var banh = await _ctx.Banhs.FindAsync(id);
        //    if (banh == null)
        //    {
        //        return NotFound();
        //    }

        //    // 3. Tạo danh sách Danh mục cho Dropdown (QUAN TRỌNG)
        //    // Tham số thứ 4 (banh.MaDanhMuc) để nó tự chọn đúng danh mục hiện tại của bánh
        //    ViewBag.MaDanhMuc = new SelectList(_ctx.DanhMucs, "MaDanhMuc", "TenDanhMuc", banh.MaDanhMuc);

        //    // 4. Trả về View để hiển thị dữ liệu
        //    return View(banh);
        //}
        // ------------------------
        // --- BỔ SUNG HÀM EDIT (POST) - Để lưu dữ liệu khi bấm nút Cập nhật ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Banh banh, IFormFile? fileAnh)
        {
            if (id != banh.MaBanh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý nếu người dùng có chọn ảnh mới
                    if (fileAnh != null && fileAnh.Length > 0)
                    {
                        // Lưu ý: Bạn cần code thêm phần lưu file vào thư mục wwwroot/images ở đây
                        // Ví dụ: banh.HinhAnh = "ten-file-moi.jpg";
                    }
                    else
                    {
                        // Nếu không chọn ảnh mới, giữ nguyên ảnh cũ (cần logic lấy lại ảnh cũ)
                        // Tạm thời code này sẽ update các thông tin khác
                    }

                    _ctx.Update(banh);
                    await _ctx.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_ctx.Banhs.Any(e => e.MaBanh == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            // Nếu nhập sai, load lại dropdown danh mục
            ViewBag.MaDanhMuc = new SelectList(_ctx.DanhMucs, "MaDanhMuc", "TenDanhMuc", banh.MaDanhMuc);
            return View(banh);
        }
    }
}
