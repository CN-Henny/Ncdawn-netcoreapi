using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Ncdawn.EFS.Models;
using Ncdawn.EFS.Response;
using Ncdawn.feignclient.Model.ModelsVm;
using Ncdawn.Core.Common;
using System.Linq.Expressions;
using Ncdawn.CommonService.Class.Models;
using Ncdawn.EFS.DBContext;
using Ncdawn.Core.SqlSugor;

namespace Ncdawn.Service.Account
{
    public class SysAreaService : IBaseService, ISysAreaService
    {
        public MSSqlDBContext db;
        IHttpRpcService httpRpcService;
        IConfiguration configuration;
        private readonly ILogService logService;
        private readonly IQRCodeService qRCodeService;
        ISysAreaRepository sysAreaRepository;
        public SysAreaService(
            IHttpRpcService _HttpRpcService,
            IQRCodeService _IQRCodeService,
            ILogService _LogService,
            IConfiguration _IConfiguration,
            ISysAreaRepository _ISysAreaRepository
            )
        {
            httpRpcService = _HttpRpcService;
            qRCodeService = _IQRCodeService;
            logService = _LogService;
            configuration = _IConfiguration;
            sysAreaRepository = _ISysAreaRepository;
            db = sysAreaRepository.GetDb();
        }
        public static List<ErrorList> erolt = new List<ErrorList>();
        #region insert
        /// <summary>
        /// insert
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        public bool Create(SysAreaPost sysAreaPost, ref string ErrorMsg)
        {
            try
            {
                SysArea sysArea = new SysArea();
                LinqHelper.ModelTrans(sysAreaPost, sysArea);
                sysArea.ObjectId = ResultHelper.NewId;
                sysArea.CreateTime = ResultHelper.NowTime;
                sysArea.ModificationTime = ResultHelper.NowTime;
                SysArea sysArea1 = new SysArea();
                sysArea1.ObjectId = "202008131744238122763d1e7363908";
                LinqHelper.ModelTrans(sysAreaPost, sysArea1);
                sysArea1.CreateTime = ResultHelper.NowTime;
                sysArea1.ModificationTime = ResultHelper.NowTime;
                SysArea sysArea2 = new SysArea();
                LinqHelper.ModelTrans(sysAreaPost, sysArea2);
                sysArea2.ObjectId = ResultHelper.NewId;
                sysArea2.CreateTime = ResultHelper.NowTime;
                sysArea2.ModificationTime = ResultHelper.NowTime;
                sysAreaRepository.CreateTransaction(sysArea);
                sysAreaRepository.CreateTransaction(sysArea1);
                sysAreaRepository.CreateTransaction(sysArea2);
                sysAreaRepository.TransactionCommit();
                return true;
            }
            catch (Exception ex)
            {
                ErrorMsg = "Create Error";
                return false;
            }

        }
        #endregion
        #region deletebylist
        /// <summary>
        /// deletebylist
        /// </summary>
        /// <param name="deletePost">deletePost</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        public bool DeleteByList(DeletePost deletePost, ref string ErrorMsg)
        {
            try
            {
                if (sysAreaRepository.DeleteNotTrue(deletePost.ObjectIds))
                {
                    ErrorMsg = "Edit Success";
                    return true;
                }
                else
                {
                    ErrorMsg = "Edit Faild";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Edit Error";
                return false;
            }

        }
        #endregion
        #region update
        /// <summary>
        /// update
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        public bool Update(SysAreaPost sysAreaPost, ref string ErrorMsg)
        {
            try
            {
                SysArea sysArea = new SysArea();
                LinqHelper.ModelTrans(sysAreaPost, sysArea);
                sysArea.ModificationTime = ResultHelper.NowTime;
                if (sysAreaRepository.EditNotNull(sysArea))
                {
                    ErrorMsg = "Update Success";
                    return true;
                }
                else
                {
                    ErrorMsg = "Update Faild";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = "Update Error";
                return false;
            }

        }
        #endregion
        #region Select
        /// <summary>
        /// Select
        /// </summary>
        /// <param name="sysAreaQuery"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public List<SysAreaVm> Select(SysAreaQuery sysAreaQuery, ref int DataCount, ref string ErrorMsg)
        {
            try
            {
                SelectInfo<SysArea> selectInfo = new SelectInfo<SysArea>("SysArea");

                Expression<Func<SysArea, bool>> predicate = null;

                var DbRequest = selectInfo
                    .SetWhere(predicate, DataBaseType.SqlServer, DataDeleteFlag.IsNotDelete)
                    .SetType(TableType.Table)
                    .OrderBy(EF => EF.CreateTime)
                    .OrderByDescending(EF => EF.ObjectId)
                    .Check(ref erolt)
                    .Run<SysAreaVm>(db, erolt);

                //selectInfo
                //    .GetById("123")
                //    .Run();

                //var DbRequest = selectInfo.SetSqlStr("select * from SysArea").Run<SysAreaVm>();

                //var DbRequest = DbContextExtensions.Query<SysAreaVm>(new MSSqlDBContext(), new DbContextSqlQueryCommands
                //{
                //    Sql = sql
                //}).ToList();
                //ErrorMsg = "Select Success";
                return DbRequest;
            }
            catch (Exception ex)
            {
                ErrorMsg = "Select Error";
                return null;
            }

        }
        #endregion
        #region SelectJson
        /// <summary>
        /// SelectJson
        /// </summary>
        /// <param name="sysAreaQuery"></param>
        /// <param name="DataCount"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public string SelectJson(SysAreaQuery sysAreaQuery, ref int DataCount, ref string ErrorMsg)
        {
            return null;

        }
        #endregion
        #region SelectOne
        /// <summary>
        /// SelectOne
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ErrorMsg">Result</param>
        /// <returns></returns>
        public SysAreaVm SelectOne(string Id, ref string ErrorMsg)
        {
            SelectInfo<SysArea> selectInfo = new SelectInfo<SysArea>("SysArea");
            var DbRequest = selectInfo
                .GetById("123")
                .Run<SysAreaVm>(sysAreaRepository.GetDb());
            return DbRequest;
        }
        #endregion

    }

}
