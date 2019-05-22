using ModelProject;
using ServiceProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HTJKWeb.Controllers
{
    public class MessageController : Controller
    {
        private static readonly MessageService MSer = new MessageService();
        public ActionResult Index()
        {
            UserIdOrNameModel UserModels = new UserIdOrNameModel();
            UserModels.ReturnUrl = "/Member/Notices";
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
                UserModels.Name = UserModel.Split('|')[0];
            }
            UserModels.TotalCount = MSer.GetMessagelistConut(MemberId);
            
            return View(UserModels);
        }
        public ActionResult AddMessage(string StrContent)
        {
            MessageModel models = new MessageModel();
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
                models.UserId = MemberId;
                models.UserName = UserModel.Split('|')[0];
            }
            models.StrContent = StrContent;
            if (MSer.AddMessage(models) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
        public ActionResult MessageList(int PageSize, int PageIndex,int isAdmin)
        {
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
            }
            var models = MSer.GetMessagelist(PageIndex, PageSize, MemberId);
            if (isAdmin>0)
            { models= MSer.GetMessagelist(PageIndex, PageSize,null); }
           
            return View(models);
        }
        public ActionResult ReplyNotices(string StrContent,Guid MemMsgId,Guid ReplyMemberId)
        {

            MessageModel models = new MessageModel();
            var MemberId = Guid.Empty;
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberId = new Guid(UserModel.Split('|')[1]);
                models.UserId = MemberId;
                models.UserName = UserModel.Split('|')[0];
            }
            models.StrContent = StrContent;
            models.TitleId = MemMsgId;
            models.ReplyUserId = ReplyMemberId;
            if (MSer.AddReplyMessage(models) == true)
            {
                return Content("1");
            }
            else { return Content("0"); }
        }
    }
}
