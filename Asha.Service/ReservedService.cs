using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asha.Repository;
using Asha.Model;
using Asha.Util;
using PagedList;

namespace Asha.Service
{
    public class ReservedService
    {
        private ReservedRepository repository = new ReservedRepository();

        public IPagedList<ORDER> GetList(string keyword, string status, string mobile, DateTime? min_date, DateTime? max_date, int PageIndex, int PageSize)
        {
            return repository.GetList(keyword,status,mobile,min_date,max_date,PageIndex,PageSize);
        }

        public ORDER Get(int id)
        {
            return repository.Get(id);
        }

        public void Delete(string[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                repository.Delete(ids[i]);
            }
        }

    }
}
