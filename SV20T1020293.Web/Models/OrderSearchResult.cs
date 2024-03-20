using SV20T1020293.DomainModels;

namespace SV20T1020293.Web.Models
{
    // Biểu diễn dữ liệu kết quả tìm kiếm đơn hàng
    public class OrderSearchResult : BasePaginationResult
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
        public List<Order> Data { get; set; } = new List<Order>();
    }
}
