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
    public class ProductController : BaseController
    {
        ProductService service = new ProductService();
        private static readonly int PAGE_SIZE = SysConstants.DAFAULT_PAGESIZE;


        // GET: Category
        public ActionResult Index()
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponIndex.Get");
            try
            {
                int pageSize = PAGE_SIZE;
                int pageIndex = SysConstants.DAFAULT_PAGEITEM;
                var model = new ProductViewModel() { SearchParameter = new ProductSearchModel() };
                pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;
                List<ProductForm> formList = new List<ProductForm>();
                var list = service.GetList(model.SearchParameter.Keyword, model.SearchParameter.Status, model.SearchParameter.Category, pageIndex, pageSize);
                foreach (var item in list)
                {
                    var form = new ProductForm();
                    form = Mapper.Map<ProductForm>(item);
                    form.Category_Name = service.GetCategory(item.CATEGORY_ID).CATEGORY;
                    formList.Add(form);
                }
                var result = new ProductViewModel
                {
                    SearchParameter = model.SearchParameter,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    List = formList.ToPagedList(pageIndex, pageSize),
                    Category_List = service.GetCategoryList(SysConstants.ENABLE_STATUS)
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
        public ActionResult Index(ProductViewModel model)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "MailBoxIndex.Post");
            try
            {
                int pageSize = model.PageSize;
                int pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;
                pageIndex = model.PageIndex < 1 ? 1 : model.PageIndex;
                List<ProductForm> formList = new List<ProductForm>();
                var list = service.GetList(model.SearchParameter.Keyword, model.SearchParameter.Status, model.SearchParameter.Category, pageIndex, pageSize);
                foreach (var item in list)
                {
                    var form = new ProductForm();
                    form = Mapper.Map<ProductForm>(item);
                    form.Category_Name = service.GetCategory(item.CATEGORY_ID).CATEGORY;
                    formList.Add(form);
                }
                var result = new ProductViewModel
                {
                    SearchParameter = model.SearchParameter,
                    PageSize = pageSize,
                    PageIndex = pageIndex,
                    List = formList.ToPagedList(pageIndex, pageSize),
                    Category_List = service.GetCategoryList(SysConstants.ENABLE_STATUS)
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
        public ActionResult Edit(string id)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponEdit.Get");
            var form = new ProductForm();
            form.Category_List = service.GetCategoryList(SysConstants.ENABLE_STATUS);
            try
            {
                if (null != id)
                {
                    var model = service.Get(id);
                    form = Mapper.Map<ProductForm>(model);
                    form.Category_List = service.GetCategoryList(SysConstants.ENABLE_STATUS);
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
        public ActionResult Edit(ProductForm form)
        {
            LogUtil.Info(System.Reflection.MethodBase.GetCurrentMethod(), "CouponEdit.Post");
            try
            {
                var model = new PRODUCT();
                form.Creator = GetLoginUserUID();
                form.Modifier = GetLoginUserUID();
                if (!string.IsNullOrEmpty(form.Id))
                {
                    model = service.Get(form.Id);
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
            return RedirectToAction("Index");
        }

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