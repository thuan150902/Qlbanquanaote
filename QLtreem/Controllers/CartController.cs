using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QLtreem.Models;


namespace QLtreem.Controllers
{
    public class CartController : Controller
    {
        QltreEmContext db=new QltreEmContext();
        public IActionResult Index()
        {
            return View();
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

        // Lấy cart từ Session (danh sách CartItem)
        List<Cart> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<Cart>>(jsoncart);
            }
            return new List<Cart>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<Cart> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        /// Thêm sản phẩm vào cart
        public IActionResult AddToCart(string maSanPham)
        {

            var sanpham = db.SanPhams
                .Where(p => p.MaSanPham == maSanPham)
                .FirstOrDefault();
            if (sanpham == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.SanPham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.qty++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new Cart() { qty = 1, SanPham = sanpham });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Cart));
        }

        /// xóa item trong cart
        public IActionResult RemoveCart(string maSanPham)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.SanPham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        /// Cập nhật
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart(string maSanPham, int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.SanPham.MaSanPham == maSanPham);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.qty = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        // Hiện thị giỏ hàng
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

    }
}
