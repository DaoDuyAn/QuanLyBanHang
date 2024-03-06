using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks.Dataflow;

namespace SV20T1020293.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "An",
                Birthday = new DateTime(1990, 10, 20),
                Salary = 500.25m
            };

            return View(model);
        }

        public IActionResult Save(Models.Person model) 
        {
            return Json(model);
        }
    }
}
