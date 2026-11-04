using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("DONHANG")]
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }

        public int? MaNguoiDung { get; set; } // Null nếu là khách vãng lai

        public DateTime NgayDat { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(15,0)")]
        public decimal TongTien { get; set; }

        [StringLength(100)]
        public string TenNguoiNhan { get; set; }

        [StringLength(15)]
        public string SDTNhan { get; set; }

        [StringLength(255)]
        public string DiaChiGiao { get; set; }

        public string? GhiChu { get; set; }

        public string TrangThai { get; set; } = "Mới đặt";

        public string PhuongThucTT { get; set; } = "COD";

        // --- THÊM DÒNG NÀY ĐỂ SỬA LỖI ---
        // Đây là cầu nối để lệnh .Include hoạt động
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}