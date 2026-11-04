using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("CHITIETDONHANG")]
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }
        public int MaDonHang { get; set; }
        public int MaBanh { get; set; }
        public int SoLuong { get; set; }
        [Column(TypeName = "decimal(15,0)")]
        public decimal DonGia { get; set; }

        [ForeignKey("MaDonHang")]
        public virtual DonHang DonHang { get; set; }
        [ForeignKey("MaBanh")]
        public virtual Banh Banh { get; set; }
    }
}