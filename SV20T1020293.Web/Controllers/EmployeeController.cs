using Microsoft.AspNetCore.Mvc;

namespace SV20T1020293.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(string id)
        {
            return View();
        }
    }
}
