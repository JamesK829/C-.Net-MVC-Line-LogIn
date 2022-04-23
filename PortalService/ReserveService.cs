using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asha.Repository;
using Asha.Model;
using Asha.Util;

namespace PortalService
{
    public class ReserveService
    {
        ProductRepository rpository = new ProductRepository();
        CustomerRepository customerRepository = new CustomerRepository();


        public List<PRODUCT_CATEGORY> GetCategoryList(string status)
        {
            return rpository.GetFrontList(status);
        }

        public List<PRODUCT> GetProductList(string id,string status)
        {
            return rpository.GetProductList(id,status);
        }

        public List<SERVE_TIME> GetServeTime()
        {
            return rpository.GetServeTime();
        }

        public string CreateOrder(ORDER data)
        {
            data.ID= IdUtil.GetId32();

            return customerRepository.CreateOrder(data);
        }

        public CUSTOMER GetCus(string id)
        {
            return rpository.GetCus(id);
        }
        public ORDER GetRecentlyOrder(string cusId, string id)
        {
            return rpository.GetRecentlyOrder(cusId,id);
        }

        public List<ORDER> GetOrderList(string cusId)
        {
            return rpository.GetOrderList(cusId);
        }

        public List<ORDER> GetReservedTime(int year, int month)
        {
            return rpository.GetReservedTime(year,month);
        }

        public ORDER CheckReserved(DateTime date,int time)
        {
            return rpository.CheckReserved(date, time);
        }

        public void DeleteOrder(string id)
        {
            rpository.DeleteOrder(id);
        }
    }
}
