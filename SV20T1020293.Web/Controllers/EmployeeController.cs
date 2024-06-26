﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using SV20T1020293.BusinessLayers;
using SV20T1020293.DomainModels;
using SV20T1020293.Web.Models;
using System.Globalization;

namespace SV20T1020293.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        const string EMPLOYEE_SEARCH = "employee_search"; //Tên biến session dùng để lưu lại điều kiện tìm kiếm

        public IActionResult Index()
        {
            //Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            //Nếu có thì sử dụng lại điều kiện tìm kiếm, ngược lại thì tìm kiếm theo điều kiện mặc định
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);

            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");

            var model = new EmployeeSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            // Lưu lại vào session điều kiện tìm kiếm
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";

            var model = new Employee()
            {
                EmployeeID = 0,
                Photo = "nophoto.png",
                BirthDate = new DateTime(2001,8,8)
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin nhân viên";

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(model.Photo))
            {
                model.Photo = "nophoto.png";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee model, string birthDateInput = "", IFormFile? uploadPhoto = null)
        {
            if (string.IsNullOrWhiteSpace(model.FullName))
                ModelState.AddModelError(nameof(model.FullName), "Tên khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Số điện thoại không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError(nameof(model.Address), "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.BirthDate.ToString()))
                ModelState.AddModelError(nameof(model.BirthDate), "Ngày sinh không được để trống");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                return View("Edit", model);
            }

            // Xử lý ngày sinh
            DateTime? d = birthDateInput.ToDateTime();
            if (d.HasValue)
            {
                model.BirthDate = d.Value;
            }

            // Xử lý ảnh upload: Nếu có ảnh được upload thì lưu ảnh lên server, gán tên file ảnh đã lưu cho model.Photo
            if (uploadPhoto != null)
            {
                // Tên file sẽ lưu trên server 
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";

                // Đường dẫn vật lý đến file sẽ lưu trên server (vd: D:\MyWeb\wwwroot\images\employees\photo.png)
                string filePath = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, @"images\employees", fileName);

                // Lưu file lên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }

                // Gán tên file ảnh cho model.Photo
                model.Photo = fileName;
            }

            if (model.EmployeeID == 0)
            {
                int id = CommonDataService.AddEmployee(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Email", "Email bị trùng");
                    ViewBag.Title = "Bổ sung nhân viên";
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được nhân viên. Có thể email bị trùng");
                    ViewBag.Title = "Cập nhật thông tin nhân viên";
                    return View("Edit", model);
                }
            }

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
