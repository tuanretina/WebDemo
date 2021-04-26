using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class ProductDao
    {
        OnlineShop db = null;
        public ProductDao()
        {
            db = new OnlineShop();
        }
        public long Insert(Product entity)
        {
            db.Products.Add(entity);
            db.SaveChanges();
            return entity.ID;

        }

        public bool Update(Product entity)
        {
            try
            {
                var product = db.Products.Find(entity.ID);


                product.CategoryID = entity.CategoryID;
                product.Code = entity.Code;
                product.CreatedBy = entity.CreatedBy;
                product.CreatedDate = entity.CreatedDate;
                product.Description = entity.Description;
                product.Detail = entity.Detail;
                product.Image = entity.Image;
                product.IncludeVAT = entity.IncludeVAT;
                product.MetaDescriptions = entity.MetaDescriptions;
                product.MetaKeywords = entity.MetaKeywords;
                product.MetaTitile = entity.MetaTitile;
                product.ModifiedBy = entity.ModifiedBy;
                product.ModifiedDate = entity.ModifiedDate;
                product.MoreImages = entity.MoreImages;
                product.Name = entity.Name;
                product.Price = entity.Price;
                product.PromotionPrice = entity.PromotionPrice;
                product.Quantity = entity.Quantity;
                product.Status = entity.Status;
                product.Tophot = entity.Tophot;
                product.ViewCount = entity.ViewCount;
                product.Warranty = entity.Warranty;
           
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Product> ListAllPaging(string seacrhString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(seacrhString))
            {
                model = model.Where(x => x.Name.Contains(seacrhString) || x.Name.Contains(seacrhString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }


      

        public Product GetById(string productname)
        {
            return db.Products.SingleOrDefault(x => x.Name == productname);
        }

        public Product ViewDetail(int id)
        {
            return db.Products.Find(id);
        }

       

        public bool Delete(int id)
        {
            try
            {
                var product = db.Products.Find(id);
                db.Products.Remove(product);
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
