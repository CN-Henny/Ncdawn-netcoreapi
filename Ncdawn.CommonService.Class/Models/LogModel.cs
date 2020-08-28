using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.CommonService.Class.Models
{
    public class LogModel
    {
        /// <summary>
        /// 操作人Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; set; }
        /// <summary>
        /// 项目名
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        public string moduleName { get; set; }
        /// <summary>
        /// 方法名(页面名)
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 日志结果
        /// </summary>
        public string result { get; set; }
    }
}
