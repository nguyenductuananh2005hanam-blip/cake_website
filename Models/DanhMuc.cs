using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("DANHMUC")]
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDanhMuc { get; set; }

        public string? MoTa { get; set; }
        public bool TrangThai { get; set; }
    }
}