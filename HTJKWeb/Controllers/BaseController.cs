﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;

namespace HTJKWeb.Controllers
{
    public class BaseController : Controller
    {
        protected List<CartModel> Carts
        {
            get
            {
                if (Session["Carts"] == null)
                {
                    Session["Carts"] = new List<CartModel>();
                }
                return (Session["Carts"] as List<CartModel>);
            }
            set { Session["Carts"] = value; }
        }

    }
}
