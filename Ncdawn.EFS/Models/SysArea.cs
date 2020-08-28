using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ncdawn.EFS.Models
{
    public class SysArea
    {

      /// <summary>
      /// 主键
      /// </summary>
      public string ObjectId { get; set; }
      /// <summary>
      /// 创建时间
      /// </summary>
      public DateTime CreateTime { get; set; }
      /// <summary>
      /// 更新时间
      /// </summary>
      public DateTime ModificationTime { get; set; }
      /// <summary>
      /// 创建人姓名
      /// </summary>
      public string CreateUserName { get; set; }
      /// <summary>
      /// 修改人姓名
      /// </summary>
      public string ModificationUserName { get; set; }
      /// <summary>
      /// 排序码
      /// </summary>
      public int Sort { get; set; }
      /// <summary>
      /// 关联数据Id
      /// </summary>
      public string Pid { get; set; }
      /// <summary>
      /// 是为港澳台()
      /// </summary>
      public string IsSpecial { get; set; }
      /// <summary>
      /// 是否为其他()
      /// </summary>
      public string IsOther { get; set; }

}
      /// <summary>
      /// Model映射实体
      /// </summary>
      public class SysAreaModel : SysArea
      {
        /// <summary>
        /// 创建时间Str
        /// </summary>
        public string CreateTimeStr { get; set; }
        /// <summary>
        /// 更新时间Str
        /// </summary>
        public string ModificationTimeStr { get; set; }
        /// <summary>
        /// 创建人姓名Str
        /// </summary>
        public string CreateUserNameStr { get; set; }
        /// <summary>
        /// 修改人姓名Str
        /// </summary>
        public string ModificationUserNameStr { get; set; }

}

}
