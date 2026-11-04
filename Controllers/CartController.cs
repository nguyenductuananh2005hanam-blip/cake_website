using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // Cần cài package Newtonsoft.Json nếu chưa có

namespace AnhHoaBakery.Controllers
{
    public class CartController : Controller
    {
        private readonly BakeryContext _ctx;
        public CartController(BakeryContext ctx) { _ctx = ctx; }

        // Lấy giỏ hàng từ Session
        public List<CartItem> GetCart()
        {
            var session = HttpContext.Session.GetString("Cart");
            if (session != null) return JsonConvert.DeserializeObject<List<CartItem>>(session);
            return new List<CartItem>();
        }

        // Thêm vào giỏ [cite: 316]
        public IActionResult AddToCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.MaBanh == id);
            if (item != null)
            {
                item.SoLuong++;
            }
            else
            {
                var p = _ctx.Banhs.Find(id);
                cart.Add(new CartItem { MaBanh = p.MaBanh, TenBanh = p.TenBanh, Gia = p.Gia, HinhAnh = p.HinhAnh, SoLuong = 1 });
            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }

        // ... (Các hàm cũ giữ nguyên)

        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = GetCart(); // Lấy giỏ hàng hiện tại
            var item = cart.FirstOrDefault(x => x.MaBanh == id);

            if (item != null)
            {
                if (quantity > 0)
                {
                    item.SoLuong = quantity; // Cập nhật số lượng mới
                }
                else
                {
                    cart.Remove(item); // Nếu nhập số 0 hoặc âm thì xóa luôn
                }

                // Lưu lại vào Session
                HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Index"); // Load lại trang giỏ hàng
        }

        // ... (Các hàm Index, Checkout cũ giữ nguyên)

        public IActionResult Index() => View(GetCart());

        // Thanh toán [cite: 314]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login", "Account");
            return View(GetCart());
        }

        [HttpPost]
        public IActionResult ConfirmCheckout(string NguoiNhan, string SDT, string DiaChi)
        {
            var cart = GetCart();
            var dh = new DonHang
            {
                MaNguoiDung = HttpContext.Session.GetInt32("UserId"),
                TenNguoiNhan = NguoiNhan,
                SDTNhan = SDT,
                DiaChiGiao = DiaChi,
                TongTien = cart.Sum(x => x.ThanhTien)
            };
            _ctx.DonHangs.Add(dh);
            _ctx.SaveChanges(); // Để lấy ID Đơn hàng

            foreach (var c in cart)
            {
                _ctx.ChiTietDonHangs.Add(new ChiTietDonHang
                {
                    MaDonHang = dh.MaDonHang,
                    MaBanh = c.MaBanh,
                    SoLuong = c.SoLuong,
                    DonGia = c.Gia
                });
            }
            _ctx.SaveChanges();
            HttpContext.Session.Remove("Cart"); // Xóa giỏ
            //return Content("Đặt hàng thành công!");
            TempData["SuccessMessage"] = "Cảm ơn bạn! Đơn hàng đã được đặt thành công.";

            // 2. Chuyển hướng ngay lập tức về Trang chủ
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Route("/Cart/AddToCartAPI")]
        public IActionResult AddToCartAPI(int id)
        {
            var banh = _ctx.Banhs.Find(id);
            if (banh == null) return Json(new { success = false, message = "Sản phẩm không tồn tại" });

            var cart = GetCart();
            var item = cart.FirstOrDefault(p => p.MaBanh == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaBanh = banh.MaBanh,
                    TenBanh = banh.TenBanh,
                    Gia = banh.Gia,
                    HinhAnh = banh.HinhAnh,
                    SoLuong = 1
                });
            }
            else
            {
                item.SoLuong++;
            }

            HttpContext.Session.SetString("Cart", Newtonsoft.Json.JsonConvert.SerializeObject(cart));

            // Trả về JSON thông báo thành công và tổng số lượng để update icon giỏ hàng
            return Json(new
            {
                success = true,
                message = "Đã thêm " + banh.TenBanh + " vào giỏ hàng!",
                cartCount = cart.Sum(x => x.SoLuong)
            });
        }
    }
}
