using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ncdawn.Core.SqlSugor
{
    public class SqlSugor
    {
        #region Expression 转成 where
        /// <summary>
        /// Expression 转成 Where String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="databaseType">数据类型（用于字段是否加引号）</param>
        /// <param name="IsDeleteFlag">删除标识是否开启</param>
        /// <returns></returns>
        public static string GetWhereStr<T>(Expression<Func<T, bool>> predicate, string databaseType, bool IsDeleteFlag)
        {
            string strWhere = string.Empty;
            if (predicate != null)
            {
                bool withQuotationMarks = GetWithQuotationMarks(databaseType);

                ConditionBuilder conditionBuilder = new ConditionBuilder();
                conditionBuilder.SetIfWithQuotationMarks(withQuotationMarks); //字段是否加引号（PostGreSql,Oracle）
                conditionBuilder.SetDataBaseType(databaseType);
                conditionBuilder.Build(predicate);

                for (int i = 0; i < conditionBuilder.Arguments.Length; i++)
                {
                    object ce = conditionBuilder.Arguments[i];
                    if (ce == null)
                    {
                        conditionBuilder.Arguments[i] = DBNull.Value;
                    }
                    else if (ce is string || ce is char)
                    {
                        if (ce.ToString().ToLower().Trim().IndexOf(@"in(") == 0 ||
                            ce.ToString().ToLower().Trim().IndexOf(@"not in(") == 0 ||
                             ce.ToString().ToLower().Trim().IndexOf(@" like '") == 0 ||
                            ce.ToString().ToLower().Trim().IndexOf(@"not like") == 0)
                        {
                            conditionBuilder.Arguments[i] = string.Format(" {0} ", ce.ToString());
                        }
                        else
                        {


                            //****************************************
                            conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                        }
                    }
                    else if (ce is DateTime)
                    {
                        conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                    }
                    else if (ce is int || ce is long || ce is short || ce is decimal || ce is double || ce is float || ce is bool || ce is byte || ce is sbyte)
                    {
                        conditionBuilder.Arguments[i] = ce.ToString();
                    }
                    else if (ce is ValueType)
                    {
                        conditionBuilder.Arguments[i] = ce.ToString();
                    }
                    else if (ce is List<string>)
                    {
                        List<string> cel = (List<string>)ce;
                        conditionBuilder.Arguments[i] = string.Empty;
                        if (cel.Count > 0)
                        {
                            foreach (var item in cel)
                            {
                                conditionBuilder.Arguments[i] += "," + string.Format("'{0}'", item.ToString());
                            }
                            conditionBuilder.Arguments[i] = conditionBuilder.Arguments[i].ToString().Substring(1);
                        }
                        else
                        {
                            conditionBuilder.Arguments[i] = "''";
                        }
                    }
                    else
                    {

                        conditionBuilder.Arguments[i] = string.Format("'{0}'", ce.ToString());
                    }

                }
                strWhere = string.Format(conditionBuilder.Condition, conditionBuilder.Arguments);
            }
            if (IsDeleteFlag)
            {
                strWhere += string.IsNullOrEmpty(strWhere) ? "(IsDelete = '0')" : "and (IsDelete = '0')";
            }
            return strWhere;
        }

        public static string GetById(string Id, string databaseType)
        {
            string strWhere = "(Id = '" + Id + "') and (IsDelete = '0')";
            return strWhere;
        }

        #region 获取是否字段加双引号
        /// <summary>
        /// 获取是否字段加双引号
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static bool GetWithQuotationMarks(string databaseType)
        {
            bool result = false;
            switch (databaseType.ToLower())
            {

                case DataBaseType.PostGreSql:
                case DataBaseType.Oracle:

                    result = true;
                    break;
            }

            return result;


        }
        #endregion

        #endregion
    }
}
