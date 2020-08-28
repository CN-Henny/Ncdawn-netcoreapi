using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.CommonService.Class.Models
{
    #region 删除实体
    public class DeletePost
    {
        public List<string> ObjectIds { get; set; }
        public string UserId { get; set; }
        public override string ToString()
        {
            var str = "";
            foreach (var item in ObjectIds)
            {
                str += "," + item;
            }
            return "UserId:" + UserId + ",ObjectIds" + str;
        }
    }
    #endregion
}
