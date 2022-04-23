using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asha.Model;

namespace AshaAdmin
{

    public class ReservedViewModel
    {
        public ReservedSearchMoel SearchParameter { get; set; }
        public IPagedList<ASHA_ORDER> List { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Id { get; set; }
        public IPagedList<ReservedForm> ReservedList { get; set; }
        public string SUBMIT_STATUS { get; set; }
        public int Total { get; set; }
    }

    public class ReservedSearchMoel
    {
        public string Keyword { get; set; }
        public DateTime? Min_Date { get; set; }
        public DateTime? Max_Date { get; set; }
        public string Status { get; set; }
        public string mobile { get; set; }
    }

    public class ReservedForm
    {
        public string Time { get; set; }
        public DateTime? Date { get; set; }
        public string ID { get; set; }
        public string Product { get; set; }
        public string Name { get; set; }
        public string Line_Name { get; set; }
        public string Category { get; set; }
        public ProductForm AHSA_PRODUCT { get; set; }
        public CustomerForm ASHA_CUSTOMER { get; set; }
        public CategoryForm ASHA_CATEGORY { get; set; }
    }


    public class CustomerForm
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Line_Name { get; set; }
        public string Mobile { get; set; }
    }
}