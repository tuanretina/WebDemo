using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class CategoryDao
    {
        OnlineShop db = null;
        public CategoryDao()
        {
            db = new OnlineShop();
        }
        public List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();

        }

        public ProductCategory ViewDetail (long id)
        {
            return db.ProductCategories.Find(id);
        }
        public IEnumerable<Category> ListAllPaging(string seacrhString, int page, int pageSize)
        {
            IQueryable<Category> model = db.Categories;
            if (!string.IsNullOrEmpty(seacrhString))
            {
                model = model.Where(x => x.Name.Contains(seacrhString) || x.Name.Contains(seacrhString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}