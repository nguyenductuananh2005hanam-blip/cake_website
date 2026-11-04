using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnhHoaBakery.Models
{
    [Table("CUAHANG")]
    public class CuaHang
    {
        [Key]
        public int MaCuaHang { get; set; }

        [Required]
        [StringLength(150)]
        public string TenCuaHang { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        [StringLength(20)]
        public string? Hotline { get; set; } // Cho phép null

        [StringLength(255)]
        public string? AnhMinhHoa { get; set; } // Cho phép null
    }
}