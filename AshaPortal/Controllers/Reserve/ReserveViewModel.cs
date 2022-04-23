using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Asha.Model;
using System.Web.Mvc;

namespace AshaPortal
{
    public class ReserveViewModel
    {
        public string Id { get; set; }
        public List<Category> CategoryList { get; set; }
        public List<Product> ProductList { get; set; }
    }

    public class Category
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class Product
    {
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }

    public class DateForm
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
        public string Line_Id { get; set; }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string ProductId { get; set; }
        public Dictionary<int, string> Time { get; set; }
        public Dictionary<int, string> ReservedTime { get; set; }
    }

    public class ReserveList
    {
        public List<Reserve> ReserveListData { get; set; }
        public Reserve RecentlyReserve { get; set; }
    }

    public class Reserve
    {
        public string ReserveDate { get; set; }
        public string ReserveTime { get; set; }
        public string ReserveProd { get; set; }
        public string ReserveCategory { get; set; }
        public string ReserveId { get; set; }
    }


}