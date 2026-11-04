using AnhHoaBakery.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Controllers
{
    public class CuaHangController : Controller
    {
        private readonly BakeryContext _context;

        public CuaHangController(BakeryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.CuaHangs.ToListAsync()); // Lấy dữ liệu từ bảng CUAHANG [cite: 601]
        }
    }
}