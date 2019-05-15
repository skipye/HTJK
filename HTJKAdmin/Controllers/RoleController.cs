using ModelProject;
using ServiceProject;
using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HTJKAdmin.Controllers
{
    public class RoleController : Controller
    {
        private static readonly RoleService CSer = new RoleService();
        private static readonly MenuService MSer = new MenuService();
        private static readonly UserService User = new UserService();
        public ActionResult Index()
        {
            SRoleModel SModels = new SRoleModel();
            return View(SModels);
        }
        public ActionResult PageList(SRoleModel SModels)
        {
            var PageList = CSer.GetPageList(SModels);
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(PageList),
                ContentType = "application/json"
            };
        }
        public ActionResult Add(int? Id)
        {
            RoleModel Models = new RoleModel();
            if (Id != null && Id > 0)
            {
                Models = CSer.GetDetailById(Id.Value);
            }
            Models.MenuItemList = MSer.GetMenuItemList("");
            Models.UserDroList = User.GetUserDrolist(Models.UserId);
            return View(Models);
        }
        [ValidateInput(false)]
        public ActionResult PostAdd(RoleModel Models)
        {
            if (CSer.AddOrUpdate(Models) == true)
            {
                return Content("1");
            }
            else { Models.UserDroList = User.GetUserDrolist(Models.UserId); Models.MenuItemList = MSer.GetMenuItemList(Models.MenuList); return View(Models); }
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
                if (CSer.DeleteMore(ListId) == true)
                {
                    return Content("True");
                }
                else return Content("False");
            }
        }

    }
}
