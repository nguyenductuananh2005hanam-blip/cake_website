using AnhHoaBakery.Data;
using AnhHoaBakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AnhHoaBakery.Controllers
{
    public class AdminThongKeController : Controller
    {
        private readonly BakeryContext _context;

        public AdminThongKeController(BakeryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Kiểm tra quyền Admin
            if (HttpContext.Session.GetString("Username") != "admin")
                return RedirectToAction("Login", "Account");

            // 1. Lấy dữ liệu đơn hàng và nhóm theo Tháng/Năm
            var data = await _context.DonHangs
                .Select(d => new { d.NgayDat, d.TongTien }) // Chỉ lấy cột cần thiết
                .ToListAsync(); // Lấy về RAM trước để xử lý ngày tháng dễ hơn

            var thongKe = data
                .GroupBy(x => new { x.NgayDat.Month, x.NgayDat.Year })
                .Select(g => new ThongKeViewModel
                {
                    Thang = g.Key.Month,
                    Nam = g.Key.Year,
                    TongDoanhThu = g.Sum(x => x.TongTien),
                    SoLuongDon = g.Count()
                })
                .OrderByDescending(x => x.Nam).ThenByDescending(x => x.Thang)
                .ToList();

            // 2. Chuẩn bị dữ liệu cho Biểu đồ (ChartJS)
            // Đảo ngược lại để tháng cũ bên trái, tháng mới bên phải cho biểu đồ hợp lý
            var dataChart = thongKe.OrderBy(x => x.Nam).ThenBy(x => x.Thang).ToList();

            ViewBag.Labels = dataChart.Select(x => "Tháng " + x.Thang + "/" + x.Nam).ToArray();
            ViewBag.Values = dataChart.Select(x => x.TongDoanhThu).ToArray();

            return View(thongKe);
        }
    }

    // Class phụ để chứa dữ liệu (Bạn có thể để ở file riêng trong Models cũng được)
    public class ThongKeViewModel
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int SoLuongDon { get; set; }
    }
}