using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asha.Repository;
using Asha.Model;

namespace Asha.Service
{
    public class ManagerService
    {
        private ManagerRepository repository = new ManagerRepository();

        public bool CheckUser(string cid,string auth)
        {
            if (repository.GetUserInfo(cid, auth)==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public MANAGER GetMenager(string cid, string auth)
        {
            return repository.GetUserInfo(cid, auth);
        }

    }
}
