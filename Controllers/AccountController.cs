//using AnhHoaBakery.Data;
//using AnhHoaBakery.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Cryptography; // Để mã hóa MD5
//using System.Text;
//using Microsoft.EntityFrameworkCore; // Dùng .Include()


//namespace AnhHoaBakery.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly BakeryContext _ctx;
//        public AccountController(BakeryContext ctx) { _ctx = ctx; }

//        // Helper: Mã hóa MD5 [cite: 262]
//        public static string GetMD5(string str)
//        {
//            MD5 md5 = new MD5CryptoServiceProvider();
//            byte[] fromData = Encoding.UTF8.GetBytes(str);
//            byte[] targetData = md5.ComputeHash(fromData);
//            string byte2String = null;
//            for (int i = 0; i < targetData.Length; i++) byte2String += targetData[i].ToString("x2");
//            return byte2String;
//        }

//        [HttpGet]
//        public IActionResult Register() => View();

//        [HttpPost]
//        public IActionResult Register(ThanhVien tv)
//        {
//            if (_ctx.ThanhViens.Any(x => x.TenDangNhap == tv.TenDangNhap))
//            {
//                ViewBag.Error = "Tên đăng nhập đã tồn tại";
//                return View();
//            }
//            tv.MatKhau = GetMD5(tv.MatKhau); // Mã hóa mật khẩu
//            tv.TrangThai = true;
//            _ctx.ThanhViens.Add(tv);
//            _ctx.SaveChanges();
//            return RedirectToAction("Login");
//        }

//        [HttpGet]
//        public IActionResult Login() => View();

//        [HttpPost]
//        public IActionResult Login(string User, string Pass)
//        {
//            var hashPass = GetMD5(Pass);
//            var acc = _ctx.ThanhViens.FirstOrDefault(x => x.TenDangNhap == User && x.MatKhau == hashPass);
//            if (acc != null)
//            {
//                HttpContext.Session.SetString("Username", acc.TenDangNhap);
//                HttpContext.Session.SetInt32("UserId", acc.MaNguoiDung);
//                return RedirectToAction("Index", "Home");
//            }
//            ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
//            return View();
//        }
//        // Thêm vào AccountController
//        [HttpGet]
//        public IActionResult Profile()
//        {
//            var userId = HttpContext.Session.GetInt32("UserId");
//            if (userId == null) return RedirectToAction("Login");

//            var user = _ctx.ThanhViens.Find(userId);
//            return View(user);
//        }

//        [HttpPost]
//        public IActionResult UpdateProfile(ThanhVien updatedUser)
//        {
//            var userId = HttpContext.Session.GetInt32("UserId");
//            if (userId == null) return RedirectToAction("Login");

//            var user = _ctx.ThanhViens.Find(userId);
//            if (user != null)
//            {
//                // Cập nhật các trường cho phép đổi (Theo Use case 2.3.9)
//                user.HoTen = updatedUser.HoTen;
//                user.Email = updatedUser.Email;
//                user.SoDienThoai = updatedUser.SoDienThoai;
//                user.DiaChi = updatedUser.DiaChi;

//                // Nếu người dùng nhập mật khẩu mới thì mới đổi
//                if (!string.IsNullOrEmpty(updatedUser.MatKhau))
//                {
//                    user.MatKhau = GetMD5(updatedUser.MatKhau);
//                }

//                _ctx.SaveChanges();
//                ViewBag.Success = "Cập nhật thông tin thành công!";
//                return View("Profile", user);
//            }
//            return View("Profile");
//        }
//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Index", "Home");
//        }
//        // GET: Hiện form đăng ký
//        [HttpGet]
//        public IActionResult Register()
//        {
//            return View();
//        }

//        // POST: Xử lý đăng ký
//        [HttpPost]
//        public IActionResult Register(string HoTen, string TenDangNhap, string SoDienThoai, string MatKhau, string ConfirmPassword)
//        {
//            if (MatKhau != ConfirmPassword)
//            {
//                TempData["Error"] = "Mật khẩu xác nhận không khớp!";
//                return View();
//            }

