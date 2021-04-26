using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class MenuDao
    {

            OnlineShop db = null;
            public MenuDao()
            {
                db = new OnlineShop();
            }

        public List<Menu> ListByGroupId(int groupId)
        {
            return db.Menus.Where(x => x.TypeID == groupId & x.Status == true).OrderBy(x=>x.DislayOrder).ToList();
        }
    }
}