using Asha.Util;
using System;
using System.Web.Mvc;
using Asha.Service;
using AutoMapper;
using Asha.Model;
using System.Collections.Generic;
using PagedList;

namespace AshaAdmin.Controllers
{
    [Authorize]
    public class ReservedController : BaseController
    {
        ReservedService service = new ReservedService();
        private static readonly int PAGE_SIZE = SysConstants.DAFAULT_PAGESIZE;



        public ActionResult Index()
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponIndex.Get");
            try
            {
                var user = GetLoginUserUID();
                int pageSize = PAGE_SIZE;
                int pageIndex = SysConstants.DAFAULT_PAGEITEM;
                var model = new ReservedViewModel() { SearchParameter = new ReservedSearchMoel() };
                pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;
                var list = service.GetList(model.SearchParameter.Keyword, model.SearchParameter.Status, model.SearchParameter.mobile, model.SearchParameter.Min_Date, model.SearchParameter.Max_Date, pageIndex, pageSize);
                //var data = Mapper.Map<IPagedList<ReservedForm>>(list);
                var dataList = new List<ReservedForm>();
                foreach(var item in list)
                {
                    var data = new ReservedForm();
                    data.ASHA_CUSTOMER = Mapper.Map<CustomerForm>(item.CUSTOMER);
                    data.AHSA_PRODUCT = Mapper.Map<ProductForm>(item.PRODUCT);
                    data.ASHA_CATEGORY = Mapper.Map<CategoryForm>(item.PRODUCT.PRODUCT_CATEGORY);
                    data.Date = item.DATE;
                    data.Time = item.SERVE_TIME.TIME;
                    data.ID = item.ID;
                    dataList.Add(data);
                }
                IPagedList<ReservedForm> pageOrders = new StaticPagedList<ReservedForm>(dataList, pageIndex , pageSize, list.TotalItemCount);

                var result = new ReservedViewModel
                {
                    SearchParameter = model.SearchParameter,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    ReservedList = pageOrders,
                    Total=list.TotalItemCount

                    //totalCount
                };
                return View(result);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index", "Index");
        }

        //Post:Role
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ReservedViewModel model)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponIndex.Post");
            try
            {
                int pageSize = model.PageSize;
                int pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;
                pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;

                var list = service.GetList(model.SearchParameter.Keyword, model.SearchParameter.Status, model.SearchParameter.mobile, model.SearchParameter.Min_Date, model.SearchParameter.Max_Date, pageIndex, pageSize);
                //var data = Mapper.Map<IPagedList<ReservedForm>>(list);
                var dataList = new List<ReservedForm>();
                foreach (var item in list)
                {
                    var data = new ReservedForm();
                    data.ASHA_CUSTOMER = Mapper.Map<CustomerForm>(item.CUSTOMER);
                    data.AHSA_PRODUCT = Mapper.Map<ProductForm>(item.PRODUCT);
                    data.ASHA_CATEGORY = Mapper.Map<CategoryForm>(item.PRODUCT.PRODUCT_CATEGORY);
                    data.Date = item.DATE;
                    data.Time = item.SERVE_TIME.TIME;
                    data.ID = item.ID;
                    dataList.Add(data);
                }
                IPagedList<ReservedForm> pageOrders = new StaticPagedList<ReservedForm>(dataList, pageIndex, pageSize, list.TotalItemCount);

                var result = new ReservedViewModel
                {
                    SearchParameter = model.SearchParameter,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    ReservedList = pageOrders,
                    Total = list.TotalItemCount

                };
                return View(result);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index", "Index");
        }


        //EDIT:Category
        public ActionResult Edit(int id)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponEdit.Get");
            var form = new TimeForm();
            try
            {
                if (0 != id)
                {
                    var model = service.Get(id);
                    form = Mapper.Map<TimeForm>(model);
                    return View(form);
                }
                return View(form);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return View(form);
        }

        ////POST:Edit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(TimeForm form)
        //{
        //    LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponEdit.Post");
        //    try
        //    {
        //        var model = new ASHA_SERVE_TIME();

        //        if (form.ID != 0)
        //        {
        //            model = service.Get(form.ID);
        //            if (null == model)
        //            {
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return View(form);
        //        }
        //        model = Mapper.Map(form, model);
        //        service.Save(model);
        //    }
        //    catch (Exception e)
        //    {
        //        LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
        //    }
        //    return RedirectToAction("Index", "Category");
        //}

        //POST:Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string[] ids)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponDelete");
            try
            {
                service.Delete(ids);
                LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "Delete Coupon : " + ",Items:" + ids);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index");
        }

    }
}