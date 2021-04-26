using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class GroupDao
    {
        OnlineShop db = null;
        public GroupDao()
        {
            db = new OnlineShop();
        }

        public List<UserGroup> ListAll()
        {
            return db.UserGroups.Where(x => x.Status == true).OrderBy(x => x.DislayOrder).ToList();
        }
    }
}