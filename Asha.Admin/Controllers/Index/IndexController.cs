using AshaAdmin.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Asha.Admin.Controllers.Index
{
    public class IndexController : BaseController
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }
    }
}