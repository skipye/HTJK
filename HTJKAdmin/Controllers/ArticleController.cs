using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;
using ServiceProject;
using System.Web.Script.Serialization;

namespace HTJKAdmin.Controllers
{
    public class ArticleController : UserBaseController
    {
        private static readonly UserService USer = new UserService();
        private static readonly NewsService NSer = new NewsService();

        public ActionResult Index()
        {
            SNewsModel SModels = new SNewsModel();
            SModels.TypeDroList = NSer.GetNewTypeDrolist(SModels.TypeId);
            return View(SModels);
        }
        public ActionResult PageList(SNewsModel SRmodels)
        {
            var PageList = NSer.GetPageList(SRmodels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
        public ActionResult Add(int? Id)
        {
            NewsModel Models = new NewsModel();
            if (Id != null && Id > 0)
            {
                Models = NSer.GetDetailById(Id.Value);
                Models.EidtAuthorId = GetUser().Id;
            }
            Models.TypeDroList = NSer.GetNewTypeDrolist(Models.TypeId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(NewsModel Models)
        {
            Models.UploadAuthorId = GetUser().Id;
            Models.EidtAuthorId = GetUser().Id;
            Models.UploadName = GetUser().Name;
            Models.EidtAuthorName = GetUser().Name;
            if (NSer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { return View(Models); }
        }
        //用户自己的内容
        public ActionResult UserIndex()
        {
            SNewsModel SModels = new SNewsModel();
            SModels.UploadAuthorId = GetUser().Id;
            SModels.TypeDroList = NSer.GetNewTypeDrolist(SModels.TypeId);
            return View(SModels);
        }
        //删除多个
        public ActionResult Delete(string ListId)
        {
            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (NSer.DeleteMore(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
        public ActionResult Checked(string ListId, int CheckedId)
        {

            if (string.IsNullOrEmpty(ListId) == true)
            {
                return Content("False");
            }
            else
            {
                if (NSer.Checked(ListId, CheckedId, GetUser().Id) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }
        //获取所有的图片列表
        public ActionResult Basket(int Id)
        {
            var list = NSer.GetFileInfoList(Id);
            return View(list);
        }

    }
}
