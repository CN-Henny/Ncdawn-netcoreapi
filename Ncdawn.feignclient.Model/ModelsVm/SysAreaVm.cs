using System;
using System.Collections.Generic;
using System.Text;
using Ncdawn.CommonService.Class.Models;
using Ncdawn.EFS.Models;

namespace Ncdawn.feignclient.Model.ModelsVm
{
    public class SysAreaVm : SysAreaModel
    {

      public string ParentName { get; set; }

}
        /// <summary>
        /// 用户入参
        /// </summary>
        public class SysAreaQuery : BaseQueryModel
        {
        }
        /// <summary>
        /// 默认Post
        /// </summary>
        public class SysAreaPost : SysArea
        {
        }

}
