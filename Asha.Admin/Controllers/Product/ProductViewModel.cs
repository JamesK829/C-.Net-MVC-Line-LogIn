using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asha.Model;

namespace AshaAdmin
{
    public class ProductViewModel
    {
        public ProductSearchModel SearchParameter { get; set; }
        public IPagedList<ProductForm> List { get; set; }
        public Dictionary<string, string> Category_List { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class ProductSearchModel
    {
        public string Keyword { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
    }

    public class ProductForm
    {
        public string Id { get; set; }
        public string Category_Id { get; set; }
        public string Product { get; set; }
        public string Status { get; set; }
        public DateTime Create_Time { get; set; }
        public DateTime Modify_Time { get; set; }
        public string Modifier { get; set; }
        public string Creator { get; set; }
        public string Category_Name { get; set; }
        public Dictionary<string, string> Category_List { get; set; }

    }
}