//            // Kiểm tra tên đăng nhập đã tồn tại chưa
//            var checkUser = _ctx.ThanhViens.FirstOrDefault(x => x.TenDangNhap == TenDangNhap);
//            if (checkUser != null)
//            {
//                TempData["Error"] = "Tên đăng nhập này đã được sử dụng!";
//                return View();
//            }

//            // Tạo khách hàng mới
//            var user = new ThanhVien
//            {
//                HoTen = HoTen,
//                TenDangNhap = TenDangNhap,
//                SoDienThoai = SoDienThoai,
//                MatKhau = MatKhau, // Lưu ý: Nên mã hóa mật khẩu trong thực tế (MD5/SHA256)
//                DiaChi = "" // Để trống cập nhật sau
//            };

//            _ctx.ThanhViens.Add(user);
//            _ctx.SaveChanges();

//            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
//            return RedirectToAction("Login");
//        }

//        public async Task<IActionResult> History()
//        {
//            // 1. Kiểm tra đăng nhập
//            var userId = HttpContext.Session.GetInt32("UserId");
//            if (userId == null)
//            {
//                return RedirectToAction("Login");
//            }

//            // 2. Lấy danh sách đơn hàng của User này
//            // Dùng .Include để lấy luôn chi tiết đơn hàng và thông tin bánh
//            var orders = await _ctx.DonHangs
//                                .Include(d => d.ChiTietDonHangs)
//                                .ThenInclude(ct => ct.Banh)
//                                .Where(d => d.MaNguoiDung == userId)
//                                .OrderByDescending(d => d.NgayDat) // Đơn mới nhất lên đầu
//                                .ToListAsync();

//            return View(orders);
//        }
//    }
//}



using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography; // Để mã hóa MD5
using System.Text;
using Microsoft.EntityFrameworkCore; // Dùng .Include()

namespace AnhHoaBakery.Controllers
{
    public class AccountController : Controller
    {
        private readonly BakeryContext _ctx;
        public AccountController(BakeryContext ctx) { _ctx = ctx; }

        //Helper: Mã hóa MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++) byte2String += targetData[i].ToString("x2");
            return byte2String;
        }

        // --- PHẦN LOGIN (ĐĂNG NHẬP) ---
        [HttpGet]
        public IActionResult Login() => View();

