﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;
using ServiceProject;
using WxPayAPI;
using Newtonsoft.Json;

namespace HTJKWeb.Controllers
{
    public class MemberController : Controller
    {
        private static readonly MemberService USer = new MemberService();
        private static readonly AddressService ASer = new AddressService();
        public ActionResult Index()
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            var CrrUser = USer.GetUserDetail(MemberId);
            return View(CrrUser);
        }
        public ActionResult PersonalInfo()
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            var models = USer.GetUserDetail(MemberId);
            return View(models);
        }
        public ActionResult MemberSubmit(MemberModel Models)
        {
            Guid UserId = Models.Id;
            string MemNum = "";
            if (USer.AddUser(Models, out UserId,out MemNum) == true)
            {
                string UserAuthority = Models.Name + "|" + Models.Id+ "|" + MemNum;
                System.Web.Security.FormsAuthentication.SetAuthCookie(UserAuthority, false);
                return RedirectToAction("Index", "Member");
            }
            else { return RedirectToAction("PersonalInfo", "Member"); }

        }
        public ActionResult MyOrder()
        {
            return View();
        }
        public ActionResult PayOrder()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult HChangPassword(string oldPassword, string newPassword)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return Content("请输入你的新密码！");
            }
            if (USer.ChangPassword(newPassword, MemberId) == true)
            { return Content("1"); }
            else { return Content("0"); }

        }
        public ActionResult Address(string ReturnUrl)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            var models = ASer.GetAddressList(MemberId);
            ViewBag.ReturnUrl = ReturnUrl;
            return View(models);
        }
        public ActionResult AddAddress(int? Id, string ReturnUrl)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            AddressModel models = new AddressModel();
            if (Id > 0)
            {
                models = ASer.GetAddressDetailById(Id.Value);
            }
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                if (Session["ReturnUrl"] != null)
                {
                    ReturnUrl = Session["ReturnUrl"].ToString();
                }
            }
            models.ReturnUrl = ReturnUrl;
            return View(models);
        }
        public ActionResult IsTopAddress(int Id)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ASer.SetIsTop(Id, MemberId) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
        [HttpPost]
        public ActionResult saveAddress(AddressModel models)
        {
            int AId = 0;
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            if (MemberId == Guid.Empty)
            {
                return RedirectToAction("Login", "Account");
            }
            models.MemberId = MemberId;
            if (ASer.AddOrUpdateAddress(models, out AId) == true)
            {
                return Content("1&" + AId);
            }
            else { return Content("0"); }
        }
        public ActionResult _TopHead(string CrrNavName)
        {
            ViewData["CrrNavName"] = CrrNavName;
            return View();
        }
       
        
        public ActionResult PointList(int PageSize, int PageIndex)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            else { return RedirectToAction("Login", "Account", new { ReturnUrl = "/Member/Point" }); }
            var models = USer.GetMemberPointList(PageIndex, PageSize, MemberId);
            return View(models);
        }
        //会员须知
        public ActionResult UserNots()
        {
            return View();
        }
        //售后服务
        public ActionResult BuyService()
        {
            return View();
        }
        //我的佣金
        public ActionResult Point()
        {
            return View();
        }
        public ActionResult Notices()
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = "/Member/Notices" });
            }
            int MessageCount = USer.GetReMessageFalseConut(MemberId)??0;
            if (MessageCount > 0) { USer.UpdateMemberMessageState(MemberId); }
            return View();
        }
        
    }
}
