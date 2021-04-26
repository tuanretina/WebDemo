using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class OrderDetailDao
    {
        OnlineShop db = null;
        public OrderDetailDao()
        {
            db = new OnlineShop();
        }
        public bool Insert(OrderDetail detail)
        {
            try
            {
                db.OrderDetails.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
          

        }
    }
}