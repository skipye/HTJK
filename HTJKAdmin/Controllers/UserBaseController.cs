using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;

namespace HTJKAdmin.Controllers
{
    public class UserBaseController : Controller
    {
        public UsersModel GetUser()
        {
            UsersModel Modes = new UsersModel();
            if (Session["User"] !=null)
            {
                string UserModel = Session["User"].ToString();
                int UserId = Convert.ToInt32(UserModel.Split('|')[1]);
                Modes.Id = UserId;
                Modes.Name = UserModel.Split('|')[0];
            }
            return Modes;
        }
    }
}
