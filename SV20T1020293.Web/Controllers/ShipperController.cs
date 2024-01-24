using Microsoft.AspNetCore.Mvc;

namespace SV20T1020293.Web.Controllers
{
    public class ShipperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung người giao hàng";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            return View();
        }
    }
}
