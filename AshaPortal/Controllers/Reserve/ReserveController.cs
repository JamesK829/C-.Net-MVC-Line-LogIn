using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Asha.Util;
using PortalService;
using Asha.Model;

namespace AshaPortal
{
    [Authorize]
    public class ReserveController : BaseController
    {

        ReserveService service = new ReserveService();
        // GET: Reserve
        public ActionResult Product()
        {
            var model = new ReserveViewModel();
            try
            {
                List<Category> List = new List<Category>();
                var CategoryList = service.GetCategoryList(SysConstants.ENABLE_STATUS);
                foreach (var item in CategoryList)
                {
                    var Category = new Category();
                    Category.CategoryId = item.ID;
                    Category.CategoryName = item.CATEGORY;
                    List.Add(Category);
                }
                model.CategoryList = List;
                return View(model);
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Product");
        }

        [HttpPost]
        public ActionResult ProductPage(ReserveViewModel form)
        {
            try
            {
                var Model = new ReserveViewModel();
                var ProductList = service.GetProductList(form.Id, SysConstants.ENABLE_STATUS);
                LogUtil.Info(MethodBase.GetCurrentMethod(), "Product: " + form.Id);
                List<Product> List = new List<Product>();

                foreach (var item in ProductList)
                {
                    var Product = new Product();
                    Product.CategoryId = item.CATEGORY_ID;
                    Product.ProductId = item.ID;
                    Product.ProductName = item.PRODUCT1;
                    List.Add(Product);
                }
                Model.ProductList = List;
                return PartialView(Model);
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return new EmptyResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Product(string cId, string pId)
        {
            return Json(new { redirectToUrl = Url.Action("Reserve", "Reserve", new { pId = pId }) });
        }

        public ActionResult Reserve(string pId, int year = 0, int month = 0)
        {
            var model = new DateForm();
            try
            {
                model.ProductId = pId;
                int thisYear = 0;
                int thisMonth = 0;
                if (year == 0 && month == 0)
                {
                    thisYear = DateTime.Now.Year;
                    thisMonth = DateTime.Now.Month;
                }
                else
                {
                    thisYear = year;
                    thisMonth = month;
                }
                model.Month = month;
                model.Year = year;
                var dataList = service.GetReservedTime(thisYear, thisMonth);
                Dictionary<int, string> timeList = new Dictionary<int, string>();
                List<int> rsTimeId = new List<int>();
                foreach (var rs in dataList)
                {
                    rsTimeId.Add(rs.TIME_ID);
                }
                var reserveTime = service.GetServeTime();
                foreach (var item in reserveTime)
                {
                    if (rsTimeId.Contains(item.ID))
                    {
                        timeList.Add(item.ID, item.DATE.ToString() + "," + item.TIME + "," + "disabled");
                    }
                    else
                    {
                        timeList.Add(item.ID, item.DATE.ToString() + "," + item.TIME + "," + "");

                    }
                }

                model.Time = timeList;
            }
            catch (Exception e)
            {
                TempData["Message"] = e.ToString();

                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservePtView(int year = 0, int month = 0)
        {
            var model = new DateForm();
            try
            {
                var reserveList = new List<Reserve>();
                int thisYear = 0;
                int thisMonth = 0;
                if (year == 0 && month == 0)
                {
                    thisYear = DateTime.Now.Year;
                    thisMonth = DateTime.Now.Month;
                }
                else
                {
                    thisYear = year;
                    thisMonth = month;
                }
                model.Month = month;
                model.Year = year;
                var dataList = service.GetReservedTime(thisYear, thisMonth);
                Dictionary<int, string> timeList = new Dictionary<int, string>();
                List<int> rsTimeId = new List<int>();
                foreach (var rs in dataList)
                {
                    rsTimeId.Add(rs.TIME_ID);
                }
                var reserveTime = service.GetServeTime();
                foreach (var item in reserveTime)
                {
                    if (rsTimeId.Contains(item.ID))
                    {
                        timeList.Add(item.ID, item.DATE.ToString() + "," + item.TIME + "," + "disabled");
                    }
                    else
                    {
                        timeList.Add(item.ID, item.DATE.ToString() + "," + item.TIME + "," + "");
                    }
                }

                model.Time = timeList;
            }
            catch (Exception e)
            {
                TempData["Message"] = e.ToString();

                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserve(string ProductId, int? date, int? month, int? year, int time)
        {
            var model = new ORDER();
            var form = new DateForm();
            var pId = ProductId;
            var orderId = "";
            var rDate = "";
            var rTime = "";
            var rPro = "";
            var rCat = "";
            try
            {
                var cus = GetLoginUserID();
                var ReserveDate = new DateTime((int)year, (int)month, (int)date);
                var checkReserve = service.CheckReserved(ReserveDate, time);
                form.ProductId = pId;
                var reserveTime = service.GetServeTime();
                Dictionary<int, string> timeList = new Dictionary<int, string>();
                foreach (var item in reserveTime)
                {
                    timeList.Add(item.ID, item.DATE.ToString() + "," + item.TIME);
                }
                form.Time = timeList;

                model.TIME_ID = time;
                model.PRODUCT_ID = ProductId;
                model.CUSTOMER_ID = cus;
                model.DATE = ReserveDate;
                model.CREATE_TIME = DateTime.Now;
                model.MODIFY_TIME = DateTime.Now;
                model.CREATOR = service.GetCus(cus).NAME;
                model.MODIFIER = service.GetCus(cus).NAME;
                model.STATUS = SysConstants.ENABLE_STATUS;
                orderId = service.CreateOrder(model);

                var recentlyData = service.GetRecentlyOrder(GetLoginUserID(), orderId);
                rDate = recentlyData.DATE.ToString("yyyy/MM/dd");
                rTime = recentlyData.SERVE_TIME.TIME;
                rPro = recentlyData.PRODUCT.PRODUCT1;
                rCat = recentlyData.PRODUCT.PRODUCT_CATEGORY.CATEGORY;
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            TempData["Message"] = "您已預定" + rCat + "-" + rPro + "於 " + rDate + " , 時段 " + rTime;

            return Json(new { redirectToUrl = Url.Action("Home", "Reserve") });
        }

        public ActionResult ReserveList(string id)
        {
            var model = new ReserveList();
            try
            {
                var reserveData = new Reserve();
                var recentlyData = service.GetRecentlyOrder(GetLoginUserID(), id);
                reserveData.ReserveDate = recentlyData.DATE.ToString("yyyy/MM/dd");
                reserveData.ReserveTime = recentlyData.SERVE_TIME.TIME;
                reserveData.ReserveProd = recentlyData.PRODUCT.PRODUCT1;
                reserveData.ReserveCategory = recentlyData.PRODUCT.PRODUCT_CATEGORY.CATEGORY;
                model.RecentlyReserve = reserveData;

                var reserveList = new List<Reserve>();
                var dataList = service.GetOrderList(id);
                foreach (var item in dataList)
                {
                    var reserve = new Reserve();
                    reserve.ReserveDate = item.DATE.ToString("yyyy/MM/dd");
                    reserve.ReserveTime = item.SERVE_TIME.TIME;
                    reserve.ReserveProd = item.PRODUCT.PRODUCT1;
                    reserve.ReserveCategory = item.PRODUCT.PRODUCT_CATEGORY.CATEGORY;
                    reserveList.Add(reserve);
                }
                model.ReserveListData = reserveList;
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return View(model);
        }

        public ActionResult ReserveData()
        {

            var model = new ReserveList();
            try
            {

                var reserveList = new List<Reserve>();
                var dataList = service.GetOrderList(GetLoginUserID());
                foreach (var item in dataList)
                {
                    var reserve = new Reserve();
                    reserve.ReserveDate = item.DATE.ToString("yyyy/MM/dd");
                    reserve.ReserveTime = item.SERVE_TIME.TIME;
                    reserve.ReserveProd = item.PRODUCT.PRODUCT1;
                    reserve.ReserveCategory = item.PRODUCT.PRODUCT_CATEGORY.CATEGORY;
                    reserve.ReserveId = item.ID;
                    reserveList.Add(reserve);
                }
                model.ReserveListData = reserveList;
            }
            catch (Exception e)
            {
                LogUtil.Error(MethodBase.GetCurrentMethod(), e);
            }
            return View(model);
        }

        public ActionResult Home()
        {
            var model = new HomeVIewModel();
            model.CId = GetLoginUserID();
            return View(model);
        }

        public ActionResult DeleteOrder(string id)
        {
            service.DeleteOrder(id);

            return RedirectToAction("ReserveData", "Reserve");
        }
    }


}