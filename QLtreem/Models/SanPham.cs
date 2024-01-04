using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLtreem.Models;

public partial class SanPham
{
    public string MaSanPham { get; set; } = null!;

    public string? TenSanPham { get; set; }

    public string? ChatLieu { get; set; }

    public double? GiaNhap { get; set; }

    public double? GiaBan { get; set; }

    public string? HinhAnhAvatar { get; set; }

    public string? MaLoaiSp { get; set; }

    public string? MaDoiTuongMh { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();

    public virtual DoiTuongMh? MaDoiTuongMhNavigation { get; set; }

    public virtual LoaiSp? MaLoaiSpNavigation { get; set; }
    [Display(Name = "File Upload")]
    [NotMapped]
    public IFormFile? FileUpload { get; set; }
}
