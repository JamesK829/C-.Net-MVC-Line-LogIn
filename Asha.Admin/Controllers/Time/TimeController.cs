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
    public class TimeController : BaseController
    {
        TimeService service = new TimeService();


        // GET: Category
        public ActionResult Index()
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponIndex.Get");
            try
            {
                List<TimeForm> formList = new List<TimeForm>();
                var list = service.GetList();
                foreach(var item in list)
                {
                    var form = new TimeForm();
                    form = Mapper.Map<TimeForm>(item);
                    formList.Add(form);
                }
                var result = new TimeViewModel
                {
                    List = formList
                };
                return View(result);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index", "Index");
        }

        //Post:Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(TimeViewModel model)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "MailBoxIndex.Post");
            try
            {
                List<TimeForm> formList = new List<TimeForm>();
                var list = service.GetList();
                foreach (var item in list)
                {
                    var form = new TimeForm();
                    form = Mapper.Map<TimeForm>(item);
                    formList.Add(form);
                }
                var result = new TimeViewModel
                {
                    List = formList,
                };
                return View(result);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), "MailBoxIndex.Post", e);
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

        //POST:Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TimeForm form)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponEdit.Post");
            try
            {
                var model = new SERVE_TIME();

                if (form.ID!=0)
                {
                    model = service.Get(form.ID);
                    if (null == model)
                    {
                        return RedirectToAction("Index");
                    }
                }
                if (!ModelState.IsValid)
                {
                    return View(form);
                }
                model = Mapper.Map(form, model);
                service.Save(model);
            }
            catch (Exception e)
            {
                LogUtil.Error(System.Reflection.MethodBase.GetCurrentMethod(), e);
            }
            return RedirectToAction("Index", "Category");
        }

        //POST:Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int[] ids)
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