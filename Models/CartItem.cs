namespace AnhHoaBakery.Models
{
    public class CartItem
    {
        public int MaBanh { get; set; }
        public string TenBanh { get; set; }
        public string HinhAnh { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien => SoLuong * Gia;
    }
}
