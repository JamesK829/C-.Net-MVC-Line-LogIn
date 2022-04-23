using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asha.Model;

namespace AshaAdmin
{
    public class CategoryViewModel
    {
        public CategorySearchModel SearchParameter { get; set; }
        public IPagedList<CategoryForm> List { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class CategorySearchModel
    {
        public string Keyword { get; set; }
        public string Status { get; set; }
    }

    public class CategoryForm
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public DateTime Create_Time { get; set; }
        public DateTime Modify_Time { get; set; }
        public string Modifier { get; set; }
        public string Creator { get; set; }

    }
}