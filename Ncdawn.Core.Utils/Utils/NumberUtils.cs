using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ncdawn.Core.Utils
{
    /// <summary>
    /// 数字处理
    /// </summary>
    public class NumberUtils
    {

        #region Object型转换为decimal型
        /// <summary>
        /// Object型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal ObjToDecimal(object expression, decimal defValue)
        {
            if (expression != null)
                return StrToDecimal(expression.ToString(), defValue);

            return defValue;
        }
        #endregion

        #region string型转换为decimal型
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string expression, decimal defValue)
        {
            if (expression == null)
            {
                return defValue;

            }
            decimal intValue = defValue;
            if (expression != null)
            {
                bool IsDecimal = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(expression, out intValue);
            }
            return intValue;
        }
        #endregion

        #region 累加数据求和---适用于通过","拼接的数据
        /// <summary>
        /// 累加数据求和---适用于通过","拼接的数据
        /// </summary>
        /// <param name="data">数据 例：100.11,200.01</param>
        /// <returns></returns>
        public static decimal GetSumData(string data)
        {
            decimal dataResult = 0;
            if (!string.IsNullOrEmpty(data))
            {
                if (data.Contains(","))
                {
                    var datas = data.Split(',');
                    foreach (var Data in datas)
                    {
                        dataResult += StrToDecimal(Data, 0);
                    }
                }
                else
                {
                    dataResult = StrToDecimal(data, 0);
                }
            }
            return dataResult;
        }
        #endregion

        #region 获取数据，如果为空，返回--
        /// <summary>
        /// 获取数据，如果为空，返回--
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rtn"></param>
        /// <returns></returns>
        public static string GetValueDefault(object obj, string rtn = "--")
        {
            string str = rtn;
            if (null != obj)
            {
                decimal d = ObjToDecimal(obj, 0);
                if (d != 0)
                {
                    str = Math.Round(d, 2).ToString();
                }
            }
            return str;
        }
        #endregion

        #region 将对象转换为保留两位的字符串
        /// <summary>
        /// 将对象转换为保留两位的字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <param name="defValue">默认值</param>
        /// <param name="length">保留长度</param>
        /// <returns>转换后的string类型结果</returns>
        public static string NullableDecimalToStr(object obj, string defValue, int length)
        {
            if (obj == null)
                return defValue;
            var MR = Math.Round(ObjToDecimal(obj, 0), length);
            string MRStr = MR.ToString(), intStr = "0", decStr = "0";
            if (MRStr.IndexOf(".") != -1)
            {
                intStr = MRStr.Split('.')[0];
                decStr = MRStr.Split('.')[1];
            }
            else
            {
                intStr = MRStr;
            }
            decStr = decStr.PadRight(length, '0');
            return intStr.ToString() + "." + decStr;
        }
        #endregion

        #region string型转换为double型
        /// <summary>
        /// string型转换为double型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static double StrToDouble(string expression, double defValue)
        {
            if ((expression == null) || ((expression.IndexOf('.') == -1)))
                return defValue;
            if (expression.Split('.')[1].Length > 12)
            {
                expression = expression.Split('.')[0] + "." + expression.Split('.')[1].Substring(0, 12);
            }
            double dblValue = defValue;
            if (expression != null)
            {
                bool IsDouble = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDouble)
                    double.TryParse(expression, out dblValue);
            }
            return dblValue;
        }
        #endregion

        #region 将对象转换为Int32类型
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object expression, int defValue)
        {
            if (expression != null)
                return StrToInt(expression.ToString(), defValue);

            return defValue;
        }
        #endregion

        #region 将字符串转换为Int32类型
        /// <summary>
        /// 将字符串转换为Int32类型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string expression, int defValue)
        {
            if (string.IsNullOrEmpty(expression) || expression.Trim().Length >= 11 || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return defValue;

            int rv;
            if (Int32.TryParse(expression, out rv))
                return rv;

            return Convert.ToInt32(StrToFloat(expression, defValue));
        }
        #endregion

        #region double转Int，四舍五入
        /// <summary>
        /// double转Int，四舍五入
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int DoubleToInt(string expression)
        {
            if (string.IsNullOrEmpty(expression) || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
                return 0;
            int rv = Convert.ToInt32(expression.Split('.')[0]);
            int pointNumber = Convert.ToInt32(expression.Split('.')[1].Substring(0, 1));
            if (pointNumber >= 5)
            {
                rv = rv + 1;
            }
            return rv;
        }
        #endregion

        #region double转Int，取整
        /// <summary>
        /// double转Int，取整
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static int DoubleToIntOnly(string expression)
        {
            int rv = 0;
            if (string.IsNullOrEmpty(expression) || !Regex.IsMatch(expression.Trim(), @"^([-]|[0-9])[0-9]*(\.\w*)?$"))
            {
                return rv;
            }
            //如果字符串不带小数点，则直接转INT返回
            if (expression.IndexOf('.') == -1)
            {
                rv = Convert.ToInt32(expression);
            }
            else
            {
                rv = Convert.ToInt32(expression.Split('.')[0]);
            }
            return rv;
        }
        #endregion

        #region double保留指定位数double，四舍五入
        /// <summary>
        /// double保留指定位数double，四舍五入
        /// </summary>
        /// <param name="dValue"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double DoubleDecimals(double dValue, int decimals)
        {
            double rtnVal = 0;
            try
            {
                rtnVal = Math.Round(dValue, decimals);
            }
            catch (Exception)
            {
                rtnVal = 0;
            }
            return rtnVal;
        }
        #endregion

        #region Object型转换为float型
        /// <summary>
        /// Object型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object expression, float defValue)
        {
            if (expression != null)
                return StrToFloat(expression.ToString(), defValue);

            return defValue;
        }
        #endregion

        #region string型转换为float型
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="expression">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string expression, float defValue)
        {
            if ((expression == null) || (expression.Length > 10))
                return defValue;

            float intValue = defValue;
            if (expression != null)
            {
                bool IsFloat = Regex.IsMatch(expression, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                    float.TryParse(expression, out intValue);
            }
            return intValue;
        }
        #endregion

        #region 判断对象是否为Int32类型的数字
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
                return IsNumeric(expression.ToString());

            return false;

        }
        #endregion

        #region 判断对象是否为Int32类型的数字
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                        return true;
                }
            }
            return false;
        }
        #endregion

        #region 是否为Double类型
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");

            return false;
        }
        #endregion
    }
}
