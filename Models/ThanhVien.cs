using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("THANHVIEN")]
    public class ThanhVien
    {
        [Key]
        public int MaNguoiDung { get; set; }

        [Required]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; }

        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public bool TrangThai { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}