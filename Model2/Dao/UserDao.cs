using Model2.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model2.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;   

        }

        public bool Login(string userName, string password)
        {

            var result = db.Users.Count(x => x.UserName == userName && x.Password == password);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }
    }
}
