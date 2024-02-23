using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020293.BusinessLayers
{
    /// <summary>
    /// Khởi tạo và lưu trữ các thông tin cấu hình của BussinessLayer
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi thông số kết nối với CSDL
        /// </summary>
        public static string ConnectionString { get; private set; } = "";

        /// <summary>
        /// Hàm khởi tạo cấu hình cho BussinessLayer
        /// (Hàm này phải được gọi trước khi chạy ứng dụng)
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}
