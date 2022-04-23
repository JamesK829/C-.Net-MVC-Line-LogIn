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
    public class CustomerService
    {
        CustomerRepository repository = new CustomerRepository();

        public CUSTOMER GetCustomer(string lineId, string mobile)
        {
            return repository.GetCustomer(mobile, lineId);
        }

        public bool CheckPhone(string phone)
        {
            return repository.CheckPhone(phone);
        }

        public CUSTOMER CheckData(string phone, string auth)
        {
            return repository.CheckData(phone, auth);
        }

        public CUSTOMER CheckLineId(string id, string name)
        {
            return repository.CheckLineId(id,name);
        }

        public string CreateCustomerData(CUSTOMER data)
        {
            data.ID = IdUtil.GetId32();
            return repository.CreateCustomerData(data);
        }
    }
}
