using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pivotal.Discovery.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Ncdawn.Service.Account
{
    public interface IHttpRpcService : IBaseService
    {
        List<T> GetDataFromRemoteService<T>(string serviceName, string apiName, Dictionary<string, string> parameters = null);
        string GetDataJsonFromRemoteService(string serviceName, string apiName, Dictionary<string, string> parameters = null);
        T GetSingleDataFromRemoteService<T>(string serviceName, string apiName, Dictionary<string, string> parameters = null);
        void HttpPost(string serviceName, string apiName, Dictionary<string, string> urlparameters = null, Dictionary<string, string> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded");
        string HttpPostAsync(string serviceName, string apiName, IDictionary<string, string> parameters);
        string GetOldService(string url);
    }

    public class HttpRpcService : IBaseService, IHttpRpcService
    {
        public static DiscoveryHttpClientHandler _handler;
        public static HttpClient httpClient;
        public static string serviceAddress;
        public static string oldWebApi;
        private readonly ILogService logService;
        public HttpRpcService(IConfiguration configuration, IDiscoveryClient client, ILogService _LogService)
        {
            _handler = new DiscoveryHttpClientHandler(client);
            httpClient = new HttpClient(_handler);
            serviceAddress = configuration.GetSection("gateway")["serviceAddress"];
            oldWebApi = configuration.GetSection("OldWebApi").Value;
            logService = _LogService;
        }

        #region 获取请求全路径
        /// <summary>
        /// 获取请求全路径
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string getPRC_Url(string serviceName, string apiName, Dictionary<string, string> parameters)
        {
            if (parameters == null)
            {
                return "http://" + serviceAddress + "/" + serviceName + "/" + apiName;
            }
            else
            {
                string paramstr = string.Empty;
                foreach (string key in parameters.Keys)
                {
                    paramstr += "&" + key + "=" + parameters[key];
                }
                return "http://" + serviceAddress + "/" + serviceName + "/" + apiName + "?" + paramstr.Substring(1);
            }
        }
        #endregion

        #region 取远程服务数据列表
        /// <summary>
        /// 取远程服务数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> GetDataFromRemoteService<T>(string serviceName, string apiName, Dictionary<string, string> parameters = null)
        {
            try
            {
                string url = getPRC_Url(serviceName, apiName, parameters);
                var t = httpClient.GetByteArrayAsync(url);
                Byte[] resultBytes = t.Result;
                string strtarget = Encoding.UTF8.GetString(resultBytes);
                if (strtarget != "null")
                {
                    return JsonConvert.DeserializeObject<List<T>>(strtarget);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 取远程单条数据
        /// <summary>
        /// 取远程单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceName"></param>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T GetSingleDataFromRemoteService<T>(string serviceName, string apiName, Dictionary<string, string> parameters = null)
        {
            try
            {
                string url = getPRC_Url(serviceName, apiName, parameters);
                logService.DLOG("HTTPRPC", "DH", "DCE", "CarCleanService", "GetDataFromRemoteService", "发送短信URL:" + url, "开始");
                var t = httpClient.GetByteArrayAsync(url);
                Byte[] resultBytes = t.Result;
                string strtarget = Encoding.UTF8.GetString(resultBytes);
                if (strtarget != "null")
                {
                    return JsonConvert.DeserializeObject<T>(strtarget);
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception)
            {
                return default(T);
            }

        }
        #endregion

        #region 取远程服务数据单条数据
        /// <summary>
        /// 取远程服务数据单条数据
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetDataJsonFromRemoteService(string serviceName, string apiName, Dictionary<string, string> parameters = null)
        {
            try
            {
                string url = getPRC_Url(serviceName, apiName, parameters);
                var t = httpClient.GetByteArrayAsync(url);
                Byte[] resultBytes = t.Result;
                string strtarget = Encoding.UTF8.GetString(resultBytes);
                if (strtarget != "null")
                {
                    return strtarget;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion

        #region 同步请求post（键值对形式）
        /// <summary>
        /// 同步请求post（键值对形式）
        /// </summary>
        /// <param name="serviceName">网络基址("http://localhost:59315")</param>
        /// <param name="apiName">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public void HttpPost(string serviceName, string apiName, Dictionary<string, string> urlparameters = null, Dictionary<string, string> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            try
            {
                string url = getPRC_Url(serviceName, apiName, urlparameters);
                HttpContent content = new FormUrlEncodedContent(formData);
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                content.Headers.ContentType.CharSet = charset;
                foreach (string key in formData.Keys)
                {
                    content.Headers.Add(key, formData[key]);
                }
                httpClient.PostAsync(url, content);
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region 异步请求post，包体形式
        /// <summary>
        /// 异步请求post，包体形式
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="apiName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string HttpPostAsync(string serviceName, string apiName, IDictionary<string, string> parameters)
        {
            string url = getPRC_Url(serviceName, apiName, null);
            string body = "{";
            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                foreach (string key in parameters.Keys)
                {
                    buffer.AppendFormat("\"{0}\":\"{1}\",", key, parameters[key]);
                }
                body += buffer.ToString() + "}";
            }
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 20000;
                byte[] btBodys = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = btBodys.Length;
                httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                return responseContent;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 取老接口数据单条数据
        /// <summary>
        /// 取老接口数据单条数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetOldService(string url)
        {
            try
            {
                url = oldWebApi + url;
                var t = httpClient.GetByteArrayAsync(url);
                Byte[] resultBytes = t.Result;
                string strtarget = Encoding.UTF8.GetString(resultBytes);
                if (strtarget != "null")
                {
                    return strtarget;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }
        #endregion
    }
}
