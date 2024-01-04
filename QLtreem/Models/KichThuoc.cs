using System;
using System.Collections.Generic;

namespace QLtreem.Models;

public partial class KichThuoc
{
    public string MaKichThuoc { get; set; } = null!;

    public string? TenKichThuoc { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
