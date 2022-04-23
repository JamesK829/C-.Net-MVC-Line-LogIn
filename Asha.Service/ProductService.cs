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
    public class ProductService
    {
        private ProductRepository repository = new ProductRepository();

        public IPagedList<PRODUCT> GetList(string keyword, string status,string category, int PageIndex, int PageSize)
        {
            var roleList = repository.GetList(keyword, status, category,PageIndex, PageSize);
            return roleList;
        }

        public PRODUCT_CATEGORY GetCategory(string id)
        {
            return repository.GetCategory(id);
        }

        public Dictionary<string, string> GetCategoryList(string status)
        {
            return repository.GetCategoryList(status);
        }

        public PRODUCT Get(string id)
        {
            return repository.Get(id);
        }

        public void Save(PRODUCT data)
        {
            data.MODIFY_TIME = DateTime.Now;
            if (string.IsNullOrEmpty(data.ID))
            {
                data.ID = IdUtil.GetUniqueKey();
                data.PRODUCT_CATEGORY = repository.GetCategory(data.CATEGORY_ID);
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
