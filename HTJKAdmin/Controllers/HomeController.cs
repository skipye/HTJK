using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTJKAdmin.Controllers
{
    public class HomeController : UserBaseController
    {
        private static readonly UserService USer = new UserService();
        private static readonly RoleService RSer = new RoleService();
        private static readonly MenuService MSer = new MenuService();
       
        public ActionResult Index()
        {
            var Models = GetUser();
            if (Models.Id <= 0)
            {
                return RedirectToAction("Login","Account");
            }
            return View();
        }
        //欢迎页面
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult _Header()
        {
            var Models = GetUser();
            return View(Models);
        }
        public ActionResult _Menu()
        {
            var Models = GetUser();
            var StrMenuList = RSer.GetUserMenuByUserId(Models.Id);
            var MenuItemList = new List<ModelProject.MenuItemModel>();
            if (!string.IsNullOrEmpty(StrMenuList))
            { MenuItemList = MSer.GetMenuItemList(StrMenuList); }
            //if (Models.UserId == 1)
            //{
            //    MenuItemList = MSer.GetMenuItemList("");
            //}
            return View(MenuItemList);
        }
        public ActionResult _Footer()
        { 
            return View(); 
        }
        public ActionResult _Meta()
        {
            return View();
        }
    }
}
