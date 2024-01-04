using System;
using System.Collections.Generic;

namespace QLtreem.Models;

public partial class ChiTietHoaDonBan
{
    public string MaHoaDonBan { get; set; } = null!;

    public string MaSanPham { get; set; } = null!;

    public string MaMauSac { get; set; } = null!;

    public string MaKichThuoc { get; set; } = null!;

    public int? SoLuong { get; set; }

    public double? DonGiaBan { get; set; }

    public virtual ChiTietSanPham Ma { get; set; } = null!;

    public virtual HoaDonBan MaHoaDonBanNavigation { get; set; } = null!;
}
