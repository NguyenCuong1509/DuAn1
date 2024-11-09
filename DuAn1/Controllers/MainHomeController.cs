using Microsoft.AspNetCore.Mvc;

namespace DuAn1.Controllers
{
    public class MainHomeController : Controller
    {
        public IActionResult TrangChu()
        {
            return View();
        }
    }
}
