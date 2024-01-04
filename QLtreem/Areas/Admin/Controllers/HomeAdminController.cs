using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QLtreem.Models;
using QLtreem.Models.Authentication;
using System.Data.Entity;
using System.Text.RegularExpressions;
using X.PagedList;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace QLtreem.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]


    public class HomeAdminController : Controller
    {
		private readonly IWebHostEnvironment _webhost;
		public HomeAdminController(IWebHostEnvironment webhost)
		{
			_webhost = webhost;
		}
		QltreEmContext db=new QltreEmContext();
        [Route("")]
        [Route("index")]
        //[Authentication]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page, string? Search)
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
			ViewBag.Search = Search;
			PagedList<SanPham> lst = new PagedList<SanPham>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }
		[Route("ThemSanPham")]
		public IActionResult ThemSanPham()
		{
			ViewBag.MaLoaiSp = new SelectList(db.LoaiSps.ToList(), "MaLoaiSp", "TenLoaiSp");
            ViewBag.MaDoiTuongMh = new SelectList(db.DoiTuongMhs.ToList(), "MaDoiTuongMh", "TenDoiTuongMh");

            return View();
		}
		[Route("ThemSanPham")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPham(SanPham sanPham)
		{
            TempData["Message"] = "";
            bool checkTonTai = db.SanPhams.Any(x => x.MaSanPham == sanPham.MaSanPham);
			if (checkTonTai)
			{
                TempData["Message"] = "Sản phẩm dã tồn tại !!";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
			}
			else
			{
				if (ModelState.IsValid)
				{
					db.SanPhams.Add(sanPham);
					db.SaveChanges();
					if (sanPham.FileUpload != null)
					{
						string masp = sanPham.MaSanPham.ToString();
						var filePath = Path.Combine(_webhost.WebRootPath, "Products/Image", sanPham.FileUpload.FileName);
						using var fileStream = new FileStream(filePath, FileMode.Create);
						sanPham.FileUpload.CopyTo(fileStream);
						string _FileName = sanPham.FileUpload.FileName;
						SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == masp);
						sp.HinhAnhAvatar = _FileName;
						db.SaveChanges();
                       
                    }
                    TempData["Message"] = "Thêm sản phẩm thành công !!!";

                    return RedirectToAction("DanhMucSanPham", "HomeAdmin");
				}
				return View(sanPham);
			}

		}

        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            TempData["Message"] = "";
            var listChiTiet = db.ChiTietSanPhams.Where(x => x.MaSanPham == maSanPham).ToList();
            foreach (var item in listChiTiet)
            {
                if (db.ChiTietSanPhams.Where(x => x.MaSanPham == item.MaSanPham) != null)
                {
                    TempData["Message"] = "Không xóa được sản phẩm !!!";
                    return RedirectToAction("DanhMucSanPham" , "HomeAdmin");
                }
            }
            if (listChiTiet != null) db.RemoveRange(listChiTiet);
            if (maSanPham != null)
            {
                db.RemoveRange(db.SanPhams.Find(maSanPham));
            }

            db.SaveChanges();
            TempData["Message"] = "Xóa sản phẩm thành công !!!";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");
        }
		[Route("SuaSanPham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSanPham)
		{
			ViewBag.MaLoaiSp = new SelectList(db.LoaiSps.ToList(), "MaLoaiSp", "TenLoaiSp");
			ViewBag.MaDoiTuongMh = new SelectList(db.DoiTuongMhs.ToList(), "MaDoiTuongMh", "TenDoiTuongMh");
            //SanPham sanPham = db.SanPhams.SingleOrDefault(x => x.MaSanPham == maSanPham);
            var sanPham = db.SanPhams.Find(maSanPham);
            return View(sanPham);
		}

		[Route("SuaSanPham")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(SanPham sanPham)
		{
            TempData["Message"] = "";
            if (ModelState.IsValid)
			{
				db.Entry(sanPham).State = EntityState.Modified;
				if (sanPham.FileUpload != null)
				{
					string masp = sanPham.MaSanPham.ToString();
					var filePath = Path.Combine(_webhost.WebRootPath, "Products/Image", sanPham.FileUpload.FileName);
					using var fileStream = new FileStream(filePath, FileMode.Create);
					sanPham.FileUpload.CopyTo(fileStream);
					string _FileName = sanPham.FileUpload.FileName;
					SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSanPham == masp);
					sp.HinhAnhAvatar = _FileName;
				}
				db.SaveChanges();
                TempData["Message"] = "Sửa sản phẩm thành công !!!";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
			}
			return View(sanPham);
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
