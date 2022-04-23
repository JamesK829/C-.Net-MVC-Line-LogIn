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
    public class TimeService
    {
        private TimeRepository repository = new TimeRepository();

        public List<SERVE_TIME> GetList()
        {
            return repository.GetList();
        }

        public SERVE_TIME Get(int id)
        {
            return repository.Get(id);
        }

        public void Save(SERVE_TIME data)
        {
            if (data.ID!=0)
            {
                for (int i = 1; i < 32; i++)
                {
                    data.DATE = i;
                repository.Create(data);
                }

            }
            else
            {
                for (int i = 1; i < 32; i++)
                {
                    data.DATE = i;
                    repository.Update(data);
                }
            }
        }

        public void Delete(int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                repository.Delete(ids[i]);
            }
        }

    }
}
