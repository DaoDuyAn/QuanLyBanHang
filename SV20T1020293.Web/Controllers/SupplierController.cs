using Microsoft.AspNetCore.Mvc;

namespace SV20T1020293.Web.Controllers
{
    public class SupplierController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin nhà cung cấp";
            return View();
        }
    }
}
