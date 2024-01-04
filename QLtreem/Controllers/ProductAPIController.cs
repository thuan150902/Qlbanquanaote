using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLtreem.Models;
using QLtreem.Models.ProductModels;

namespace QLtreem.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductAPIController : ControllerBase
	{
		QltreEmContext db=new QltreEmContext();
		[HttpGet]
		public IEnumerable<Product> GetAllProducts()
		{
			var sanPham = (from p in db.SanPhams
						   select new Product
						   {
							   MaSanPham = p.MaSanPham,
							   TenSanPham = p.TenSanPham,
							   MaLoaiSp = p.MaLoaiSp,
							   HinhAnhAvatar = p.HinhAnhAvatar,
							   GiaBan = p.GiaBan
							   
						   }).ToList();
			return sanPham;

		}
		[HttpGet("{maLoaiSp}")]
		public IEnumerable<Product> GetProductsByCategory(string maLoaiSp)
		{
			IList<Product> products = new List<Product>();
			var sanPham = db.SanPhams.Where(x => x.MaLoaiSp == maLoaiSp).ToList();
			foreach (var s in sanPham)
			{
				products.Add(new Product
				{
					MaSanPham = s.MaSanPham,
					TenSanPham = s.TenSanPham,
					MaLoaiSp = s.MaLoaiSp,
					HinhAnhAvatar = s.HinhAnhAvatar,
					GiaBan = s.GiaBan

				});
			}

			return products;
		}
	}
}
