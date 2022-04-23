using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asha.Model;
using System.Data.Entity;

namespace Asha.Repository
{
    public class CustomerRepository
    {
        DemoDBEntities db = new DemoDBEntities();

        public CUSTOMER GetCustomer(string mobile, string lineId)
        {
            var query = db.CUSTOMER.AsQueryable();
            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x => x.MOBILE.Equals(mobile));
            }
            if (!string.IsNullOrEmpty(lineId))
            {
                query = query.Where(x => x.LINE_ID.Equals(lineId));
            }
            return query.FirstOrDefault();
        }

        public bool CheckPhone(string phone)
        {
            var query = db.CUSTOMER.AsQueryable();
            if (!string.IsNullOrEmpty(phone))
            {
                if (query.Where(x => x.MOBILE.Equals(phone)).FirstOrDefault() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public CUSTOMER CheckData(string phone, string auth)
        {
            var query = db.CUSTOMER.AsQueryable();
            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(x => x.MOBILE.Equals(phone) && x.AUTH.Equals(auth));
            }
            return query.FirstOrDefault();
        }

        public CUSTOMER CheckLineId(string id,string name)
        {
            var query = db.CUSTOMER.AsQueryable();
            if (!string.IsNullOrEmpty(id))
            {
                var z = query.Where(x => x.LINE_ID.Equals(id) && x.LINE_NAME.Equals(name)).Select(x => x.ID).FirstOrDefault();
                if (!string.IsNullOrEmpty(z))
                {
                    return query.Where(x => x.LINE_ID.Equals(id) && x.LINE_NAME.Equals(name)).FirstOrDefault();
                }
            }
            //var a=query.FirstOrDefault();
            return null;
        }

        public string CreateCustomerData(CUSTOMER data)
        {
            db.CUSTOMER.Add(data);
            db.Entry(data).State = EntityState.Added;
            db.SaveChanges();
            return data.ID;
        }

        public string CreateOrder(ORDER data)
        {
            db.ORDER.Add(data);
            db.Entry(data).State = EntityState.Added;
            db.SaveChanges();
            return data.ID;
        }

    }
}
