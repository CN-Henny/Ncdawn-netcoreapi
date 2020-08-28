using System;
using System.IO;
using System.Net;
using System.Text;

namespace Ncdawn.Core.Common
{
    public class AMapHelper
    {
        //地球半径，单位米
        private const double EARTH_RADIUS = 6378137;
        public static double pi = 3.14159265358979324;
        public static double a = 6378245.0;
        public static double ee = 0.00669342162296594323;

        const string key = "140efcaf5bd7dec821d42c0b630c542c";

        /// <summary>
        /// 根据经纬度获取地址
        /// </summary>
        /// <param name="LngLatStr">经度纬度组成的字符串 例如:"113.692100,34.752853"</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns>失败返回"" </returns>
        public static string GetLocationByLngLat(string LngLatStr, int timeout = 10000)
        {
            string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={LngLatStr}";
            return GetLocationByURL(url, timeout);
        }

        /// <summary>
        /// 根据经纬度获取地址
        /// </summary>
        /// <param name="lng">经度 例如:113.692100</param>
        /// <param name="lat">维度 例如:34.752853</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns>失败返回"" </returns>
        public static string GetLocationByLngLat(double lng, double lat, int timeout = 10000)
        {
            string url = $"http://restapi.amap.com/v3/geocode/regeo?key={key}&location={lng},{lat}";
            return GetLocationByURL(url, timeout);
        }
        /// <summary>
        /// 根据URL获取地址
        /// </summary>
        /// <param name="url">Get方法的URL</param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns></returns>
        private static string GetLocationByURL(string url, int timeout = 10000)
        {
            string strResult = "";
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "multipart/form-data";
                req.Accept = "*/*";
                req.UserAgent = "";
                req.Timeout = timeout;
                req.Method = "GET";
                req.KeepAlive = true;
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = sr.ReadToEnd();
                }
                int formatted_addressIndex = strResult.IndexOf("formatted_address");
                int addressComponentIndex = strResult.IndexOf("addressComponent");
                int cutIndex = addressComponentIndex - formatted_addressIndex - 23;
                int subIndex = formatted_addressIndex + 20;
                return strResult.Substring(subIndex, cutIndex);
            }
            catch (Exception)
            {
                strResult = "";
            }
            return strResult;
        }

        /// <summary>
        /// 根据URL获取地址
        /// </summary>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <param name="timeout">超时时间默认10秒</param>
        /// <returns></returns>
        public static string GetGMapLocationByURL(double lng, double lat, int timeout = 10000)
        {
            string url = $"http://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&sensor=true";
            string strResult = "";
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.ContentType = "multipart/form-data";
                req.Accept = "*/*";
                req.UserAgent = "";
                req.Timeout = timeout;
                req.Method = "GET";
                req.KeepAlive = true;
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    strResult = sr.ReadToEnd();
                }
                int formatted_addressIndex = strResult.IndexOf("formatted_address");
                int addressComponentIndex = strResult.IndexOf("addressComponent");
                int cutIndex = addressComponentIndex - formatted_addressIndex - 23;
                int subIndex = formatted_addressIndex + 20;
                return strResult.Substring(subIndex, cutIndex);
            }
            catch (Exception)
            {
                strResult = "";
            }
            return strResult;
        }


    }
}
