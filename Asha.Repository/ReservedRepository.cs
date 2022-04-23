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
    public class ReservedRepository
    {
        private DemoDBEntities db = new DemoDBEntities();

        public List<ORDER> GetList()
        {
            return db.ORDER.ToList(); 
        }

        public IPagedList<ORDER> GetList(string keyword, string status,string mobile, DateTime? min_date, DateTime? max_date, int PageIndex, int PageSize)
        {
            var query = db.ORDER.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.CUSTOMER.NAME.Contains(keyword) || x.CUSTOMER.LINE_NAME.Contains(keyword));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(x => x.STATUS.Equals(status));
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                query = query.Where(x => x.CUSTOMER.MOBILE.Equals(mobile));
            }
            if (min_date != null)
            {
                query = query.Where(x => x.DATE >= min_date);
            }
            if (max_date != null)
            {
                query = query.Where(x => x.DATE <= max_date);
            }
            query = query.OrderBy(x => x.DATE).ThenByDescending(x=>x.SERVE_TIME.TIME);

            return query.ToPagedList(PageIndex, PageSize);
        }


        public ORDER Get(int id)
        {
            return db.ORDER.Where(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public void Create(ORDER model)
        {
            db.ORDER.Add(model);
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }

        public void Update(ORDER model)
        {
            try
            {
                var target = db.ORDER.Find(model.ID);
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
            var target = db.ORDER.Find(id);
            db.Entry(target).State = EntityState.Deleted;
            db.SaveChanges();
        }
    }
}
