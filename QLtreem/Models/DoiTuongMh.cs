using System;
using System.Collections.Generic;

namespace QLtreem.Models;

public partial class DoiTuongMh
{
    public string MaDoiTuongMh { get; set; } = null!;

    public string? TenDoiTuongMh { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
