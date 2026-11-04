using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("BANH")]
    public class Banh
    {
        [Key]
        public int MaBanh { get; set; }

        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(150)]
        public string TenBanh { get; set; }

        [Column(TypeName = "decimal(15,0)")]
        public decimal Gia { get; set; }

        public DateTime NgaySanXuat { get; set; }
        public DateTime HanSuDung { get; set; }

        [StringLength(255)]
        public string? HinhAnh { get; set; }
        public string? MoTa { get; set; }
        public bool TrangThai { get; set; }

        [ForeignKey("MaDanhMuc")]
        public virtual DanhMuc? DanhMuc { get; set; }
    }
}