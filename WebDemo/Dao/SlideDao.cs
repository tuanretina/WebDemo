using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class SlideDao
    {
        OnlineShop db = null;
        public SlideDao()
        {
            db = new OnlineShop();
        }

        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(y => y.DislayOrder).ToList();
        }

    }
}