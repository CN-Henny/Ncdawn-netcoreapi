using Ncdawn.feignclient.Model.ModelsVm;
using System.Collections.Generic;
using Ncdawn.CommonService.Class.Models;

namespace Ncdawn.Service.Account
{
    public interface ISysAreaService : IBaseService
    {

        /// <summary>
        /// insert
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        bool Create(SysAreaPost sysAreaPost, ref string ErrorMsg);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="deletePost">deletePost</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        bool DeleteByList(DeletePost deletePost, ref string ErrorMsg);
        /// <summary>
        /// update
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        bool Update(SysAreaPost sysAreaPost, ref string ErrorMsg);
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="sysAreaQuery">Result</param>
        /// <param name="DataCount">DataCount</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        List<SysAreaVm> Select(SysAreaQuery sysAreaQuery, ref int DataCount, ref string ErrorMsg);
        /// <summary>
        /// SelectJson
        /// </summary>
        /// <param name="sysAreaQuery">Result</param>
        /// <param name="DataCount">DataCount</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        string SelectJson(SysAreaQuery sysAreaQuery, ref int DataCount, ref string ErrorMsg);
        /// <summary>
        /// SelectOne
        /// </summary>
        /// <param name="Id">Primary key</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        SysAreaVm SelectOne(string Id, ref string ErrorMsg);

    }

}
