﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SV20T1020293.BusinessLayers;
using SV20T1020293.DomainModels;
using SV20T1020293.Web.Models;

namespace SV20T1020293.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 20;
        const string CREATE_TITLE = "Bổ sung khách hàng";
        const string CUSTOMER_SEARCH = "customer_search"; //Tên biến session dùng để lưu lại điều kiện tìm kiếm

        public IActionResult Index()
        {
            //Kiểm tra xem trong session có lưu điều kiện tìm kiếm không
            //Nếu có thì sử dụng lại điều kiện tìm kiếm, ngược lại thì tìm kiếm theo điều kiện mặc định
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH);

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
            var data = CommonDataService.ListOfCustomers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");

            var model = new CustomerSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            // Lưu lại vào session điều kiện tìm kiếm
            ApplicationContext.SetSessionData(CUSTOMER_SEARCH, input);

            return View(model);
        }


        public IActionResult Create()
        {
            ViewBag.Title = CREATE_TITLE;

            var model = new Customer()
            {
                CustomerID = 0
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";

            var model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost] // Attribute => chỉ nhận dữ liệu gửi lên dưới dạng POST
        public IActionResult Save(Customer model) // ~ Viết tường minh (int customerID, string customerName, ...)
        {
            //TODO: Kiểm soát dữ liệu trong model xem có hợp lệ hay không?
            //Yêu cầu: Tên khách hàng, tên giao dịch, email và tỉnh thành không được để trống
            if (string.IsNullOrWhiteSpace(model.CustomerName))
                ModelState.AddModelError(nameof(model.CustomerName), "Tên khách hàng không được để trống");
            if (string.IsNullOrWhiteSpace(model.ContactName))
                ModelState.AddModelError(nameof(model.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrWhiteSpace(model.Address))
                ModelState.AddModelError(nameof(model.Address), "Địa chỉ không được để trống");
            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Số điện thoại không được để trống");
            if (string.IsNullOrWhiteSpace(model.Email))
                ModelState.AddModelError(nameof(model.Email), "Email không được để trống");
            if (string.IsNullOrWhiteSpace(model.Province))
                ModelState.AddModelError(nameof(model.Province), "Vui lòng chọn tỉnh/thành");

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.CustomerID == 0 ? CREATE_TITLE : "Cập nhật thông tin khách hàng";
                return View("Edit", model);
            }

            if (model.CustomerID == 0)
            {
                int id = CommonDataService.AddCustomer(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Email", "Email bị trùng");
                    ViewBag.Title = CREATE_TITLE;
                    return View("Edit", model);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateCustomer(model);
                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được khách hàng. Có thể email bị trùng");
                    ViewBag.Title = "Cập nhật thông tin khách hàng";
                    return View("Edit", model);
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
