using QLtreem.Models;
namespace QLtreem.Repository
{
	public interface ILoaiSPRepository
	{
		LoaiSp Add(LoaiSp loaisp);
		LoaiSp Update(string MaLoai);
		LoaiSp Delete(string MaLoai);
		LoaiSp GetLoaiSp(string MaLoai);
		IEnumerable<LoaiSp> GetAllLoaiSp();
	}
}
