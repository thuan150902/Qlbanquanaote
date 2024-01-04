using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QLtreem.Models;
using QLtreem.Models.Authentication;
using System.Data.Entity;
using System.Diagnostics;
using System.Text.RegularExpressions;
using X.PagedList;

namespace QLtreem.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		QltreEmContext db= new QltreEmContext();
		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		[Authentication]
		public IActionResult Index(int? page , String? Search)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 6;
			

			List<SanPham> lstsanpham = new List<SanPham>();
			if (Search != null && Search != "")
			{
				int number;
				String st = Search.Trim();
				bool isNumeric = int.TryParse(st, out number);
				if (isNumeric)
				{
					lstsanpham = db.SanPhams.Where(x => x.GiaNhap == int.Parse(st) || x.GiaBan == int.Parse(st)).ToList();
				}
				else
				{
					lstsanpham = db.SanPhams.AsEnumerable()
						.Where(x => Convert(x.TenSanPham.ToLower()).Contains(Convert(st).ToLower()) || Convert(x.ChatLieu.ToLower()).Contains(Convert(st).ToLower())).ToList();
				}
			}
			else
			{
				lstsanpham = db.SanPhams.ToList();
			}
			PagedList<SanPham> lst = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize);
			ViewBag.Search = Search;
			return View(lst);
		}
		public IActionResult Shop(int? page, String? Search)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 6;
			List<SanPham> lstsanpham = new List<SanPham>();
			if (Search != null && Search != "")
			{
				int number;
				String st = Search.Trim();
				bool isNumeric = int.TryParse(st, out number);
				if (isNumeric)
				{
					lstsanpham = db.SanPhams.Where(x => x.GiaNhap == int.Parse(st) || x.GiaBan == int.Parse(st)).ToList();
				}
				else
				{
					lstsanpham = db.SanPhams.AsEnumerable()
						.Where(x => Convert(x.TenSanPham.ToLower()).Contains(Convert(st).ToLower()) || Convert(x.ChatLieu.ToLower()).Contains(Convert(st).ToLower())).ToList();
				}
			}
			else
			{
				lstsanpham = db.SanPhams.ToList();
			}
			PagedList<SanPham> lst = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize);
			return View(lst);
		}
		public IActionResult ChiTietSanPham(string maSp)
		{
			var anhSanPham = db.HinhAnhSps.Where(x => x.MaSanPham == maSp).ToList();
			ViewBag.anhSanPham = anhSanPham;
			List<String> sizeSP = (from ctsp in db.ChiTietSanPhams
								   join kt in db.KichThuocs on ctsp.MaKichThuoc equals kt.MaKichThuoc
								   where ctsp.MaSanPham == maSp
								   select kt.TenKichThuoc
						  ).Distinct().ToList();
			ViewData["SizeSP"] = sizeSP;
			List<string> mausac= (from ctsp in db.ChiTietSanPhams
								  join ms in db.MauSacs on ctsp.MaMauSac equals ms.MaMauSac
								  where ctsp.MaSanPham == maSp
								  select ms.TenMauSac
						  ).Distinct().ToList();
			ViewData["MauSP"]= mausac;
			

			var sanpham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == maSp);
			if (sanpham == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return View(sanpham);
			}
		}
		public IActionResult SanPhamTheoLoai(string MaLoai , int? page)
		{
			int pageNumber = page == null || page < 1 ? 1 : page.Value;
			int pageSize = 6;
			var lstSanpham = db.SanPhams.AsNoTracking().Where(x => x.MaLoaiSp == MaLoai).OrderBy(x => x.TenSanPham);
			PagedList<SanPham> lst = new PagedList<SanPham>(lstSanpham, pageNumber, pageSize);
			ViewBag.MaLoaiSp = MaLoai;
			return View(lst);
		}
		public IActionResult ProductDetail(string maSp)
		{
			return View();
		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		public static string Convert(string input)
		{
			var result = Regex.Replace(input, "[àáảãạăắằẳẵặâầấẩẫậ]", "a");
			result = Regex.Replace(result, "[đ]", "d");
			result = Regex.Replace(result, "[èéẻẽẹêềếểễệ]", "e");
			result = Regex.Replace(result, "[ìíỉĩị]", "i");
			result = Regex.Replace(result, "[òóỏõọôồốổỗộơờớởỡợ]", "o");
			result = Regex.Replace(result, "[ùúủũụưừứửữự]", "u");
			result = Regex.Replace(result, "[ỳýỷỹỵ]", "y");
			result = Regex.Replace(result, "[^a-zA-Z0-9]", " ");

			return result;
		}
	}
}