namespace QLtreem.Models.ProductModels
{
	public class Product
	{
		public string MaSanPham { get; set; } = null!;
		public string? TenSanPham { get; set; }
		public string? HinhAnhAvatar { get; set; }
		public string? MaLoaiSp { get; set; }
		public double? GiaBan { get; set; }
	}
}
