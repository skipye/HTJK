using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelProject;
using ServiceProject;
using System.Web.Script.Serialization;
using WxPayAPI;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using LitJson;

namespace HTJKWeb.Controllers
{
    public class ProductsController : CartController
    {
        private static readonly NewsService NSer = new NewsService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PageList(SNewsModel SModel, int? Type, int? PageSize)
        {
            SModel.PageSize = PageSize ?? 9;
            var models = NSer.GetWebPageList(SModel, Type ?? 2);
            return View(models);
        }
        public ActionResult Detail(int Id,string t)
        {
            //var t=Request["t"];
            if (string.IsNullOrEmpty(t))
            {
                Session["t"] = t;
            }
            var Models = NSer.GetDetailById(Id);
            var MemberNumber = "";
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberNumber = UserModel.Split('|')[2].ToString();
            }
            Models.MemberNumber = MemberNumber;
            var existingCart = this.Carts;
            if (existingCart != null && existingCart.Count > 0)
            {
                int CartCount = 0;
                foreach (var item in existingCart)
                {
                    CartCount += item.Amount;
                }
                Models.CartCount = CartCount;
            }
            return View(Models);
        }
        public ActionResult WXProductsDetail(int Id)
        {
            var MemberNumber = "";
            if (Session["User"] != null)
            {
                string UserModel = Session["User"].ToString();
                MemberNumber = UserModel.Split('|')[2].ToString();
            }
            var RURL = "http://m.shengyuan-edu.com/Products/Detail/" + Id + "?t=" + MemberNumber;
            var Models = NSer.GetDetailById(Id);
            Models.appId = WxPayConfig.APPID;
            Models.timestamp = WxPayApi.GenerateTimeStamp();
            Models.nonceStr = WxPayApi.GenerateNonceStr();
            Models.finalsign = Getsignature(Models.nonceStr, Models.timestamp, RURL);
            Models.ticket = Session["ticket"].ToString();
            Models.RUrl = RURL;
            return new ContentResult
            {
                Content = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(Models),
                ContentType = "application/json"
            };
        }
       
       /// <summary>
        /// 获取access_token
        /// </summary>
        /// <returns></returns>
       public string Getaccesstoken()
       {
           string urljson = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + WxPayConfig.APPID + "&secret=" + WxPayConfig.APPSECRET;
           string strjson = "";
           try
           {
               //请求url以获取数据
               string result = HttpService.Get(urljson);
               JsonData jd = JsonMapper.ToObject(result);
               strjson = (string)jd["access_token"];
               Session["access_token"] = strjson;
           }
           catch (Exception ex)
           {
               throw new Exception(ex.Message);
           }
           return strjson;
       }
        /// <summary>
        /// 获得jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public string Getjsapi_ticket()
        {
            string accesstoken = (string)Session["access_token"];
            string urljson = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + accesstoken + "&type=jsapi";
            string strjson = "";
            try
            {
                //请求url以获取数据
                string result = HttpService.Get(urljson);
                JsonData jd = JsonMapper.ToObject(result);
                strjson = (string)jd["ticket"];
                Session["ticket"] = strjson;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
 
            return strjson;
        }
        /// <summary>
        /// 生成signature
        /// </summary>
        /// <param name="nonceStr"></param>
        /// <param name="timespanstr"></param>
        /// <returns></returns>
        public string Getsignature(string nonceStr, string timespanstr,string RUrl)
        {
            if (Session["access_token"] == null)
            {
                Getaccesstoken();
            }
            if (Session["ticket"] == null)
            {
                Getjsapi_ticket();
            }

            string str = "jsapi_ticket=" + (string)Session["ticket"] + "&noncestr=" + nonceStr +
                "&timestamp=" + timespanstr + "&url=" + RUrl;// +"&wxref=mp.weixin.qq.com";
            string singature = getSha1(str).ToLower();
            string ss = singature;
            return ss;
        }
        public static String getSha1(String str)
         {
           //建立SHA1对象
           SHA1 sha = new SHA1CryptoServiceProvider();
           //将mystr转换成byte[] 
           ASCIIEncoding enc = new ASCIIEncoding();
           byte[] dataToHash = enc.GetBytes(str);
           //Hash运算
           byte[] dataHashed = sha.ComputeHash(dataToHash);
           sha.Dispose();
           //将运算结果转换成string
           string hash = BitConverter.ToString(dataHashed).Replace("-", "");
           return hash;
         }
        
    }
}
