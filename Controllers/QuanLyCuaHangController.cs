//using AnhHoaBakery.Data;
//using AnhHoaBakery.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace AnhHoaBakery.Controllers
//{
//    public class QuanLyCuaHangController : Controller
//    {
//        private readonly BakeryContext _context;
//        private readonly IWebHostEnvironment _env; // Để upload ảnh cửa hàng

//        public QuanLyCuaHangController(BakeryContext context, IWebHostEnvironment env)
//        {
//            _context = context;
//            _env = env;
//        }

//        // GET: Index
//        public async Task<IActionResult> Index()
//        {
//            if (HttpContext.Session.GetString("Username") != "admin")
//                return RedirectToAction("Login", "Account");
//            return View(await _context.CuaHangs.ToListAsync());
//        }

//        // GET: Create
//        public IActionResult Create() => View();

//        // POST: Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(CuaHang cuaHang, IFormFile? HinhAnhFile)
//        {
//            if (ModelState.IsValid)
//            {
//                if (HinhAnhFile != null)
//                {
//                    // Xử lý upload ảnh
//                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhFile.FileName);
//                    string path = Path.Combine(_env.WebRootPath, "images", fileName);
//                    using (var stream = new FileStream(path, FileMode.Create))
//                    {
//                        await HinhAnhFile.CopyToAsync(stream);
//                    }
//                    cuaHang.AnhMinhHoa = fileName;
//                }

//                _context.Add(cuaHang);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(cuaHang);
//        }

//        // Action Edit và Delete tương tự QuanLyDanhMuc, chỉ cần đổi tên Model
//    }
//}


using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Controllers
{
    public class QuanLyCuaHangController : Controller
    {
        private readonly BakeryContext _context;
        private readonly IWebHostEnvironment _env; // Để lấy đường dẫn thư mục wwwroot

        public QuanLyCuaHangController(BakeryContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // 1. DANH SÁCH CỬA HÀNG
        public async Task<IActionResult> Index()
        {
            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("Username") != "admin")
                return RedirectToAction("Login", "Account");

            return View(await _context.CuaHangs.ToListAsync());
        }

        // 2. TẠO MỚI (Giao diện)
        public IActionResult Create()
        {
            return View();
        }

        // 2. TẠO MỚI (Xử lý dữ liệu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CuaHang cuaHang, IFormFile? HinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                if (HinhAnhFile != null)
                {
                    // Tạo tên file ngẫu nhiên để tránh trùng
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhFile.FileName);
                    string path = Path.Combine(_env.WebRootPath, "images", fileName);

                    // Lưu file vào thư mục wwwroot/images
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await HinhAnhFile.CopyToAsync(stream);
                    }
                    cuaHang.AnhMinhHoa = fileName; // Lưu tên file vào DB
                }

                _context.Add(cuaHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cuaHang);
        }

        // 3. CHỈNH SỬA (Giao diện)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cuaHang = await _context.CuaHangs.FindAsync(id);
            if (cuaHang == null) return NotFound();

            return View(cuaHang);
        }

        // 3. CHỈNH SỬA (Xử lý dữ liệu)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CuaHang cuaHang, IFormFile? HinhAnhFile)
        {
            if (id != cuaHang.MaCuaHang) return NotFound();

            // Tìm bản ghi cũ trong database để cập nhật
            var existingCuaHang = await _context.CuaHangs.FindAsync(id);
            if (existingCuaHang == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật các thông tin văn bản
                    existingCuaHang.TenCuaHang = cuaHang.TenCuaHang;
                    existingCuaHang.DiaChi = cuaHang.DiaChi;
                    existingCuaHang.Hotline = cuaHang.Hotline;

                    // Xử lý ảnh: Chỉ cập nhật nếu người dùng chọn ảnh mới
                    if (HinhAnhFile != null)
                    {
                        // 1. Upload ảnh mới
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(HinhAnhFile.FileName);
                        string path = Path.Combine(_env.WebRootPath, "images", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await HinhAnhFile.CopyToAsync(stream);
                        }

                        // 2. Cập nhật tên ảnh mới vào DB
                        existingCuaHang.AnhMinhHoa = fileName;
                    }
                    // Nếu HinhAnhFile == null thì giữ nguyên existingCuaHang.AnhMinhHoa cũ

                    _context.Update(existingCuaHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuaHangExists(cuaHang.MaCuaHang)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cuaHang);
        }

        // 4. XÓA CỬA HÀNG
        // Lưu ý: Nếu dùng thẻ <a> để xóa thì dùng [HttpGet], nếu dùng Form thì dùng [HttpPost]
        // Ở đây tôi viết kiểu đơn giản nhất cho nút "Xóa" chạy thẳng
        public async Task<IActionResult> Delete(int id)
        {
            var cuaHang = await _context.CuaHangs.FindAsync(id);
            if (cuaHang != null)
            {
                _context.CuaHangs.Remove(cuaHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CuaHangExists(int id)
        {
            return _context.CuaHangs.Any(e => e.MaCuaHang == id);
        }
    }
}