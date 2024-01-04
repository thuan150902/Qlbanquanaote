using Microsoft.AspNetCore.Mvc;
using QLtreem.Repository;

namespace QLtreem.ViewComponents
{
	public class LoaiSPMenuViewComponent : ViewComponent
	{
		private readonly ILoaiSPRepository _loaiSPRepository;
		public LoaiSPMenuViewComponent(ILoaiSPRepository loaiSPRepository)
		{
			_loaiSPRepository = loaiSPRepository;
		}

		public IViewComponentResult Invoke()
		{
			var loaisps = _loaiSPRepository.GetAllLoaiSp().OrderBy(x => x.TenLoaiSp);
			return View(loaisps);
		}
	}
}