        //[HttpPost]
        //public IActionResult Login(string User, string Pass)
        //{
        //    // Mã hóa mật khẩu người dùng nhập vào để so sánh với DB
        //    var hashPass = GetMD5(Pass);
        //    var acc = _ctx.ThanhViens.FirstOrDefault(x => x.TenDangNhap == User && x.MatKhau == hashPass);
        //    if (acc != null)
        //    {
        //        HttpContext.Session.SetString("Username", acc.TenDangNhap);
        //        HttpContext.Session.SetInt32("UserId", acc.MaNguoiDung);
        //        return RedirectToAction("Index", "Home");
        //    }
        //    ViewBag.Error = "Sai tài khoản hoặc mật khẩu";
        //    return View();
        //}
        [HttpPost]
        public IActionResult Login(string User, string Pass)
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Pass))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tài khoản và mật khẩu";
                return View();
            }

            var hashPass = GetMD5(Pass);
            var acc = _ctx.ThanhViens.FirstOrDefault(x => x.TenDangNhap == User && x.MatKhau == hashPass);

            if (acc != null)
            {
                // Kiểm tra trạng thái tài khoản (TrangThai = true là hoạt động, false là bị khóa)
                if (!acc.TrangThai)
                {
                    ViewBag.Error = "Tài khoản của bạn đã bị khóa.";
                    return View();
                }

                HttpContext.Session.SetString("Username", acc.TenDangNhap);
                HttpContext.Session.SetInt32("UserId", acc.MaNguoiDung);

                // Phân quyền Admin tạm thời dựa vào tên đăng nhập
                if (acc.TenDangNhap.ToLower() == "admin")
                {
                    HttpContext.Session.SetString("IsAdmin", "True");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }
        // --- PHẦN LOGOUT (ĐĂNG XUẤT) ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // --- PHẦN REGISTER (ĐĂNG KÝ) ---
        // Đã xóa phần trùng lặp, chỉ giữ lại logic chi tiết này

        // --- PHẦN REGISTER (ĐĂNG KÝ) ---

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(ThanhVien model, string ConfirmPassword, bool agreeTerms)
        {
            // 1. Kiểm tra checkbox "Tôi đồng ý..."
            if (!agreeTerms)
            {
                ViewBag.Error = "Bạn cần đồng ý với điều khoản và quy định.";
                return View(model);
            }

            // 2. Kiểm tra xác nhận mật khẩu
            if (model.MatKhau != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View(model);
            }

            // 3. Kiểm tra Tên đăng nhập đã tồn tại chưa
            var checkUser = _ctx.ThanhViens.FirstOrDefault(x => x.TenDangNhap == model.TenDangNhap);
            if (checkUser != null)
            {
                ViewBag.Error = "Tên đăng nhập này đã tồn tại.";
                return View(model);
            }

            // 4. Kiểm tra Email đã tồn tại chưa (tùy chọn, nên có)
            var checkEmail = _ctx.ThanhViens.FirstOrDefault(x => x.Email == model.Email);
            if (checkEmail != null)
            {
                ViewBag.Error = "Email này đã được sử dụng.";
                return View(model);
            }

            try
            {
                // 5. Mã hóa mật khẩu trước khi lưu (Sử dụng hàm GetMD5 có sẵn của bạn)
                model.MatKhau = GetMD5(model.MatKhau);

                // 6. Lưu vào bảng THANHVIEN
                _ctx.ThanhViens.Add(model);
                _ctx.SaveChanges();

                // 7. Hiển thị thông báo thành công đúng như yêu cầu
                ViewBag.Success = "Xin chúc mừng bạn, bạn đã đăng ký thành công!";

                // Trả về View để hiện thông báo (Form sẽ bị ẩn đi nhờ logic bên View)
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra: " + ex.Message;
                return View(model);
            }
        }

        // --- PHẦN PROFILE (HỒ SƠ) ---
        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _ctx.ThanhViens.Find(userId);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateProfile(ThanhVien updatedUser)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            var user = _ctx.ThanhViens.Find(userId);
            if (user != null)
            {
                // Cập nhật các trường cho phép đổi
                user.HoTen = updatedUser.HoTen;
                user.Email = updatedUser.Email;
                user.SoDienThoai = updatedUser.SoDienThoai;
                user.DiaChi = updatedUser.DiaChi;

                // Nếu người dùng nhập mật khẩu mới thì mới đổi và mã hóa lại
                if (!string.IsNullOrEmpty(updatedUser.MatKhau))
                {
                    user.MatKhau = GetMD5(updatedUser.MatKhau);
                }

                _ctx.SaveChanges();
                ViewBag.Success = "Cập nhật thông tin thành công!";
                return View("Profile", user);
            }
            return View("Profile");
        }

        // --- PHẦN LỊCH SỬ ĐƠN HÀNG ---
        public async Task<IActionResult> History()
        {
            // 1. Kiểm tra đăng nhập
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            // 2. Lấy danh sách đơn hàng của User này
            var orders = await _ctx.DonHangs
                                    .Include(d => d.ChiTietDonHangs)
                                    .ThenInclude(ct => ct.Banh)
                                    .Where(d => d.MaNguoiDung == userId)
                                    .OrderByDescending(d => d.NgayDat)
                                    .ToListAsync();

            return View(orders);
        }
    }
}