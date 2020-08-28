using System.Collections.Generic;

namespace Ncdawn.Core.SqlSugor
{
    public class OrderByType
    {
        /// <summary>
        /// ASC
        /// </summary>
        public const string ASC = "ASC";
        /// <summary>
        ///  DESC
        /// </summary>
        public const string DESC = "DESC";
    }
    public class TableType
    {
        /// <summary>
        /// ASC
        /// </summary>
        public const int Table = 0;
        /// <summary>
        ///  DESC
        /// </summary>
        public const int View = 1;
    }
    public class DataDeleteFlag
    {
        /// <summary>
        /// Delete
        /// </summary>
        public const int IsDelete = 1;
        /// <summary>
        ///  UnDelete
        /// </summary>
        public const int IsNotDelete = 0;
    }
    public class SelectInfo<T>
    {
        /// <summary>
        /// 表名/视图名
        /// </summary>
        public string tableName { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public int tableType { get; set; } = -1;
        /// <summary>
        /// 条件查询
        /// </summary>
        public string strWhere { get; set; }
        /// <summary>
        /// 页
        /// </summary>
        public int pageNum { get; set; } = -1;
        /// <summary>
        /// 条
        /// </summary>
        public int recordNum { get; set; } = -1;
        /// <summary>
        /// 排序
        /// </summary>
        public Dictionary<int, string> orderByStr { get; set; }
        /// <summary>
        /// 是否转换成json
        /// </summary>
        public bool jsonFlag { get; set; }

        public SelectInfo(string tableName)
        {
            this.tableName = tableName;
            orderByStr = new Dictionary<int, string>();
        }
    }

    public class CheckInfo
    {

        /// <summary>
        /// 表名/视图名
        /// </summary>
        public string tableName { get; set; }
        /// <summary>
        /// 表类型
        /// </summary>
        public int tableType { get; set; }
        /// <summary>
        /// 条件查询
        /// </summary>
        public string strWhere { get; set; }
        /// <summary>
        /// 页
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 条
        /// </summary>
        public int recordNum { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string orderByStr { get; set; }

        /// <summary>
        /// 是否转换成json
        /// </summary>
        public bool jsonFlag { get; set; }
    }

    public class ErrorList
    {
        public string Name { get; set; }
        public Errorinfo Errorinfo { get; set; }
    }
    public class Errorinfo
    {
        public bool Calibration { get; set; } = true;
        public string Warning { get; set; }
        public string Error { get; set; }
    }
    public class SqlStrModel
    {
        public string SqlStr { get; set; }
    }
    public class GetByIdModel
    {
        public string IdStr { get; set; }
    }
}
