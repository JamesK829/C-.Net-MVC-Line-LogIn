using Asha.Model;
using Asha.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Asha.Repository
{
    public class ManagerRepository
    {
        private DemoDBEntities db = new DemoDBEntities();

        public MANAGER GetUserInfo(string cid, string auth)
        {
            return db.MANAGER.Where(x => x.CID.Equals(cid) && x.AUTH.Equals(auth) && x.STATUS.Equals("1")).FirstOrDefault();
        }

    }
}
