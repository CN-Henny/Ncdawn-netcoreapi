using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ncdawn.EFS.DBContext;

namespace Ncdawn.Core.SqlSugor
{
    public static class TSqlFuncWork
    {
        /// <summary>
        /// Where语句:EF,DataBaseType.SqlServer, DataDeleteFlag.IsDelete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="predicate">Lambda</param>
        /// <param name="databaseType">数据库类型</param>
        /// <param name="IsDeleteFlag">是否开启删除标识</param>
        /// <returns></returns>
        public static SelectInfo<T> SetWhere<T>(this SelectInfo<T> selectInfo, Expression<Func<T, bool>> predicate, string databaseType, int IsDeleteFlag)
        {
            selectInfo.strWhere = SqlSugor.GetWhereStr<T>(predicate, DataBaseType.SqlServer, IsDeleteFlag == 1);
            return selectInfo;
        }
        /// <summary>
        /// 目前只支持单字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="tKey"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static SelectInfo<T> OrderBy<T, tKey>(this SelectInfo<T> selectInfo, Expression<Func<T, tKey>> predicate)
        {
            Expression body = predicate.Body;
            MemberExpression m = (MemberExpression)body;
            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            string orderByName = propertyInfo.Name;
            string orderByStr = " " + orderByName + " ASC ";
            //处理先后关系
            selectInfo.orderByStr.Add(selectInfo.orderByStr.Count + 1, orderByStr);
            return selectInfo;
        }
        /// <summary>
        /// 目前只支持单字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="tKey"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static SelectInfo<T> OrderByDescending<T, tKey>(this SelectInfo<T> selectInfo, Expression<Func<T, tKey>> predicate)
        {
            Expression body = predicate.Body;
            MemberExpression m = (MemberExpression)body;
            PropertyInfo propertyInfo = m.Member as PropertyInfo;
            string orderByName = propertyInfo.Name;
            string orderByStr = " " + orderByName + " DESC ";
            //处理先后关系
            selectInfo.orderByStr.Add(selectInfo.orderByStr.Count + 1, orderByStr);
            return selectInfo;
        }
        /// <summary>
        /// 设置查询类型:TableType.Table/View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="tableType"></param>
        /// <returns></returns>
        public static SelectInfo<T> SetType<T>(this SelectInfo<T> selectInfo, int tableType)
        {
            selectInfo.tableType = tableType;
            return selectInfo;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="pageNum"></param>
        /// <param name="recordNum"></param>
        /// <returns></returns>
        public static SelectInfo<T> SetPaging<T>(this SelectInfo<T> selectInfo, int pageNum, int recordNum = 0)
        {
            selectInfo.pageNum = pageNum;
            selectInfo.recordNum = recordNum;
            return selectInfo;
        }
        /// <summary>
        /// 是否转换成json返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectInfo"></param>
        /// <param name="jsonFlag"></param>
        /// <returns></returns>
        public static SelectInfo<T> ToJson<T>(this SelectInfo<T> selectInfo, bool jsonFlag)
        {
            selectInfo.jsonFlag = jsonFlag;
            return selectInfo;
        }
        #region select type
        public static GetByIdModel GetById<T>(this SelectInfo<T> selectInfo, string id)
        {
            GetByIdModel GetByIdModel = new GetByIdModel();
            return GetByIdModel;
        }
        public static SqlStrModel SetSqlStr<T>(this SelectInfo<T> selectInfo, string sqlWhereStr)
        {
            SqlStrModel sqlStrModel = new SqlStrModel();
            sqlStrModel.SqlStr = sqlWhereStr;
            return sqlStrModel;
        }
        #endregion
        public static CheckInfo Check<T>(this SelectInfo<T> selectInfo, ref List<ErrorList> erolt)
        {
            List<ErrorList> erolts = new List<ErrorList>();
            Dictionary<string, IConfigIO> retDic = new Dictionary<string, IConfigIO>();
            CheckInfo checkInfo = new CheckInfo();
            Type[] configIOList = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute<ConfigIOAttr>() != null).ToArray();
            foreach (Type type in configIOList)
            {
                Activator.CreateInstance(type.MakeGenericType(typeof(T)), selectInfo, erolts, checkInfo);
            }
            erolt = erolts;
            //检查错误
            return checkInfo;
        }
        #region 运行
        public static List<T> Run<T>(this CheckInfo checkInfo, MSSqlDBContext db, List<ErrorList> erolt)
        {
            var dbContextSqlQueryCommand = TsqlFactory.Select(checkInfo);
            return SelectDb<T>(dbContextSqlQueryCommand, db);
        }
        public static List<T> Run<T>(this CheckInfo checkInfo, MSSqlDBContext db, List<ErrorList> erolt, ref int DataCount)
        {
            //select * from PaasOLT_OrderCarClean order by Id offset((2 - 1) * 20) rows fetch next 20 rows only;
            //查询
            string TsqlCountStr = "select COUNT(1) from " + checkInfo.tableName + " ";
            //条件
            string strWhere = checkInfo.strWhere == null ? null : " where " + checkInfo.strWhere;
            //查询总数
            DbContextSqlQueryCommand dbContextSqlQueryCommand = new DbContextSqlQueryCommand();
            dbContextSqlQueryCommand.QueryType = "text";
            dbContextSqlQueryCommand.Query = TsqlCountStr + strWhere;
            int DbRequest = (int)DbContextExtensions.QueryObject(new MSSqlDBContext(), new DbContextSqlQueryCommands
            {
                Sql = dbContextSqlQueryCommand
            });
            DataCount = DbRequest;
            return Run<T>(checkInfo, db, erolt);
        }
        public static List<T> Run<T>(this SqlStrModel sqlStrModel, MSSqlDBContext db)
        {
            DbContextSqlQueryCommand dbContextSqlQueryCommand = new DbContextSqlQueryCommand();
            dbContextSqlQueryCommand.QueryType = "text";
            dbContextSqlQueryCommand.Query = sqlStrModel.SqlStr;
            return SelectDb<T>(dbContextSqlQueryCommand,db);
        }
        public static T Run<T>(this GetByIdModel sqlStrModel, MSSqlDBContext db)
        {
            DbContextSqlQueryCommand dbContextSqlQueryCommand = new DbContextSqlQueryCommand();
            dbContextSqlQueryCommand.QueryType = "text";
            dbContextSqlQueryCommand.Query = sqlStrModel.IdStr;
            return SelectDb<T>(dbContextSqlQueryCommand, db).FirstOrDefault();
            //string sql = string.Empty;
        }
        #endregion

        public static List<T> SelectDb<T>(DbContextSqlQueryCommand dbContextSqlQueryCommand, MSSqlDBContext db)
        {
            var DbRequest = DbContextExtensions.Query<T>(db, new DbContextSqlQueryCommands
            {
                Sql = dbContextSqlQueryCommand
            }).ToList();
            //string sql = string.Empty;
            return DbRequest;
        }
    }

}
