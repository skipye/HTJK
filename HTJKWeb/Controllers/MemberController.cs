using System;
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
            if (USer.AddUser(Models, out UserId) == true)
            {
                string UserAuthority = Models.Name + "|" + Models.Id;
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

        public class WXFilterAttribute : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                string openId = "";
                string userAgent = httpContext.Request.UserAgent;
                if (userAgent.IndexOf("MicroMessenger") <= -1)//不是微信浏览器
                {
                    return true;
                }

                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    string appid = string.Empty;
                    string secret = string.Empty;


                    appid = WxPayConfig.APPID;
                    secret = WxPayConfig.APPSECRET;

                    var code = httpContext.Request["Code"];
                    string returnUrl = HttpUtility.UrlDecode(httpContext.Request["ReturnUrl"] ?? "/");


                    if (string.IsNullOrEmpty(code))
                    {
                        string host = httpContext.Request.Url.Host;
                        string path = httpContext.Request.Path;
                        string redirectUrl = "http://" + host + path + "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);//重定向的url，这里不需要进行编码，在后面会自己编码
                        //string redirectUrl = "http://m.shengyuan-edu.com" + path + "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);//重定向的url，这里不需要进行编码，在后面会自己编码
                        try
                        {
                            //todo:通过微信获取2.0授权的url
                            string url = GetAuthorizeUrl(appid, redirectUrl, "state", "snsapi_base");

                            httpContext.Response.Redirect(url);
                        }
                        catch (System.Exception ex)
                        {
                            httpContext.Response.Write("构造网页授权获取code的URL时出错，错误是：" + ex.Message);
                            httpContext.Response.End();
                        }
                    }
                    else
                    {
                        var client = new System.Net.WebClient();
                        client.Encoding = System.Text.Encoding.UTF8;
                        string url = GetAccessTokenUrl(appid, secret, code);
                        var data = client.DownloadString(url);
                        var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                        string accessToken;
                        if (!obj.TryGetValue("access_token", out accessToken))
                        {
                            httpContext.Response.Write("构造网页授权获取access_token的URL时出错");
                            httpContext.Response.End();
                        }
                        if (obj["openid"] != null)
                        {
                            httpContext.Session["openId"] = obj["openid"];
                            var Models = USer.IsWXLogin(openId);
                            if (Models != null && Models.IsLogin == true)
                            {
                                string UserAuthority = Models.UserName + "|" + Models.UserId + "|" + Models.MemberNumber;
                                httpContext.Session["User"] = UserAuthority;
                                httpContext.Response.Redirect(returnUrl);
                            }
                            
                        }

                    }
                }
                return true;
            }
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                base.OnAuthorization(filterContext);
                if (filterContext.HttpContext.Response.StatusCode == 401)
                {
                    filterContext.Result = new RedirectResult("/403.htm");//跳转异常页面
                }
            }

            //扩展
            public string GetAuthorizeUrl(string appId, string redirectUrl, string state, string scope, string responseType = "code")
            {
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    redirectUrl = HttpUtility.UrlEncode(redirectUrl, System.Text.Encoding.UTF8);
                }
                else
                {
                    redirectUrl = null;
                }
                object[] args = new object[] { appId, redirectUrl, responseType, scope, state };
                return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect", args);
            }
            public string GetAccessTokenUrl(string appId, string secret, string code, string grantType = "authorization_code")
            {
                object[] args = new object[] { appId, secret, code, grantType };
                string requestUri = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type={3}", args);
                //return GetAccessTokenInfo(_httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result);
                return requestUri;
            }
        }
    }
}
