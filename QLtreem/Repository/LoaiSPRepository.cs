using QLtreem.Models;

namespace QLtreem.Repository
{
	public class LoaiSPRepository : ILoaiSPRepository
	{
		private readonly QltreEmContext context;
		public LoaiSPRepository(QltreEmContext context)
		{
			this.context = context;
		}
		public LoaiSp Add(LoaiSp loaisp)
		{
			throw new NotImplementedException();
		}

		public LoaiSp Delete(string MaLoai)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<LoaiSp> GetAllLoaiSp()
		{
			return context.LoaiSps;
		}

		public LoaiSp GetLoaiSp(string MaLoai)
		{
			throw new NotImplementedException();
		}

		public LoaiSp Update(string MaLoai)
		{
			throw new NotImplementedException();
		}
	}
}
