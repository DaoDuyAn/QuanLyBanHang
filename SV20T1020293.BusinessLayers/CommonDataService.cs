using SV20T1020293.DataLayers;
using SV20T1020293.DataLayers.SQLServer;
using SV20T1020293.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020293.BusinessLayers
{
    /// <summary>
    /// Cung cấp các chức năng nghiệp vụ liên quan đến các dữ liệu "chung"
    /// (tỉnh/thành, khách hàng, nhà cung cấp, loại hàng, người giao hàng, nhân viên)
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> custommerDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;
            provinceDB = new ProvinceDAL(connectionString);
            custommerDB = new CustomerDAL(connectionString);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount">Tham số đầu ra cho biết số dòng dữ liệu tìm được</param>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (rỗng nếu lấy toàn bộ khách hàng)</param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = custommerDB.Count(searchValue);

            return custommerDB.List(page, pageSize, searchValue).ToList();
        }


    }
}
