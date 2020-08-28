namespace Ncdawn.Core.Common
{
    /// <summary>
    /// Easyui列头过滤属性
    /// </summary>
    public class DataFilterModel
    {
        public string field { set; get; }
        public string op { set; get; }
        public string value { set; get; }
    }

    /// <summary>
    /// 获取权限关系IDs前入参
    /// </summary>
    public class SysAuthorityInfo
    {
        public string userStructId { get; set; }
        public string userPosId { get; set; }
    }
}
