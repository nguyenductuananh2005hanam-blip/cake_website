using AnhHoaBakery.Models;
using Microsoft.EntityFrameworkCore;

namespace AnhHoaBakery.Data
{
    public class BakeryContext : DbContext
    {
        public BakeryContext(DbContextOptions<BakeryContext> options) : base(options) { }

        public DbSet<Banh> Banhs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<ThanhVien> ThanhViens { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<CuaHang> CuaHangs { get; set; }
    }
}