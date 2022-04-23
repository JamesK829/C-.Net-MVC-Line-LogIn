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
    public class CategoryRepository
    {
        private DemoDBEntities db = new DemoDBEntities();

        public IPagedList<PRODUCT_CATEGORY> GetList(string keyword, string status, int PageIndex, int PageSize)
        {
            var query = db.PRODUCT_CATEGORY.AsQueryable();
            try
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(x => x.CATEGORY.Contains(keyword));
                }
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(x => x.STATUS.Equals(status));
                }
                query = query.OrderByDescending(x => x.MODIFY_TIME);
            }
            catch (Exception e)
            {
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "RoleEdit" + e);
            }
            var list= query.ToPagedList(PageIndex, PageSize);
            return list;
        }


        public PRODUCT_CATEGORY Get(string id)
        {
            return db.PRODUCT_CATEGORY.Where(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public void Create(PRODUCT_CATEGORY model)
        {
            db.PRODUCT_CATEGORY.Add(model);
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }

        public void Update(PRODUCT_CATEGORY model)
        {
            try
            {
                var target = db.PRODUCT_CATEGORY.Find(model.ID);
                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "RoleEdit" + e);
            }
        }

        public void UpdateStatus(string id, string status, string modifier)
        {
            var target = db.PRODUCT_CATEGORY.Find(id);
            target.MODIFIER = modifier;
            db.Entry(target).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(string id)
        {
            var target = db.PRODUCT_CATEGORY.Find(id);
            db.Entry(target).State = EntityState.Deleted;
            db.SaveChanges();
        }
    }
}
