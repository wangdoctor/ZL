using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZL.Infrastructure
{
    public class ZLHttpRequet
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public string Post(string url, string postdata)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            //post传参数  
            byte[] bytes = Encoding.UTF8.GetBytes(postdata);
            request.ContentType = "application/json;charset=UTF-8";
            request.ContentLength = postdata.Length;
            Stream sendStream = request.GetRequestStream();
            sendStream.Write(bytes, 0, bytes.Length);
            sendStream.Close();
            string OrderQuantity = string.Empty;
            try
            {
                var rsp = request.GetResponse() as HttpWebResponse; // 最好能捕获异常302的HttpException,然后再处理一下。在Data中取键值 Location  
                OrderQuantity = new StreamReader(rsp.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            catch (Exception ex)
            {
                var rsp = ((System.Net.WebException)ex).Response as HttpWebResponse;
                OrderQuantity = new StreamReader(rsp.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            return OrderQuantity;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public string Get(string url, string postdata)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            //post传参数  
            byte[] bytes = Encoding.ASCII.GetBytes(postdata);
            request.ContentType = "application/json";
            request.ContentLength = postdata.Length;
            Stream sendStream = request.GetRequestStream();
            sendStream.Write(bytes, 0, bytes.Length);
            sendStream.Close();
            string OrderQuantity = string.Empty;
            try
            {
                var rsp = request.GetResponse() as HttpWebResponse; // 最好能捕获异常302的HttpException,然后再处理一下。在Data中取键值 Location  
                OrderQuantity = new StreamReader(rsp.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            catch (Exception ex)
            {
                var rsp = ((System.Net.WebException)ex).Response as HttpWebResponse;
                OrderQuantity = new StreamReader(rsp.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            }
            return OrderQuantity;
        }
    }
}
