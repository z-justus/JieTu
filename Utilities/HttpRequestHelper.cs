using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class HttpRequestHelper
    {
        public static string Post(string url, string requestFormStr, bool isJsonFormat = false)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;
            CookieContainer cookieContainer = new CookieContainer();
            httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            if (isJsonFormat)
            {
                httpWebRequest.ContentType = "application/json;charset=UTF-8";
            }
            else
            {
                httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            }
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:19.0) Gecko/20100101 Firefox/19.0";
            byte[] bytes = Encoding.UTF8.GetBytes(requestFormStr);
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Timeout = 1800000;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            httpWebResponse.Close();
            return text;
        }
    }
}
