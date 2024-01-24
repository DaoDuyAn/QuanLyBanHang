using Microsoft.AspNetCore.Mvc;

namespace SV20T1020293.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            return View("Edit");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View();
        }
    }
}
