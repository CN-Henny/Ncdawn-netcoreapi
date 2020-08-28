using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.CommonService.Class.Models
{
    /// <summary>
    /// 通用查询
    /// </summary>
    public class BaseQueryModel
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public string QueryStr { get; set; }
        /// <summary>
        /// 模糊查询
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 模糊查询
        /// </summary>
        public int RecordNum { get; set; }
    }
    public class LineChart
    {
        public string name { get; set; }
        public string type { get; set; }
        public string stack { get; set; }

        public List<string> data { get; set; } = new List<string>();
    }
    public class PieChart
    {
        public string name { get; set; }
        public string value { get; set; }
    }
}
