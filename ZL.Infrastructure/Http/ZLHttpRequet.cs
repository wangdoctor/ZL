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
            byte[] bytes = Encoding.ASCII.GetBytes(postdata);
            request.ContentType = "application/json";
            request.ContentLength = postdata.Length;
            Stream sendStream = request.GetRequestStream();
            sendStream.Write(bytes, 0, bytes.Length);
            sendStream.Close();

            //得到返回值  
            WebResponse response7 = request.GetResponse();
            string OrderQuantity = new StreamReader(response7.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();

            //转化成json对象处理  
            //List<GetOrderQuantity> getOrderQuantity = sr.Deserialize<List<GetOrderQuantity>>(OrderQuantity);  
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

            //得到返回值  
            WebResponse response7 = request.GetResponse();
            string OrderQuantity = new StreamReader(response7.GetResponseStream(), Encoding.GetEncoding("gb2312")).ReadToEnd();

            //转化成json对象处理  
            //List<GetOrderQuantity> getOrderQuantity = sr.Deserialize<List<GetOrderQuantity>>(OrderQuantity);  
            return OrderQuantity;



        }
    }
}
