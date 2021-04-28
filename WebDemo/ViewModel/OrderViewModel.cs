using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDemo.ViewModel
{
    public class OrderViewModel
    {
        public long ID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ShipAddress { get; set; }
        public string ShipName { get; set; }
        public string CustomerID { get; set; }
        public long? UserID { get; set; }
        public string ShipMobile { get; set; }
        public string ShipEmail { get; set; }
        public bool? Status { get; set; }
        public decimal? Price { get; set; }
        public string Product { get; set; }
        
        public int? Quantity { get; set; }

        public decimal? Total { get; set; }
    }
}