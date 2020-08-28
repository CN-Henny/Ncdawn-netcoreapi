using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

//调用例子
//var list = DbContextExtensions.Query<T>(new DbContextSqlQueryCommands
//{
//    Sql = new DbContextSqlQueryCommand(JsonFilePath,SQLID,new SqlParameter(......)
//});
namespace Ncdawn.Core.SqlSugor
{

    /// <summary>
    /// 数据库查询语句
    /// </summary>
    public class DbContextSqlQueryCommand
    {

        /// <summary>
        /// 查询语句
        /// </summary>
        public string Query { get; set; }
        public string QueryType { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object Parameters { get; set; }

        //public DbContextSqlQueryCommand(string SQLID, object @params = null, string JsonFilePath = "SqlFiles/SystemSqlStr.json")
        //{
        //    IConfiguration configuration = new ConfigurationBuilder()
        //      .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(JsonFilePath, optional: true, reloadOnChange: true)  //指定加载的配置文件
        //      .Build();
        //    Query = configuration.GetSection(SQLID)["sql"];
        //    QueryType = configuration.GetSection(SQLID)["type"];
        //    Parameters = @params;
        //}
    }

    /// <summary>
    /// 数据库查询语句集合
    /// </summary>
    public class DbContextSqlQueryCommands
    {
        /// <summary>
        /// 数据库为SqlServer时使用的查询语句
        /// </summary>
        public DbContextSqlQueryCommand Sql { get; set; }
        /// <summary>
        /// 数据库为MySql时使用的查询语句
        /// </summary>
        public DbContextSqlQueryCommand MySql { get; set; }
        /// <summary>
        /// 数据库为Sqlite时使用的查询语句
        /// </summary>
        public DbContextSqlQueryCommand Sqlite { get; set; }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbContextType
    {
        SqlServer = 1,
        MySql = 2,
        Sqlite = 3,
    }

    /// <summary>
    /// EF上下文扩展
    /// </summary>
    public static class DbContextExtensions
    {
        //拼接参数
        private static void combineParams(DbContextType type, ref DbCommand command, object @params = null)
        {
            if (@params != null)
            {
                Type paramType;
                switch (type)
                {
                    case DbContextType.SqlServer:
                        paramType = typeof(SqlParameter);
                        break;
                    case DbContextType.MySql:
                        paramType = typeof(MySqlParameter);
                        break;
                    case DbContextType.Sqlite:
                        paramType = typeof(SqliteParameter);
                        break;
                    default:
                        throw new Exception("未实现的数据库类型");
                }
                foreach (var param in @params as SqlParameter[])
                {
                    var paramItem = Activator.CreateInstance(paramType, $"{param.ParameterName}", (object)param.Value);
                    command.Parameters.Add(paramItem);
                }
            }
        }
        //创建命令（同时返回连接符）
        private static DbCommand createCommand(DbContext context, DbContextSqlQueryCommands commands, out DbConnection connection)
        {
            var conn = context.Database.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            if (commands.Sqlite != null && context.Database.IsSqlite())
            {
                cmd.CommandText = commands.Sqlite.Query;
                combineParams(DbContextType.Sqlite, ref cmd, commands.Sqlite.Parameters);
            }
            else if (commands.MySql != null && context.Database.IsMySql())
            {
                cmd.CommandText = commands.MySql.Query;
                combineParams(DbContextType.MySql, ref cmd, commands.MySql.Parameters);
            }
            else if (commands.Sql != null && context.Database.IsSqlServer())
            {
                cmd.CommandText = commands.Sql.Query;
                combineParams(DbContextType.SqlServer, ref cmd, commands.Sql.Parameters);
            }
            return cmd;
        }

        ///// <summary>
        ///// 执行sql语句，返回受影响行数
        ///// </summary>
        ///// <param name="context">EF上下文</param>
        ///// <param name="commands">数据库查询语句集合</param>
        ///// <returns>受影响行数</returns>
        //public static int Exec(this DbContext context, DbContextSqlQueryCommands commands)
        //{
        //    var command = createCommand(context, commands, out var conn);
        //    var rsl = command.ExecuteNonQuery();
        //    conn.Close();
        //    return rsl;
        //}

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>数据DataTable</returns>
        public static DataTable Query(this DbContext context, DbContextSqlQueryCommands commands)
        {
            var command = createCommand(context, commands, out var conn);
            var reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }
        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>数据DataTable</returns>
        public static string jsonQuery(this DbContext context, DbContextSqlQueryCommands commands)
        {
            string jsonReturn = string.Empty;
            var command = createCommand(context, commands, out var conn);
            var reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            for(var i=0;i<dt.Rows.Count;i++)
            {
                jsonReturn += (dt.Rows[i][0]).ToString();
            }
            return jsonReturn;
        }

        /// <summary>
        /// 查询数据库，返回多个查询结果集
        /// </summary>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>数据DataSet</returns>
        public static DataSet QuerySet(this DbContext context, DbContextSqlQueryCommands commands)
        {
            var dt = Query(context, commands);
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 查询数据库，返回IEnumerable的强类型数据
        /// </summary>
        /// <typeparam name="T">查询结果类型</typeparam>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>IEnumerable的强类型数据</returns>
        public static IEnumerable<T> Query<T>(this DbContext context, DbContextSqlQueryCommands commands)
        {

            var tb = Query(context, commands);
            return DataTableToList<T>(tb);
        }

        /// <summary>
        /// 查询数据库，返回第一条数据
        /// </summary>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>查询到的第一条数据或null</returns>
        public static DataRow QueryOne(this DbContext context, DbContextSqlQueryCommands commands)
        {
            var dt = Query(context, commands);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        /// <summary>
        /// 查询数据库，返回唯一数据
        /// </summary>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>查询到的唯一数据</returns>
        public static object QueryObject(this DbContext context, DbContextSqlQueryCommands commands)
        {
            var command = createCommand(context, commands, out var conn);
            var rsl = command.ExecuteScalar();
            conn.Close();
            return rsl;
        }

        /// <summary>
        /// 查询数据库，返回唯一强类型数据
        /// </summary>
        /// <typeparam name="T">查询结果类型</typeparam>
        /// <param name="context">EF上下文</param>
        /// <param name="commands">数据库查询语句集合</param>
        /// <returns>查询到的唯一强类型数据</returns>
        public static T QueryObject<T>(this DbContext context, DbContextSqlQueryCommands commands)
        {
            return (T)QueryObject(context, commands);
        }

        /// <summary>  
        /// 将Datatable转换为List集合  
        /// </summary>  
        /// <typeparam name="T">类型参数</typeparam>  
        /// <param name="dt">datatable表</param>  
        /// <returns></returns>  
        private static List<T> DataTableToList<T>(DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        //DbContext.Database.SqlQuery<GMGood>(sql.ToString(), sqlParameters).ToList();
    }

}
