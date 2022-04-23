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
    public class CategoryService
    {
        private CategoryRepository repository = new CategoryRepository();

        public IPagedList<PRODUCT_CATEGORY> GetList(string keyword, string status, int PageIndex, int PageSize)
        {
            var roleList = repository.GetList(keyword, status, PageIndex, PageSize);
            return roleList;
        }

        public PRODUCT_CATEGORY Get(string id)
        {
            return repository.Get(id);
        }

        public void Save(PRODUCT_CATEGORY data)
        {
            data.MODIFY_TIME = DateTime.Now;
            if (string.IsNullOrEmpty(data.ID))
            {
                data.ID = IdUtil.GetUniqueKey();
                data.CREATE_TIME = DateTime.Now;
                repository.Create(data);
            }
            else
            {
                repository.Update(data);
            }
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
