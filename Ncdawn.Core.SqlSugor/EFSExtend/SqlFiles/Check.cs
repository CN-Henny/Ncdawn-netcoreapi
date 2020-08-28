using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ncdawn.Core.SqlSugor
{
    [ConfigIOAttr("Check")]
    public class Check<T>
    {
        public Check(SelectInfo<T> selectInfo, List<ErrorList> erolt, CheckInfo checkInfo)
        {
            var methods = GetType().GetMethods().Where(t => t.GetCustomAttribute<ConfigMethodAttr>() != null);
            foreach (MethodInfo method in methods)
            {
                ErrorList errorList = new ErrorList();
                string configName = method.GetCustomAttribute<ConfigMethodAttr>().Name.ToUpper();
                Errorinfo errorinfo = method.Invoke(this, new object[] { selectInfo, checkInfo }) as Errorinfo;
                errorList.Name = configName;
                errorList.Errorinfo = errorinfo;
                if (!errorinfo.Calibration || !string.IsNullOrEmpty(errorinfo.Warning))
                {
                    erolt.Add(errorList);
                }
            }
        }
        [ConfigMethodAttr("tableName")]
        public Errorinfo Check01(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            if (selectInfo.tableName == null)
            {
                errorinfo.Calibration = false;
                errorinfo.Error = "tableName is not null";
            }
            if (errorinfo.Calibration)
            {
                checkInfo.tableName = selectInfo.tableName;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("tableType")]
        public Errorinfo Check02(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            if (selectInfo.tableType == -1)
            {
                errorinfo.Warning = "tableType is null,default is changed to Table";
                checkInfo.tableType = TableType.Table;
            }
            if (errorinfo.Calibration && string.IsNullOrEmpty(errorinfo.Warning))
            {
                checkInfo.tableType = selectInfo.tableType;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("strWhere")]
        public Errorinfo Check03(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            if (string.IsNullOrEmpty(selectInfo.strWhere))
            {
                errorinfo.Warning = "strWhere is null";
                checkInfo.strWhere = "";
            }
            else
            {
                checkInfo.strWhere = selectInfo.strWhere;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("pageNum")]
        public Errorinfo Check04(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            if (selectInfo.pageNum == -1)
            {
                errorinfo.Warning = "pageNum is null,default is changed to 1";
                checkInfo.pageNum = 1;
            }
            if (errorinfo.Calibration && string.IsNullOrEmpty(errorinfo.Warning))
            {
                checkInfo.pageNum = selectInfo.pageNum;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("recordNum")]
        public Errorinfo Check05(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            if (selectInfo.recordNum == -1)
            {
                errorinfo.Warning = "recordNum is null,default is changed to 10";
                checkInfo.recordNum = 10;
            }
            if (errorinfo.Calibration && string.IsNullOrEmpty(errorinfo.Warning))
            {
                checkInfo.recordNum = selectInfo.recordNum;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("orderByStr")]
        public Errorinfo Check06(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            string orderByStr = string.Empty;
            if (selectInfo.orderByStr.ToList().Count == 0)
            {
                errorinfo.Warning = "orderByStr is null,default is changed to order by 1";
                checkInfo.orderByStr = "order by 1";
            }
            else
            {
                foreach (var item in selectInfo.orderByStr.ToList())
                {
                    orderByStr += "," + item.Value;
                }
                checkInfo.orderByStr = orderByStr;
            }
            return errorinfo;
        }
        [ConfigMethodAttr("jsonFlag")]
        public Errorinfo Check07(SelectInfo<T> selectInfo, CheckInfo checkInfo)
        {
            Errorinfo errorinfo = new Errorinfo();
            return errorinfo;
        }
    }
}
