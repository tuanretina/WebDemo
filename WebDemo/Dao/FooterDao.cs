using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class FooterDao
    {
        OnlineShop db = null;
        public FooterDao()
        {
            db = new OnlineShop();
        }
        public Footer GetFooter()
        {
            return db.Footers.SingleOrDefault(x => x.Status == true);
        }
    }
}