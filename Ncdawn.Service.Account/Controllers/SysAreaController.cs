using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pivotal.Discovery.Client;
using Ncdawn.CommonService;
using Ncdawn.feignclient.Model.ModelsVm;
using Ncdawn.EFS.Response;
using Ncdawn.CommonService.Class.Models;

namespace Ncdawn.Service.Account.Controllers
{
    [Route("api/[controller]")]
    public class SysAreaController : BaseServiceController
    {

        private readonly HttpClient httpClient;
        private readonly DiscoveryHttpClientHandler _handler;
        IHttpRpcService httpRpcService;
        private readonly ILogService logService;
        private readonly IConfiguration configuration;
        private IHostingEnvironment _hostingEnvironment;
        ISysAreaService sysAreaService;
        ISysAreaRepository sysAreaRepository;
        public SysAreaController(
            IDiscoveryClient _client,
            IHttpRpcService _HttpRpcService,
            IHostingEnvironment hostingEnvironment,
            ILogService _LogService,
            IConfiguration _IConfiguration,
            ISysAreaService _ISysAreaService,
            ISysAreaRepository _ISysAreaRepository
            )
        {
            httpRpcService = _HttpRpcService;
            _handler = new DiscoveryHttpClientHandler(_client);
            httpClient = new HttpClient(_handler, false);
            logService = _LogService;
            configuration = _IConfiguration;
            _hostingEnvironment = hostingEnvironment;
            sysAreaService = _ISysAreaService;
            sysAreaRepository = _ISysAreaRepository;
        }
        #region INSERT
        /// <summary>
        /// INSERT
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public IActionResult Create([FromBody] SysAreaPost sysAreaPost)
        {
            string ErrorMsg = string.Empty;
            sysAreaPost.CreateUserName = GetOperationId();
            sysAreaPost.ModificationUserName = GetOperationId();
            var flag = sysAreaService.Create(sysAreaPost, ref ErrorMsg);
            if (flag)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion
        #region DeleteByList
        /// <summary>
        /// DeleteByList
        /// </summary>
        /// <param name="deletePost">deletePost</param>
        /// <returns></returns>
        [HttpPost("DeleteByList")]
        public IActionResult DeleteByList([FromBody] DeletePost deletePost)
        {
            string ErrorMsg = string.Empty;
            var flag = sysAreaService.DeleteByList(deletePost, ref ErrorMsg);
            if (flag)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion
        #region Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="sysAreaPost">Model</param>
        /// <returns></returns>
        [HttpPost("Update")]
        public IActionResult Update([FromBody] SysAreaPost sysAreaPost)
        {
            string ErrorMsg = string.Empty;
            sysAreaPost.ModificationUserName = GetOperationId();
            var flag = sysAreaService.Update(sysAreaPost, ref ErrorMsg);
            if (flag)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion
        #region SELECT
        /// <summary>
        /// SELECT
        /// </summary>
        /// <param name="sysAreaQuery">sysAreaQuery</param>
        /// <returns></returns>
        [HttpPost("Select")]
        public IActionResult Select([FromBody] SysAreaQuery sysAreaQuery)
        {
            string ErrorMsg = string.Empty;
            int DataCount = 0;
            var flag = sysAreaService.Select(sysAreaQuery, ref DataCount, ref ErrorMsg);
            if (flag != null)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, DataCount)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion
        #region SELECTJSON
        /// <summary>
        /// SELECTJSON
        /// </summary>
        /// <param name="sysAreaQuery">sysAreaQuery</param>
        /// <returns></returns>
        [HttpPost("SelectJson")]
        public IActionResult SelectJson([FromBody] SysAreaQuery sysAreaQuery)
        {
            string ErrorMsg = string.Empty;
            int DataCount = 0;
            var flag = sysAreaService.SelectJson(sysAreaQuery, ref DataCount, ref ErrorMsg);
            if (flag != null)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag, DataCount)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion
        #region SelectOne
        /// <summary>
        /// SelectOne
        /// </summary>
        /// <param name="Id">Primary key</param>
        /// <returns></returns>
        [HttpGet("SelectOne")]
        public IActionResult SelectOne(string Id)
        {
            string ErrorMsg = string.Empty;
            var flag = sysAreaService.SelectOne(Id, ref ErrorMsg);
            if (flag != null)
            {
                return new JsonResult(
                    ResponseHelper.IsSuccess_Msg_Data_HttpCode(ErrorMsg, flag)
                    );
            }
            else
            {
                return new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(ErrorMsg, flag)
                    );
            }

}
        #endregion

}

}
