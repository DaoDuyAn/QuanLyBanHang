using SV20T1020293.DomainModels;

namespace SV20T1020293.Web.Models
{
    /// <summary>
    /// Lớp cơ sở cho các lớp biểu diễn là kết quả của thao tác
    /// tìm kiếm, phân trang
    /// </summary>
    public abstract class BasePaginationResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount { 
            get
            {
                if (PageSize == 0)
                    return 1;

                int c = RowCount / PageSize;
                if (RowCount / PageSize > 0)
                    c += 1;

                return c;
            }
                
        }  
    }

    /// <summary>
    /// Kết quả tìm kiếm và lấy danh sách khách hàng
    /// </summary>
    public class CustomerSearchResult : BasePaginationResult
    {
        public List<Customer>? Data { get; set; }
    }
}
