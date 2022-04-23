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
    public class TimeRepository
    {
        private DemoDBEntities db = new DemoDBEntities();

        public List<SERVE_TIME> GetList()
        {

            return db.SERVE_TIME.ToList(); 
        }


        public SERVE_TIME Get(int id)
        {
            return db.SERVE_TIME.Where(x => x.ID.Equals(id)).FirstOrDefault();
        }

        public void Create(SERVE_TIME model)
        {
            db.SERVE_TIME.Add(model);
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }

        public void Update(SERVE_TIME model)
        {
            try
            {
                var target = db.SERVE_TIME.Find(model.ID);
                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "RoleEdit" + e);
            }
        }


        public void Delete(int id)
        {
            var target = db.SERVE_TIME.Find(id);
            db.Entry(target).State = EntityState.Deleted;
            db.SaveChanges();
        }
    }
}
