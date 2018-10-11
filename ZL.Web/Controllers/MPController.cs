using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using ZL.Infrastructure;
using System.Security.Cryptography;
using System.Text;

namespace ZL.Web.Controllers
{
    public class MPController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 获取Openid
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetOpenid([FromBody]WechatLoginInfo loginfo)
        {
            string appid = ConfigurationManager.AppSettings["AppID"];
            string appsecret = ConfigurationManager.AppSettings["AppSecret"];
            WeChatAppDecrypt wcad = new WeChatAppDecrypt(appid, appsecret);
            var resultInfo = wcad.DecodeOpenIdAndSessionKey(new WechatLoginInfo() { code = loginfo.code });
            SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@openid",resultInfo.openid),
                new SqlParameter("@sessionkey",resultInfo.session_key)
                // new SqlParameter("@openid","2"),
                //new SqlParameter("@sessionkey","3333")
                };
            string sql = @" exec AddUser @openid,@sessionkey";
            DbHelperSQL db = new DbHelperSQL();
            db.ExecuteSql(sql, paras);
            return Ok(resultInfo.openid);
        }

        /// <summary>
        /// 保存微信用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveWechatUserInfo([FromBody]WechatUInfo user)
        {
            SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@openid",user.Openid),
                new SqlParameter("@gender",user.Gender),
                new SqlParameter("@avatarUrl",user.avatarUrl),
                new SqlParameter("@city",user.City),
                new SqlParameter("@country",user.Country),
                new SqlParameter("@language",user.Language),
                new SqlParameter("@province",user.Province),
                new SqlParameter("@nickname",user.nickName)
                };
            string sql = @"UPDATE [JM_WechatUserInfo]
                           SET [NickName] = @nickname
                              ,[AvatarUrl] =@avatarUrl
                              ,[Gender] = @gender
                              ,[Language] = @language
                              ,[Country] = @country
                              ,[Province] = @province
                              ,[City] = @city
                         WHERE openid=@openid";
            DbHelperSQL db = new DbHelperSQL();
            db.ExecuteSql(sql, paras);
            return Ok(true);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Login([FromBody]ShopLoginUserInfo loginfo)
        {
            Log l = new Log();
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s = zlhttp.Post(ConfigurationManager.AppSettings["api"] + "/auth/login", JsonConvert.SerializeObject(loginfo));
            l.Info("登录："+s);
            var info = JsonConvert.DeserializeObject<Root>(s);
            if (info != null && info.cellphone != null)
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@openid",loginfo.Openid),
                new SqlParameter("@cellphone",info.cellphone),
                new SqlParameter("@photo",info.photo),
                new SqlParameter("@userName",info.userName),
                new SqlParameter("@userId",info.userId),
                new SqlParameter("@formID",loginfo.FormID)
                };
                string sql = @"exec [AddShopUser] @openid,@cellphone,@photo,@userName,@userId,@formID,0";
                DbHelperSQL db = new DbHelperSQL();
                db.ExecuteSql(sql, paras);
                return Ok(true);
            }
            else if (info != null && info.message != null)
            {
                return Ok(info.message);
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="loginfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetSms([FromBody]ShopUserInfo loginfo)
        {
            Log l = new Log();
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s = zlhttp.Post(ConfigurationManager.AppSettings["api"] + "/sms/vcode", JsonConvert.SerializeObject(loginfo));
            l.Info("获取短信验证码："+s);
            var info = JsonConvert.DeserializeObject<Root>(s);
            if (info != null && info.result != null)
            {
                return Ok(true);
            }
            else if (info != null && info.message != null)
            {
                return Ok(info.message);
            }
            else
            {
                return Ok(false);
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="loginfo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Register([FromBody]ShopUserInfo loginfo)
        {

            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s = zlhttp.Post(ConfigurationManager.AppSettings["api"] + "/auth/register", JsonConvert.SerializeObject(loginfo));
            Log l = new Log();
            l.Info("注册：" + s);
            var info = JsonConvert.DeserializeObject<Root>(s);
            if (info != null && info.cellphone != null)
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@openid",loginfo.Openid),
                new SqlParameter("@fopenid",loginfo.FOpenid),
                new SqlParameter("@cellphone",info.cellphone),
                new SqlParameter("@photo",""),
                new SqlParameter("@userName",info.userName),
                new SqlParameter("@userId",info.storeUserId),
                new SqlParameter("@formID",loginfo.FormID)
                };
                string sql = @"exec [AddShopUser] @openid,@cellphone,@photo,@userName,@userId,@formID,1
                                exec [BindUser]  @fopenid,@openid";
                DbHelperSQL db = new DbHelperSQL();
                db.ExecuteSql(sql, paras);
                return Ok(true);
            }
            else if (info != null && info.message != null)
            {
                return Ok(info.message);
            }
            else
            {
                return Ok(false);
            }
        }
        /// <summary>
        /// 剩余返现金额
        /// </summary>
        /// <param name="loginfo"></param>
        /// <returns></returns>
        private float GetTotalRecharge()
        {
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s = zlhttp.Get(ConfigurationManager.AppSettings["api"] + "/auth/getTotalRecharge?key=sS5W8R7Ktt2bF7g4&startTime=2018-10-01 00:00:00&endTime=2018-10-10 00:00:00&activityId=6", "");
            Log l = new Log();
            l.Info("剩余返现金额：" + s);
            var info = JsonConvert.DeserializeObject<Root>(s);
            if (info != null && info.rechargeAmout != 0)
            {
                //float all = info.RechargeCashbackActivity.cashbackAggregateLimit;
                float use = (6000000 - info.rechargeAmout) / 10000;
                return use;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 拉取商城用户消费金额
        /// </summary>
        /// <param name="shopInfo"></param>
        /// <returns></returns>
        private Account Getamount(string userid)
        {
            //查询用户的商城ID
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            string s = zlhttp.Get(ConfigurationManager.AppSettings["api"] + "/rcActivity/shoppingRate?activityId=7&userId=" + userid, "");
            Log l = new Log();
            l.Info("查询用户的商城：" + s);
            var info = JsonConvert.DeserializeObject<Root>(s);
            //返现信息
            Account ac = new Account();
            if (info != null && info.data != null)
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@ShopInitRate",info.data.initRate),
                new SqlParameter("@ShopSpendingAmount",info.data.spendingAmount),
                new SqlParameter("@ShopUserID",userid),
                };
                string sql = @" update[JM_ShopUserInfo] set[ShopInitRate] =@ShopInitRate,[ShopSpendingAmount]=@ShopSpendingAmount  where[ShopUserID]=@ShopUserID";
                DbHelperSQL db = new DbHelperSQL();
                db.ExecuteSql(sql, paras);

                //float all = info.RechargeCashbackActivity.cashbackAggregateLimit;
                //翻译倍数
                float nowAmount = float.Parse(info.data.spendingAmount);

                if (nowAmount >= 15000)
                {
                    ac.yixiaofei = nowAmount;
                    ac.zaixiaofei = 0;
                    ac.beishu = 1;
                    ac.lang = 690;
                    ac.nowbeishu = 2.0;
                }
                else if (nowAmount >= 9000 && nowAmount < 15000)
                {
                    ac.yixiaofei = nowAmount;
                    ac.zaixiaofei = 15000 - nowAmount;
                    ac.beishu = 1;
                    ac.lang = (nowAmount - 9000) / 6000 * 162 + 162 * 3;
                    ac.nowbeishu = 1.8;
                }
                else if (nowAmount >= 6000 && nowAmount < 9000)
                {
                    ac.yixiaofei = nowAmount;
                    ac.zaixiaofei = 9000 - nowAmount;
                    ac.beishu = 0.8;
                    ac.lang = (nowAmount - 6000) / 3000 * 162 + 162 * 2;
                    ac.nowbeishu = 1.6;
                }
                else if (nowAmount >= 3000 && nowAmount < 6000)
                {
                    ac.yixiaofei = nowAmount;
                    ac.zaixiaofei = 6000 - nowAmount;
                    ac.beishu = 0.5;
                    ac.lang = (nowAmount - 3000) / 3000 * 162 + 162;
                    ac.nowbeishu = 1.3;
                }
                else
                {
                    ac.yixiaofei = nowAmount;
                    ac.zaixiaofei = 3000 - nowAmount;
                    ac.beishu = 0.3;
                    ac.lang = nowAmount / 3000 * 162;
                    ac.nowbeishu = 1;
                }
            }
            else
            {
                ac.yixiaofei = 0;
                ac.zaixiaofei = 0;
                ac.beishu = 0;
                ac.lang = 0;
                ac.nowbeishu = 1;
            }
            return ac;
        }

        /// <summary>
        /// 同步返现倍数
        /// </summary>
        /// <param name="shopInfo"></param>
        /// <returns></returns>
        private void Rate(Rote shopInfo)
        {

            // MD5Encrypt32('');
            ZLHttpRequet zlhttp = new ZLHttpRequet();
            shopInfo.sign = MD5Encrypt32(shopInfo.activityId + "|" + shopInfo.userId + "|" + shopInfo.openId + "|" + shopInfo.lastRate + "|kei3jw");
            string s = zlhttp.Post(ConfigurationManager.AppSettings["api"] + "/rcActivity/rate", JsonConvert.SerializeObject(shopInfo));
            Log l = new Log();
            l.Info("同步倍率：" + s);
        }

        /// <summary>
        /// 获取有多少位好友帮助
        /// </summary>
        /// <param name="wu"></param>
        /// <returns></returns>
        private List<string> GetHelp(string openid)
        {
            SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@Openid",openid), };
            string sql = @" select w.[AvatarUrl] from JM_Invitationinfo h left join[JM_WechatUserInfo] w on h.fopenid=w.Openid where h.openid=@Openid";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.Query(sql, paras);
            List<string> listHead = new List<string>();
            if (data.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    listHead.Add(data.Tables[0].Rows[i][0] + "");
                }
            }
            return listHead;
        }

        public static string MD5Encrypt32(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="wu"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUserInfoByOpenid([FromBody]WechatUInfo wu)
        {
            SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@Openid",wu.Openid), };
            string sql = @"   select [NickName],[AvatarUrl],ShopUserID from [JM_WechatUserInfo] w
  left join [JM_ShopUserInfo] s on w.openid=s.openid  where w.Openid=@Openid";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.Query(sql, paras);
            if (data.Tables[0].Rows.Count > 0)
            {
                return Ok(new WechatUInfo()
                {
                    nickName = data.Tables[0].Rows[0][0] + "",//昵称
                    avatarUrl = data.Tables[0].Rows[0][1] + "",//头像
                    ShopUserID = data.Tables[0].Rows[0][2] + "",//商城ID
                    Openid = wu.Openid,//Openid
                    Host = ConfigurationManager.AppSettings["api"],//Host
                });
            }
            else
            {
                return Ok("");
            }
        }

        private WechatUInfo Getwu(string openid)
        {
            SqlParameter[] paras = new SqlParameter[]
               {
                new SqlParameter("@Openid",openid), };
            string sql = @"   SELECT NickName,AvatarUrl  FROM [JM_WechatUserInfo] where openid=@Openid";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.Query(sql, paras);
            WechatUInfo wu = new WechatUInfo();
            if (data.Tables[0].Rows.Count > 0)
            {
                wu.nickName = data.Tables[0].Rows[0][0] + "";//昵称
                wu.avatarUrl = data.Tables[0].Rows[0][1] + "";//头像
            }
            return wu;
        }


        /// <summary>
        /// 查询返现信息
        /// </summary>
        /// <param name="wu"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetReappearance([FromBody]WechatUInfo wu)
        {
            //查询用户在商城的ID
            SqlParameter[] paras = new SqlParameter[]
                {
                new SqlParameter("@Openid",wu.Openid), };
            string sql = @"select COUNT(1) from JM_InvitationInfo  where Openid=(select Openid from JM_ShopUserInfo where ShopUserID='94061')";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.GetSingle(sql, paras);
            Reappearance ra = new Reappearance();
            if (data != null)
            {
                //获取商城用户消费金额
                ra.account = Getamount(data.ToString());
                //获取剩余总金额
                ra.remaining = GetTotalRecharge();
                //获取好友助力列表
                ra.friends = GetHelp(wu.Openid);
                //获取微信信息
                ra.wr = Getwu(wu.Openid);
                double bs = ra.account.beishu + Js(ra.friends.Count) + 1;
                //同步倍率
                Rate(new Rote()
                {
                    activityId = '7',
                    openId = wu.Openid,
                    userId = data.ToString(),
                    lastRate = bs
                });
            }
            else
            {
                return Ok(false);
            }
            return Ok(ra);
        }

        private double Js(int length)
        {
            if (length >= 20)
            {
                return 0.5;
            }
            else if (length >= 15)
            {
                return 0.2;
            }
            else
            {
                return 0;
            }
        }



        /// <summary>
        /// 查询特惠单品
        /// </summary>
        /// <param name="wu"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGoods()
        {
            //查询特惠单品
            string sql = "select * from jm_goods where goodstype=1";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.Query(sql);
            List<Goods> goods = new List<Goods>();
            if (data.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    goods.Add(new Goods()
                    {
                        GoodsName = data.Tables[0].Rows[i]["GoodsName"] + "",
                        GoodsPrice = data.Tables[0].Rows[i]["GoodsPrice"] + "",
                        GoodsFree = data.Tables[0].Rows[i]["GoodsFree"] + "",
                        GoodsImg = data.Tables[0].Rows[i]["GoodsImg"] + "",
                        GoodsColumn = data.Tables[0].Rows[i]["GoodsColumn"] + "",
                    });
                }
            }

            //查询爆款
            string sql1 = "select * from jm_goods where goodstype=2";
            var data1 = db.Query(sql1);
            List<Goods> goods1 = new List<Goods>();
            if (data1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < data1.Tables[0].Rows.Count; i++)
                {
                    goods1.Add(new Goods()
                    {
                        GoodsName = data1.Tables[0].Rows[i]["GoodsName"] + "",
                        GoodsPrice = data1.Tables[0].Rows[i]["GoodsPrice"] + "",
                        GoodsFree = data1.Tables[0].Rows[i]["GoodsFree"] + "",
                        GoodsImg = data1.Tables[0].Rows[i]["GoodsImg"] + "",
                        GoodsColumn = data1.Tables[0].Rows[i]["GoodsColumn"] + "",
                    });
                }
            }



            return Ok(new GoodsAll() {
                danpin=goods,
                baokuan=goods1
            });
        }

        /// <summary>
        /// 查询特惠单品
        /// </summary>
        /// <param name="wu"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetGoodsTj()
        {
            //查询必买
            string sql = "select * from jm_goods where goodstype=3";
            DbHelperSQL db = new DbHelperSQL();
            var data = db.Query(sql);
            List<Goods> goods = new List<Goods>();
            if (data.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < data.Tables[0].Rows.Count; i++)
                {
                    goods.Add(new Goods()
                    {
                        GoodsName = data.Tables[0].Rows[i]["GoodsName"] + "",
                        GoodsPrice = data.Tables[0].Rows[i]["GoodsPrice"] + "",
                        GoodsFree = data.Tables[0].Rows[i]["GoodsFree"] + "",
                        GoodsImg = data.Tables[0].Rows[i]["GoodsImg"] + "",
                        GoodsColumn = data.Tables[0].Rows[i]["GoodsColumn"] + "",
                    });
                }
            }
            return Ok(goods);
        }


    }

    public class User
    {
        public string name { get; set; }
        public string age { get; set; }
    }
    public class GoodsAll
    {
        public List<Goods>  danpin { get; set; }
        public List<Goods>  baokuan { get; set; }
    }
    /// <summary>
    /// 微信中的用户信息和sessionkey
    /// </summary>
    public class WechatUInfo
    {
        public string Openid { get; set; }
        public string SessionKey { get; set; }
        public string nickName { get; set; }
        public string avatarUrl { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string ShopUserID { get; set; }
        public string Host { get; set; }
    }
    /// <summary>
    /// 商城用户信息
    /// </summary>
    public class ShopUserInfo
    {
        public string Cellphone { get; set; }
        public string Password { get; set; }
        public string Smscode { get; set; }
        public string Rand { get; set; }
        public string Captcha { get; set; }
        public string FormID { get; set; }
        public string ValidateType { get; set; }
        public string Openid { get; set; }
        public string FOpenid { get; set; }
        public string registSource { get; set; }
        

    }
    public class ShopLoginUserInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FormID { get; set; }
        public string Openid { get; set; }
    }
    public class ErrorsItem
    {
        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// [Forbidden] - 服务器拒绝执行该请求
        /// </summary>
        public string reason { get; set; }
    }
    public class RechargeCashbackActivity
    {
        /// <summary>
        /// 
        /// </summary>
        public string activityId { get; set; }
        /// <summary>
        /// 10.01充值活动
        /// </summary>
        public string activityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activityStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float cashbackAggregateLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deleted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string displayOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isCashbackLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isRechargeLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastUpdate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string maxRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rechargeAggregateLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rechargeCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rechargeItems { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string startTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public double initRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string spendingAmount { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ErrorsItem> errors { get; set; }
        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string photo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cashAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string cityCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deleted { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string displayOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string growthValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastUpdate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string leverName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passwordPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string passwordSalt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pointAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string presentAccount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string provinceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string provinceName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string registSource { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string storeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string storeUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userLevelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vipcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RechargeCashbackActivity RechargeCashbackActivity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float rechargeAmout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lastRate { get; set; }
    }
    public class Rote
    {
        /// <summary>
        /// 
        /// </summary>
        public int activityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lastRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sign { get; set; }
    }

    /// <summary>
    /// 商城返现信息
    /// </summary>
    public class Reappearance
    {
        /// <summary>
        /// 返现信息
        /// </summary>
        public Account account { get; set; }

        public float remaining { get; set; }

        public List<string> friends { get; set; }
        public WechatUInfo wr { get; set; }
    }
    /// <summary>
    /// 返现信息
    /// </summary>
    public class Account
    {
        /// <summary>
        /// 
        /// </summary>
        public float yixiaofei { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float zaixiaofei { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Double beishu { get; set; }
        public Double lang { get; set; }
        public Double nowbeishu { get; set; }
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    public class Goods
    {
        public string GoodsName { get; set; }
        public string GoodsPrice { get; set; }
        public string GoodsFree { get; set; }
        public string GoodsImg { get; set; }
        public string GoodsColumn { get; set; }
    }
}