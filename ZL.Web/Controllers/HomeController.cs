using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZL.Infrastructure;
using Newtonsoft.Json;
using System.Net;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ZL.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Log l = new Log();
            //l.Info(Request.Url.ToString());
            //路由好友openid
            var r_fopenid = RouteData.Values["fopenid"];
            //路由自己openid
            var r_openid = RouteData.Values["openid"];
            ///授权获取的openid
            string openid = Request.QueryString["openId"] + "";
            string nickname = Request.QueryString["nickName"] + "";
            string headimgurl = Request.QueryString["headImgUrl"] + "";

            if (!string.IsNullOrEmpty(r_openid + "") && !string.IsNullOrEmpty(r_fopenid + "") && !string.IsNullOrEmpty(Session["openid"] + ""))
            {
                ViewBag.cs = CJ(Session["openid"] + "");
                ViewBag.openid = Session["openid"];
                ViewBag.fopenid = r_fopenid;
                return View();
            }
            ////读取分享者Openid
            if (!string.IsNullOrEmpty(openid) || !string.IsNullOrEmpty(Session["openid"] + ""))
            {
                if (string.IsNullOrEmpty(Session["openid"] + ""))
                {
                    Session["openid"] = openid;
                }
                else if (!string.IsNullOrEmpty(openid))
                {
                    Session["openid"] = openid;
                }
                AddUser(openid, nickname, headimgurl);
                if (string.IsNullOrEmpty(r_fopenid + ""))
                {
                    r_fopenid = Session["openid"];
                }
                Response.Redirect(WebUtility.UrlDecode("http://" + Request.Url.Authority) + "/home/index/" + Session["openid"] + "/" + r_fopenid);
            }
            else
            {
                Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx4d20a3efbcce8669&redirect_uri=http://wx.jumax-sh.dev.sudaotech.com/api/wechat/wechatToken/oauth2?url=" + WebUtility.UrlDecode("http://" + Request.Url.Authority + Request.Url.AbsolutePath) + "&response_type=code&scope=snsapi_userinfo#wechat_redirect");

            }
            return View();
        }

        public ActionResult About()
        {

            Bssub(new Bs() { userId = "78869", activityId = "1", lastRate = "1.6" });
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
        public ActionResult Login(LoginUserInfo userinfo, string openid)
        {
            string msg = string.Empty;
            ZLHttpRequet zlHttp = new ZLHttpRequet();
            ResponseInfo res = new ResponseInfo();
            Log logger = new Log();
            logger.Info("登录提交：" + JsonConvert.SerializeObject(userinfo) + "&&" + openid);
            msg = zlHttp.Post("http://www.jumax-sh.dev.sudaotech.com/api/mall/auth/login", JsonConvert.SerializeObject(userinfo));
            if (msg.IndexOf("error") > -1)
            {
                logger.Info("登录返回：" + openid + msg);
                res = JsonConvert.DeserializeObject<ResponseInfo>(msg);
                return Json(res.message);
            }
            else
            {
                logger.Info("登录返回：" + openid + msg);
                var dd = JsonConvert.DeserializeObject<UserInfo>(msg);
                Bind(openid, dd.userId);
                return Json("true");
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Regist(RegistUserInfo reg, string fopenid, string openid)
        {
            string msg = string.Empty;
            ZLHttpRequet zlHttp = new ZLHttpRequet();
            ResponseInfo res = new ResponseInfo();
            Log logger = new Log();
            logger.Info("注册提交：" + JsonConvert.SerializeObject(reg) + "&&openid=" + openid + "&&fopenid" + fopenid);
            msg = zlHttp.Post(ConfigurationManager.AppSettings["baseurl"] + "/auth/register", JsonConvert.SerializeObject(reg));
            if (msg.IndexOf("error") > -1)
            {
                logger.Info("注册返回：" + openid + msg);
                res = JsonConvert.DeserializeObject<ResponseInfo>(msg);
                //var uid = GerBs(fopenid).Split(',');
                

                //if (uid.Length > 1)
                //{
                //    Bssub(new Bs()
                //    {
                //        activityId = "1",
                //        lastRate = decimal.Round(decimal.Parse(uid[0]), 1)+"",
                //        userId = uid[1]
                //    });
                //}
                return Json(res.message);

            }
            else
            {
                logger.Info("注册返回：" + openid + msg);
                var dd = JsonConvert.DeserializeObject<UserInfo>(msg);
                Bind(openid, dd.storeUserId);
                if (openid!=fopenid)
                {
                    BindUser(openid, fopenid);
                }
                var uid = GerBs(fopenid).Split(',');
                if (uid.Length > 1)
                {
                    Bssub(new Bs()
                    {
                        activityId = "1",
                        lastRate = decimal.Round(decimal.Parse(uid[0]), 1) + "",
                        userId = uid[1]
                    });
                }
                return Json("true");
            }
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
            logger.Info("短信验证码提交：" + JsonConvert.SerializeObject(smscode));
            msg = zlHttp.Post(ConfigurationManager.AppSettings["baseurl"] + "/sms/vcode", JsonConvert.SerializeObject(smscode));
            if (msg.IndexOf("error") > -1)
            {
                logger.Info(msg);
                res = JsonConvert.DeserializeObject<ResponseInfo>(msg);
                return Json(res.message);
            }
            else
            {
                logger.Info(msg);
                var dd = JsonConvert.DeserializeObject<UserInfo>(msg);
                return Json("true");
            }
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
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="nickname"></param>
        /// <param name="headimgurl"></param>
        public void AddUser(string openid, string nickname, string headimgurl)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                new SqlParameter("@nickname",nickname),
                new SqlParameter("@qans",headimgurl),
                };
            string sql = @"exec adduser @openid,@nickname,@qans";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                db.GetSingle(sql, sqlParams);
            }
            catch (Exception)
            {


            }

        }
        public void BindUser(string openid, string fopenid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                new SqlParameter("@fopenid",fopenid),
                };
            string sql = @"exec binduser @openid,@fopenid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                db.GetSingle(sql, sqlParams);
            }
            catch (Exception)
            {


            }

        }


        public ActionResult GetUserinfoF(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = @"select top 1 * from J_UserInfo where Openid =@openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                DataSet dt = db.Query(sql, sqlParams);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    return Json(new WechatInfo()
                    {
                        NickName = dt.Tables[0].Rows[0]["Nickname"] + "",
                        HeadUrl = dt.Tables[0].Rows[0]["Headimgurl"] + "",
                        BaseMultiple = dt.Tables[0].Rows[0]["BaseMultiple"] + "",

                    });
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception)
            {
                return Json(null);

            }

        }

        public ActionResult GetOwnUserid(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = " select JUserid from J_UserInfo  where Openid= @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                string bs = db.GetSingle(sql, sqlParams) + "";
                return Json(bs);
            }
            catch (Exception)
            {
                return Json("0");

            }
        }
        /// <summary>
        /// 获取抽奖次数
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public string CJ(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = " select blanace from [J_UserInfo] where openid= @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                return db.GetSingle(sql, sqlParams).ToString();
            }
            catch (Exception)
            {
                return "0";

            }
        }
        /// <summary>
        /// 提交中奖倍数
        /// </summary>
        /// <param name="bs"></param>
        public void Bssub(Bs bs)
        {
            string msg = string.Empty;
            ZLHttpRequet zlHttp = new ZLHttpRequet();
            ResponseInfo res = new ResponseInfo();
            Log logger = new Log();
            logger.Info("提交中奖倍数:" + JsonConvert.SerializeObject(bs));
            msg = zlHttp.Post(ConfigurationManager.AppSettings["baseurl"] + "/rcActivity/rate", JsonConvert.SerializeObject(bs));
            if (msg.IndexOf("error") > -1)
            {
                res = JsonConvert.DeserializeObject<ResponseInfo>(msg);
                logger.Info("返回中奖倍数:" + msg);
            }
            else
            {
                var dd = JsonConvert.DeserializeObject<Bsr>(msg);
                logger.Info("返回中奖倍数:" + msg);
            }
        }

        public void Bind(string openid, string userid)
        {

            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                new SqlParameter("@userid", userid),
                };
            string sql = "  update [DB_Jumax201803].[dbo].[J_UserInfo] set JUserid =@userid where Openid = @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                db.GetSingle(sql, sqlParams);
            }
            catch (Exception)
            {


            }
        }

        public void UpInfo(string openid, string mul)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                new SqlParameter("@mul", mul),
                };
            string sql = "update[DB_Jumax201803].[dbo].[J_UserInfo] set BaseMultiple =@mul where Openid = @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                db.GetSingle(sql, sqlParams);
            }
            catch (Exception)
            {


            }
        }
        public ActionResult Max(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = " select MAX(Prize) prize from j_prize where openid= @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                string bs = db.GetSingle(sql, sqlParams).ToString();
                UpInfo(openid, bs);
                var uid = GerBs(openid).Split(',');
                if (uid.Length > 1)
                {
                    Bssub(new Bs()
                    {
                        activityId = "1",
                        lastRate = uid[0],
                        userId = uid[1]
                    });
                }
                return Json(bs);
            }
            catch (Exception)
            {
                return Json("0");

            }
        }
        /// <summary>
        /// 抽奖
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public ActionResult Lottery(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = "   exec [GetLottery] @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                return Json(db.GetSingle(sql, sqlParams).ToString());
            }
            catch (Exception)
            {
                return Json("-2,-2");

            }
        }
        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult Imgy()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            string uuid = ConfigurationManager.AppSettings["baseurl"] + "/captcha/image?rand=" + Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return Json(uuid);
        }

        public ActionResult Flist(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid),
                };
            string sql = @" select s.nickname,s.headimgurl from J_InvitationInfo i
                            inner join[J_UserInfo] s on i.Openid = s.Openid
                            where i.fOpenid = @openid  and s.nickname<>''";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                List<WechatInfo> list = new List<WechatInfo>();
                DataSet dt = db.Query(sql, sqlParams);
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    list.Add(new WechatInfo()
                    {
                        NickName = dt.Tables[0].Rows[i]["nickname"] + "",
                        HeadUrl = dt.Tables[0].Rows[i]["headimgurl"] + "",
                    });
                }
                return Json(list);
            }
            catch (Exception)
            {
                return Json(null);

            }

        }


        public string GerBs(string openid)
        {
            SqlParameter[] sqlParams = new SqlParameter[] {
                new SqlParameter("@openid", openid)
                };
            string sql = "exec [Getbs] @openid";
            DbHelperSQL db = new DbHelperSQL();
            try
            {
                DataSet dt = db.Query(sql, sqlParams);
                string cs = dt.Tables[0].Rows[0][0] + "";
                string count = dt.Tables[0].Rows[0][1] + "";
                string userid = dt.Tables[0].Rows[0][2] + "";
                if (!string.IsNullOrEmpty(cs) && !string.IsNullOrEmpty(userid))
                {
                    if (int.Parse(count) >= 21)
                    {
                        return (float.Parse(cs) + 0.7).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 18)
                    {
                        return (float.Parse(cs) + 0.6).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 15)
                    {
                        return (float.Parse(cs) + 0.5).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 12)
                    {
                        return (float.Parse(cs) + 0.4).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 9)
                    {
                        return (float.Parse(cs) + 0.3).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 6)
                    {
                        return (float.Parse(cs) + 0.2).ToString() + "," + userid;
                    }
                    else if (int.Parse(count) >= 3)
                    {
                        return (float.Parse(cs) + 0.1).ToString() + "," + userid;
                    }
                    else
                    {
                        return (float.Parse(cs)).ToString() + "," + userid;
                    }
                }
                else
                {
                    return "0";
                }

            }
            catch (Exception)
            {

                return "0";
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
        public string storeUserId { get; set; }
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
    public class ResponseInfo
    {
        public string code { get; set; }
        public List<Err> errors { get; set; }
        public string message { get; set; }
        public string result { get; set; }

    }
    /// <summary>
    /// 提交倍数
    /// </summary>
    public class Bs
    {
        public string activityId { get; set; }
        public string lastRate { get; set; }
        public string userId { get; set; }
    }
    /// <summary>
    /// 倍数返回值
    /// </summary>
    public class Bsr
    {
        public string activityId { get; set; }
        public string activityRateId { get; set; }
        public string createTime { get; set; }
        public string createUserId { get; set; }
        public string createUserName { get; set; }
        public string deleted { get; set; }
        public string displayOrder { get; set; }
        public string lastRate { get; set; }
        public string lastUpdate { get; set; }
        public string updateTime { get; set; }
        public string updateUserId { get; set; }
        public string updateUserName { get; set; }
        public string userId { get; set; }
        public string version { get; set; }
    }
    public class Err
    {
        public string message { get; set; }
        public string reason { get; set; }
    }
    public class WechatInfo
    {
        /// <summary>
        /// Openid
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像Url
        /// </summary>
        public string HeadUrl { get; set; }
        public string BaseMultiple { get; set; }
    }
}