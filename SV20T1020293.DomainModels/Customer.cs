using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020293.DomainModels
{
    /// <summary>
    /// Khách hàng
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string Province { set; get; } = "";
        public string Address { set; get; } = "";
        public string Phone { set; get; } = "";
        public string Email { set; get; } = "";
        public bool IsLocked { set; get; }

    }
}
