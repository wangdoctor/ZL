using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZL.Infrastructure;

namespace ZL.Web.Controllers
{
    public class MPController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]WechatLoginInfo loginfo)
        {
            WeChatAppDecrypt wcad = new WeChatAppDecrypt("wx20663550f030a33f", "5d2ad1a80d1dbf84040ca2ea06c3a96d");

            var ddd = wcad.DecodeOpenIdAndSessionKey(loginfo);
                //new WechatLoginInfo()
            //{
            //    code = "011Y4LA02peroZ0H91y02YUpA02Y4LAg",
            //    encryptedData = "WXDO/llRYLhs24UpBeP6a+V4fZqYhUTJt/kKidYyp3TbGsE3wE7yuk3seQjZPV+vK/r0VG7vzu5FrV6c2h8u7KE6r2vn/GqAURunXO42hd14cDvJw7AMecTRCjf3r8xLba5JNU3xTNXcONRm98LTP3MN6TRvH69WqIrufZGoMh3nAd54mmpiFAlHPK8yUVsSD+nHA57roV70LOLGDT8dtyAXApbVdwcLlya1JMwBfcXd6U0QFFa+Q77rhJVFfCzUQGw7rAjijfT5lVnnBhSEkBQXWPjzRdiQZ7aD7/m9Fb9vXJpXlDH48HV9PsPcuW1R5Yl68ZedEUaX3raSsDZbojSK0csDq6oOF+hoISPlIPo7axp6gsvJ3fC1QLVYHHv/DqY4XaM9yWeEeih+C+8GinzJOzNso9SN5WGfHJbEocGqskiUHNrPGDMGU+A1B8i3296Q3Xw/+kuZHEj7zjqDerPVdgkdNLwxPMV444BnviM=",
            //    iv = "eBXlugT85RWW9s9q3ur4jg==",
            //    rawData = "{'nickName':'王博士','gender':1,'language':'zh_CN','city':'Liaocheng','province':'Shandong','country':'China','avatarUrl':'https://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKcthg7CBtDdBSrpX5RDyrLxicctz8V2LXgIJXCzdpicAJ9r1kRWRYcLtiallWkEI73wzEiaJicOImW9uw/132'}",
            //    signature = "ad8eee39a418dc83f62ba950ad9d340ae3e8581a",
            //});
            Log l = new Log();
            l.Info(ddd.openid);

            //var d = wcad.Decrypt(new WechatLoginInfo()
            //{
            //    code = "011Y4LA02peroZ0H91y02YUpA02Y4LAg",
            //    encryptedData = "WXDO/llRYLhs24UpBeP6a+V4fZqYhUTJt/kKidYyp3TbGsE3wE7yuk3seQjZPV+vK/r0VG7vzu5FrV6c2h8u7KE6r2vn/GqAURunXO42hd14cDvJw7AMecTRCjf3r8xLba5JNU3xTNXcONRm98LTP3MN6TRvH69WqIrufZGoMh3nAd54mmpiFAlHPK8yUVsSD+nHA57roV70LOLGDT8dtyAXApbVdwcLlya1JMwBfcXd6U0QFFa+Q77rhJVFfCzUQGw7rAjijfT5lVnnBhSEkBQXWPjzRdiQZ7aD7/m9Fb9vXJpXlDH48HV9PsPcuW1R5Yl68ZedEUaX3raSsDZbojSK0csDq6oOF+hoISPlIPo7axp6gsvJ3fC1QLVYHHv/DqY4XaM9yWeEeih+C+8GinzJOzNso9SN5WGfHJbEocGqskiUHNrPGDMGU+A1B8i3296Q3Xw/+kuZHEj7zjqDerPVdgkdNLwxPMV444BnviM=",
            //    iv = "eBXlugT85RWW9s9q3ur4jg==",
            //    rawData = "{'nickName':'王博士','gender':1,'language':'zh_CN','city':'Liaocheng','province':'Shandong','country':'China','avatarUrl':'https://wx.qlogo.cn/mmopen/vi_32/Q0j4TwGTfTKcthg7CBtDdBSrpX5RDyrLxicctz8V2LXgIJXCzdpicAJ9r1kRWRYcLtiallWkEI73wzEiaJicOImW9uw/132'}",
            //    signature = "ad8eee39a418dc83f62ba950ad9d340ae3e8581a",
            //});
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
    public class User
    {
        public string name { get; set; }
        public string age { get; set; }
    }
}