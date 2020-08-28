namespace Ncdawn.Core.SqlSugor
{
    public partial class TsqlFactory
    {
        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="checkInfo"></param>
        /// <returns></returns>
        public static DbContextSqlQueryCommand Select(CheckInfo checkInfo)
        {

            string TsqlStr = string.Empty;
            //select * from PaasOLT_OrderCarClean order by Id offset((2 - 1) * 20) rows fetch next 20 rows only;
            //查询
            TsqlStr = "select * from " + checkInfo.tableName + " ";
            if (checkInfo.tableType == 0)
            {
                TsqlStr = "select *,(select TrueName from SysUser where ObjectId = " + checkInfo.tableName + ".CreateUserName) as CreateUserNameStr,(select TrueName from SysUser where ObjectId = " + checkInfo.tableName + ".ModificationUserName) as ModificationUserNameStr from " + checkInfo.tableName + " ";
            }
            //条件
            string strWhere = string.IsNullOrEmpty(checkInfo.strWhere) ? null : " where " + checkInfo.strWhere;
            //排序
            string orderByStr = " order by " + checkInfo.orderByStr.Substring(1);
            //分页和排序
            string pageRecordStr = string.Empty;
            if (checkInfo.pageNum != 0)
            {
                pageRecordStr = " offset((" + checkInfo.pageNum + " - 1) * " + checkInfo.recordNum + ") rows fetch next " + checkInfo.recordNum + " rows only ";
            }
            //序列化json
            string forJsonStr = checkInfo.jsonFlag ? " for json path,INCLUDE_NULL_VALUES " : string.Empty;
            //创建返回
            DbContextSqlQueryCommand dbContextSqlQueryCommand = new DbContextSqlQueryCommand();
            dbContextSqlQueryCommand.QueryType = "text";
            //返回
            dbContextSqlQueryCommand.Query = TsqlStr + strWhere + orderByStr + pageRecordStr + forJsonStr;
            return dbContextSqlQueryCommand;
        }
        #endregion
    }

}
