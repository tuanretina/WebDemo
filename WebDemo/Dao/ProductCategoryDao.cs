using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class ProductCategoryDao
    {
        OnlineShop db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShop();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DislayOrder).ToList();
        }

        public ProductCategory ViewDetail(long id)
        {
            return db.ProductCategories.Find(id);
        }
        public IEnumerable<ProductCategory> ListAllPaging(string seacrhString, int page, int pageSize)
        {
            IQueryable<ProductCategory> model = db.ProductCategories;
            if (!string.IsNullOrEmpty(seacrhString))
            {
                model = model.Where(x => x.Name.Contains(seacrhString) || x.Name.Contains(seacrhString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public long Insert(ProductCategory entity)
        {
            db.ProductCategories.Add(entity);
            db.SaveChanges();
            return entity.ID;

        }

        public bool Update(ProductCategory entity)
        {
            try
            {
                var cate = db.ProductCategories.Find(entity.ID);

                cate.ID = entity.ID;


                cate.Name = entity.Name;
                cate.MetaTitile = entity.MetaTitile;
                cate.CreatedBy = entity.CreatedBy;
                cate.CreatedDate = DateTime.Now;
                cate.ModifiedBy = entity.ModifiedBy;
                cate.ModifiedDate = DateTime.Now;   
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var cate = db.ProductCategories.Find(id);
                db.ProductCategories.Remove(cate);
                db.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }


    }
}