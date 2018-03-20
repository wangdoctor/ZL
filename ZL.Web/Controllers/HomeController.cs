using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZL.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Data.SqlClient;

namespace ZL.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ///路由好友openid
            var r_fopenid = RouteData.Values["openid"];
            ///授权获取的openid
            string openid = Request.QueryString["openId"] + "";
            string nickname = Request.QueryString["nickName"] + "";
            string headimgurl = Request.QueryString["headImgUrl"] + "";
            if (!string.IsNullOrEmpty(r_fopenid+"")&& !string.IsNullOrEmpty(Session["openid"] + ""))
            {
                return View();
            }
            ////读取分享者Openid
            if (!string.IsNullOrEmpty(openid) || !string.IsNullOrEmpty(Session["openid"] + ""))
            {
                Session["openid"] = openid;
                Log l = new Log();
                l.Info(openid+nickname+headimgurl);
                AddUser(openid, nickname,headimgurl);

                Response.Redirect(WebUtility.UrlDecode("http://" + Request.Url.Authority)+"/home/index/" + Session["openid"]);
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx4d20a3efbcce8669&redirect_uri=http://wx.jumax-sh.dev.sudaotech.com/api/wechat/wechatToken/oauth2?url=" + WebUtility.UrlDecode("http://" + Request.Url.Authority + Request.Url.AbsolutePath) + "&response_type=code&scope=snsapi_userinfo#wechat_redirect");

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginUserInfo userinfo)
        {
            string msg = string.Empty;
            ZLHttpRequet zlHttp = new ZLHttpRequet();
            ResponseInfo res = new ResponseInfo();
            Log logger = new Log();
            msg = zlHttp.Post("http://www.jumax-sh.dev.sudaotech.com/api/mall/auth/login", JsonConvert.SerializeObject(userinfo));
            if (msg.IndexOf("error") > -1)
            {
                res = JsonConvert.DeserializeObject<ResponseInfo>(msg);
                return Json(msg);
            }
            else
            {
                var dd = JsonConvert.DeserializeObject<UserInfo>(msg);
                return Json(msg);
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Regist()
        {
            return Json("");
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendSmsCode(SmsCode smscode)
        {
            string msg = string.Empty;
            ZLHttpRequet zlHttp = new ZLHttpRequet();
            ResponseInfo res = new ResponseInfo();
            Log logger = new Log();
            try
            {
                msg = zlHttp.Post("http://www.jumax-sh.dev.sudaotech.com/api/mall/sms/vcode", JsonConvert.SerializeObject(smscode));

            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return Json(msg + "***http://www.jumax-sh.dev.sudaotech.com/api/mall/sms/vcode***" + JsonConvert.SerializeObject(smscode));
            }

            return Json(msg);
        }
        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckSmsCode()
        {
            return Json("");
        }

        public void AddUser(string openid,string nickname,string headimgurl)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                new SqlParameter("@nickname",nickname),
                new SqlParameter("@qans",headimgurl),
                };
            string sql = "exec adduser @openid,@nickname,@qans";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                  db.GetSingle(sql,sqlParams);
            }
            catch (Exception)
            {

             
            }
          
        }
    }
    /// <summary>
    /// Jumax登录信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginUserInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
    }
    /// <summary>
    /// 注册信息
    /// </summary>
    public class RegistUserInfo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string smscode { get; set; }
        /// <summary>
        /// 注册来源
        /// </summary>
        public string registSource { get; set; }


    }

    /// <summary>
    /// 获取短信验证码
    /// </summary>
    public class SmsCode
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 随机数
        /// </summary>
        public string rand { get; set; }
        /// <summary>
        /// 图像验证码
        /// </summary>
        public string captcha { get; set; }
        /// <summary>
        /// 类型 1.注册 2.登录
        /// </summary>
        public int validateType { get; set; }

    }
    /// <summary>
    /// 校验短信验证码
    /// </summary>
    public class CheckSmsCode
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string smscode { get; set; }

    }
    /// <summary>
    /// 接口返回信息
    /// </summary>
    class ResponseInfo
    {
        public string code { get; set; }
        public List<Err> errors { get; set; }
        public string message { get; set; }
        public string result { get; set; }

    }
    class Err
    {
        public string message { get; set; }
        public string reason { get; set; }
    }
    class WechatInfo
    {
        /// <summary>
        /// Openid
        /// </summary>
        private string Openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        private string NickName { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        private string HeadUrl { get; set; }
    }
}