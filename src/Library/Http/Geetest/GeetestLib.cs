using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Microservice.Library.Http.Geetest
{
    /// <summary>
    /// GeetestLib 极验验证C# SDK基本库
    /// </summary>
    public class GeetestLib
    {
        /// <summary>
        /// SDK版本号
        /// </summary>
        public const String version = "3.2.0";
        /// <summary>
        /// SDK开发语言
        /// </summary>
        public const String sdkLang = "csharp";
        /// <summary>
        /// 极验验证API URL
        /// </summary>
        protected const String apiUrl = "http://api.geetest.com";
        /// <summary>
        /// register url
        /// </summary>
        protected const String registerUrl = "/register.php";
        /// <summary>
        /// validate url
        /// </summary>
        protected const String validateUrl = "/validate.php";
        /// <summary>
        /// 极验验证API服务状态Session Key
        /// </summary>
        public const String gtServerStatusSessionKey = "gt_server_status";
        /// <summary>
        /// 极验验证二次验证表单数据 Chllenge
        /// </summary>
        public const String fnGeetestChallenge = "geetest_challenge";
        /// <summary>
        /// 极验验证二次验证表单数据 Validate
        /// </summary>
        public const String fnGeetestValidate = "geetest_validate";
        /// <summary>
        /// 极验验证二次验证表单数据 Seccode
        /// </summary>
        public const String fnGeetestSeccode = "geetest_seccode";
        private String userID = "";
        private String responseStr = "";
        private readonly String captchaID = "";
        private readonly String privateKey = "";
        private String client_type = "web";
        private String ip_address = "";

        /// <summary>
        /// 验证成功结果字符串
        /// </summary>
        public const int successResult = 1;
        /// <summary>
        /// 证结失败验果字符串
        /// </summary>
        public const int failResult = 0;
        /// <summary>
        /// 判定为机器人结果字符串
        /// </summary>
        public const String forbiddenResult = "forbidden";

        private readonly int timeout;

        private readonly IWebProxy proxy;

        /// <summary>
        /// GeetestLib构造函数
        /// </summary>
        /// <param name="publicKey">极验验证公钥</param>
        /// <param name="privateKey">极验验证私钥</param>
        /// <param name="timeout"></param>
        /// <param name="proxy"></param>
        public GeetestLib(String publicKey, String privateKey, int timeout = 20000, string proxy = null)
        {
            this.privateKey = privateKey;
            this.captchaID = publicKey;
            this.timeout = timeout;

            if (!string.IsNullOrEmpty(proxy))
            {
                this.proxy = new WebProxy(new Uri(proxy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetRandomNum()
        {
            Random rand = new Random();
            int randRes = rand.Next(100);
            return randRes;
        }

        /// <summary>
        /// 验证初始化预处理
        /// </summary>
        /// <returns>初始化结果</returns>
        public Byte PreProcess(string userID = "", string client_type = "web", string ip_address = "")
        {
            if (this.captchaID == null)
            {
                Console.WriteLine("publicKey is null!");
            }
            else
            {
                this.userID = userID;
                this.client_type = client_type;
                this.ip_address = ip_address;
                String challenge = this.RegisterChallenge();
                if (challenge.Length == 32)
                {
                    this.GetSuccessPreProcessRes(challenge);
                    return 1;
                }
                else
                {
                    this.GetFailPreProcessRes();
                    Console.WriteLine("Server regist challenge failed!");
                }
            }

            return 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetResponseStr()
        {
            return this.responseStr;
        }

        /// <summary>
        /// 预处理失败后的返回格式串
        /// </summary>
        private void GetFailPreProcessRes()
        {
            int rand1 = this.GetRandomNum();
            int rand2 = this.GetRandomNum();
            String md5Str1 = this.Md5Encode(rand1 + "");
            String md5Str2 = this.Md5Encode(rand2 + "");
            String challenge = md5Str1 + md5Str2.Substring(0, 2);
            this.responseStr = "{" + string.Format(
                 "\"success\":{0},\"gt\":\"{1}\",\"challenge\":\"{2}\",\"new_captcha\":{3}", 0,
                this.captchaID, challenge, "true") + "}";
        }

        /// <summary>
        /// 预处理成功后的标准串
        /// </summary>
        private void GetSuccessPreProcessRes(String challenge)
        {
            challenge = this.Md5Encode(challenge + this.privateKey);
            this.responseStr = "{" + string.Format(
              "\"success\":{0},\"gt\":\"{1}\",\"challenge\":\"{2}\",\"new_captcha\":{3}", 1,
              this.captchaID, challenge, "true") + "}";
            //                 "\"success\":{0},\"gt\":\"{1}\",\"challenge\":\"{2}\"", 1,
            //               this.captchaID, challenge) + "}";
        }

        /// <summary>
        /// failback模式的验证方式
        /// </summary>
        /// <param name="challenge">failback模式下用于与validate一起解码答案， 判断验证是否正确</param>
        /// <param name="validate">failback模式下用于与challenge一起解码答案， 判断验证是否正确</param>
        /// <param name="seccode">failback模式下，其实是个没用的参数</param>
        /// <returns>验证结果</returns>
        public int FailbackValidateRequest(String challenge, String validate, String seccode)
        {
            if (!this.RequestIsLegal(challenge, validate, seccode)) return GeetestLib.failResult;
            int validateResult = this.Failback_check_result(challenge, validate);
            return validateResult;
        }

        private Boolean RequestIsLegal(String challenge, String validate, String seccode)
        {
            if (challenge?.Length == 0 || validate?.Length == 0 || seccode?.Length == 0) return false;
            return true;
        }

        /// <summary>
        /// 向gt-server进行二次验证
        /// </summary>
        /// <param name="challenge">本次验证会话的唯一标识</param>
        /// <param name="validate">拖动完成后server端返回的验证结果标识字符串</param>
        /// <param name="seccode">验证结果的校验码，如果gt-server返回的不与这个值相等则表明验证失败</param>
        /// <returns>二次验证结果</returns>
        public int EnhencedValidateRequest(String challenge, String validate, String seccode)
        {
            if (!this.RequestIsLegal(challenge, validate, seccode)) return GeetestLib.failResult;
            if (validate.Length > 0 && CheckResultByPrivate(challenge, validate))
            {
                String query = "seccode=" + seccode + "&sdk=csharp_" + GeetestLib.version;
                String response = "";
                try
                {
                    response = PostValidate(query);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
#pragma warning restore CA1031 // Do not catch general exception types
                if (response.Equals(Md5Encode(seccode)))
                {
                    return GeetestLib.successResult;
                }
            }
            return GeetestLib.failResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="validate"></param>
        /// <param name="seccode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int EnhencedValidateRequest(String challenge, String validate, String seccode, String userID)
        {
            if (!this.RequestIsLegal(challenge, validate, seccode)) return GeetestLib.failResult;
            if (validate.Length > 0 && CheckResultByPrivate(challenge, validate))
            {
                String query = "seccode=" + seccode + "&user_id=" + userID + "&sdk=csharp_" + GeetestLib.version;
                String response = "";
                try
                {
                    response = PostValidate(query);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
#pragma warning restore CA1031 // Do not catch general exception types
                if (response.Equals(Md5Encode(seccode)))
                {
                    return GeetestLib.successResult;
                }
            }
            return GeetestLib.failResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private String ReadContentFromGet(String url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout;
                if (proxy != null)
                {
                    request.Proxy = proxy;
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                String retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            {
                return "";
            }
#pragma warning restore CA1031 // Do not catch general exception types

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private String RegisterChallenge()
        {
            string url;
            if (string.Empty.Equals(this.userID))
            {
                url = string.Format("{0}{1}?gt={2}&client_type={3}&ip_address={4}", GeetestLib.apiUrl, GeetestLib.registerUrl, this.captchaID, this.client_type, this.ip_address);
            }
            else
            {
                url = string.Format("{0}{1}?gt={2}&user_id={3}&client_type={4}&ip_address={5}", GeetestLib.apiUrl, GeetestLib.registerUrl, this.captchaID, this.userID, this.client_type, this.ip_address);
            }
            string retString = this.ReadContentFromGet(url);
            return retString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        private Boolean CheckResultByPrivate(String origin, String validate)
        {
            String encodeStr = Md5Encode(privateKey + "geetest" + origin);
            return validate.Equals(encodeStr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private String PostValidate(String data)
        {
            String url = string.Format("{0}{1}", GeetestLib.apiUrl, GeetestLib.validateUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = timeout;
            if (proxy != null)
            {
                request.Proxy = proxy;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(data);
            // 发送数据
            Stream myRequestStream = request.GetRequestStream();
            byte[] requestBytes = System.Text.Encoding.ASCII.GetBytes(data);
            myRequestStream.Write(requestBytes, 0, requestBytes.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // 读取返回信息
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private String Md5Encode(String plainText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(plainText)));
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        private int Failback_check_result(String challenge, String validate)
        {
            String encodeStr = this.Md5Encode(challenge);
            if (encodeStr == validate) { return GeetestLib.successResult; }
            else { return GeetestLib.failResult; }
        }

    }
}
