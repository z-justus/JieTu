using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using Module;
using Utilities;
using System.Windows.Forms;

namespace BaiduAIAPI
{
    public class AccessToken
    {
        private readonly static string _AppID;
        private readonly static string _APIKey;
        private readonly static string _SecretKey;
        private readonly static string _AuthHostUrl;

        static AccessToken()
        {
            //读取config配置
            _AppID = ConfigurationManager.AppSettings["AppID"].ToString();
            _APIKey = ConfigurationManager.AppSettings["APIKey"].ToString();
            _SecretKey = ConfigurationManager.AppSettings["SecretKey"].ToString();
            _AuthHostUrl = ConfigurationManager.AppSettings["AuthHostUrl"].ToString();
        }

        /// <summary>
        /// 获取令牌
        /// </summary>
        /// <returns></returns>
        public static AccessTokenView GetAccessToken()
        {

            AccessTokenView accessTokenView = new AccessTokenView();
            string tokenData = GetBaiduAccessToken();
            if (tokenData.Contains("\"error\""))
            {//标识异常

                accessTokenView.IsSuccess = false;
                accessTokenView.SuccessModel = null;
                accessTokenView.ErrorModel = JsonExtends.ToObject<ErrorAccessTokenModel>(tokenData);
            }
            else
            {//标识正常
                accessTokenView.IsSuccess = true;
                accessTokenView.SuccessModel = JsonExtends.ToObject<AccessTokenModel>(tokenData);
                accessTokenView.ErrorModel = null;
            }
            return accessTokenView;
        }

        public static string GetBaiduRecognizeUrl()
        {
            RecognizePattern pattern = GetBaiduRecognizePattern();
            return pattern == RecognizePattern.高精度版 
                                    ? "https://aip.baidubce.com/rest/2.0/ocr/v1/accurate_basic"
                                    : "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
        }

        private static string GetBaiduAccessToken()
        {
            HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> contentParaList = new List<KeyValuePair<string, string>>();
            contentParaList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));//参考地址：http://ai.baidu.com/docs#/Auth/top
            contentParaList.Add(new KeyValuePair<string, string>("client_id", _APIKey));
            contentParaList.Add(new KeyValuePair<string, string>("client_secret", _SecretKey));
            HttpResponseMessage response = client.PostAsync(_AuthHostUrl, new FormUrlEncodedContent(contentParaList)).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        private static RecognizePattern GetBaiduRecognizePattern()
        {
            string highPrecision = ConfigurationManager.AppSettings["HighPrecision"].ToString();
            if (string.IsNullOrWhiteSpace(highPrecision))
            {
                return RecognizePattern.普通版;
            }

            if (bool.TryParse(highPrecision, out bool isHighPrecision))
            {
                return isHighPrecision ? RecognizePattern.高精度版 : RecognizePattern.普通版;
            }
            else
            {
                return RecognizePattern.普通版;
            }
        }

        private enum RecognizePattern
        {
            普通版 = 0,
            高精度版 
        }
    }
}
