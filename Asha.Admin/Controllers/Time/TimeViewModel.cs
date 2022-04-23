using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Asha.Model;

namespace AshaAdmin
{
    public class TimeViewModel
    {
        public int Id { get; set; }
        public List<TimeForm> List { get; set; }
    }

    public class TimeForm
    {
        public string Time { get; set; }
        public int Date { get; set; }
        public int ID { get; set; }

    }
}