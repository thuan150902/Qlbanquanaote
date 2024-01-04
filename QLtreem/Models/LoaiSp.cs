using System;
using System.Collections.Generic;

namespace QLtreem.Models;

public partial class LoaiSp
{
    public string MaLoaiSp { get; set; } = null!;

    public string? TenLoaiSp { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
