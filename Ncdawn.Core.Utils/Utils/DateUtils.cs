using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.Core.Utils
{
    /// <summary>
    /// 日期工具
    /// </summary>
    public class DateUtils
    {

        #region 将对象转换为日期时间类型
        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str, DateTime defValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime dateTime;
                if (DateTime.TryParse(str, out dateTime))
                    return dateTime;
            }
            return defValue;
        }
        #endregion

        #region 将对象转换为日期时间类型
        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime StrToDateTime(string str)
        {
            return StrToDateTime(str, DateTime.Now);
        }
        #endregion

        #region 将对象转换为日期时间类型
        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj)
        {
            return StrToDateTime(obj.ToString());
        }
        #endregion

        #region 将对象转换为日期时间类型
        /// <summary>
        /// 将对象转换为日期时间类型
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            return StrToDateTime(obj.ToString(), defValue);
        }
        #endregion

        #region 将对象转换为日期时间格式字符串
        /// <summary>
        /// 将对象转换为日期时间格式字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="formatter">要转换的对象类型; 例如yyyyMMdd</param>
        /// <returns>转换后的日期格式字符串类型结果</returns>
        public static string ObjectToDateTimeStr(object obj, string formatter)
        {
            string rtnStr = "";
            if (null == obj)
            {
                return rtnStr;
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(formatter))
                    {
                        formatter = "yyyy-MM-dd HH:mm:ss";
                    }
                    rtnStr = StrToDateTime(obj.ToString()).ToString(formatter);
                }
                catch (Exception)
                {

                }
                return rtnStr;
            }
        }
        #endregion

        #region 将对象转换为日期时间格式字符串
        /// <summary>
        /// 将对象转换为日期时间格式字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="formatter">要转换的对象类型; 例如yyyyMMdd</param>
        /// <returns>转换后的日期格式字符串类型结果</returns>
        public static string ObjectToDateTimeStr(object obj)
        {
            return ObjectToDateTimeStr(obj, null);
        }
        #endregion

        #region 计算两个日期之间天数
        /// <summary>
        /// 计算两个日期之间天数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int CalcTotalDays(DateTime start, DateTime end)
        {
            int total = new TimeSpan(end.Ticks - start.Ticks).Days;
            return total;
        }
        #endregion
    }
}
