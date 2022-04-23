using Asha.Model;
using Asha.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;

namespace Asha.Repository
{
    public class ProductRepository
    {
        private DemoDBEntities db = new DemoDBEntities();

        public IPagedList<PRODUCT> GetList(string keyword, string status, string category, int PageIndex, int PageSize)
        {
            var query = db.PRODUCT.AsQueryable();
            try
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => x.PRODUCT1.Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(x => x.STATUS.Equals(status));
                }
                if (!string.IsNullOrWhiteSpace(category))
                {
                    query = query.Where(x => x.CATEGORY_ID.Equals(category));
                }
                query = query.OrderByDescending(x => x.MODIFY_TIME);
            }
            catch (Exception e)
            {
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "RoEdit" + e);
            }
            var list = query.ToPagedList(PageIndex, PageSize);
            return list;
        }

        public PRODUCT_CATEGORY GetCategory(string id)
        {
            return db.PRODUCT_CATEGORY.Where(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public Dictionary<string, string> GetCategoryList(string status)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            var list = db.PRODUCT_CATEGORY.Where(x => x.STATUS.Equals(status)).ToList();
            foreach (var item in list)
            {
                options.Add(item.ID, item.CATEGORY);
            }

            return options;
        }

        public PRODUCT Get(string id)
        {
            return db.PRODUCT.Where(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public void Create(PRODUCT model)
        {
            db.PRODUCT.Add(model);
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }

        public void Update(PRODUCT model)
        {
            try
            {
                var target = db.PRODUCT.Find(model.ID);
                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "RoleEdit" + e);
            }
        }

        public void Delete(string id)
        {
            var target = db.PRODUCT.Find(id);
            db.Entry(target).State = EntityState.Deleted;
            db.SaveChanges();
        }

        #region Portal
        public List<PRODUCT_CATEGORY> GetFrontList(string status)
        {
            return db.PRODUCT_CATEGORY.Where(x => x.STATUS.Equals(status)).ToList();
        }

        public List<PRODUCT> GetProductList(string id, string status)
        {
            return db.PRODUCT.Where(x => x.STATUS.Equals(status) && x.PRODUCT_CATEGORY.ID.Equals(id)).OrderByDescending(x => x.seq).ToList();
        }

        public List<SERVE_TIME> GetServeTime()
        {
            return db.SERVE_TIME.OrderBy(x => x.TIME).ToList();
        }

        public CUSTOMER GetCus(string id)
        {
            return db.CUSTOMER.Where(x => x.ID.Equals(id) && x.STATUS.Equals("1")).FirstOrDefault();
        }

        public ORDER GetRecentlyOrder(string cusId, string id)
        {
            return db.ORDER.Where(x => x.CUSTOMER_ID.Equals(cusId) && x.ID.Equals(id)).FirstOrDefault();
        }

        public List<ORDER> GetOrderList(string cusId)
        {
            return db.ORDER.Where(x => x.CUSTOMER_ID.Equals(cusId) && x.DATE >= DateTime.Now).ToList();
        }

        public List<ORDER> GetReservedTime(int year, int month)
        {
            var query = db.ORDER.AsQueryable();
            var list = query.Where(x => x.DATE.Year == year && x.DATE.Month == month && x.DATE >= new DateTime(year, month, 1)).ToList();
            return list;
        }

        public ORDER CheckReserved(DateTime date, int time)
        {
            var query = db.ORDER.AsQueryable();
            query = query.Where(x => x.DATE==date && x.TIME_ID == time);
            return query.FirstOrDefault();
        }

        public void DeleteOrder(string id)
        {
            var target = db.ORDER.Find(id);
            db.Entry(target).State = EntityState.Deleted;
            db.SaveChanges();
        }
        #endregion

    }
}
