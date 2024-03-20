using SV20T1020293.DomainModels;

namespace SV20T1020293.Web.Models
{
    // Biểu diễn dữ liệu sử dụng cho chức năng hiển thị chi tiết của đơn hàng (Order/Details):
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
