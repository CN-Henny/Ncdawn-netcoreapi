using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ncdawn.CommonService;
using Newtonsoft.Json;
using System.Text;

namespace Ncdawn.feignclient.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SupportFilter : IActionFilter
    {
        /// <summary>
        /// 白名单，逗号拼接
        /// </summary>
        public static string whiteList = "Login";
        /// <summary>
        /// 是否启用
        /// </summary>
        public static bool isEnable = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var arg in context.ActionArguments)
            {
                stringBuilder.Append($"{arg.Key} : {JsonConvert.SerializeObject(arg.Value)}");
            }
            string operationId = context.HttpContext.Request.Headers["operationId"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();
            var controllerName = context.RouteData.Values["controller"].ToString();
            if (isEnable && whiteList.IndexOf(actionName) == -1)
            {
                //判断OperationId
                if (string.IsNullOrEmpty(operationId))
                {
                    context.Result = new JsonResult(
                        ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode("无效用户")
                        );
                }
            }
            
            string Msg = "模型异常：";
            if (!context.ModelState.IsValid)
            {
                int i = 0;
                foreach (var item in context.ModelState)
                {
                    Msg += "字段：" + item.Key + ";错误：";
                    foreach (var itemValues in item.Value.Errors)
                    {
                        Msg += itemValues.ErrorMessage + "|";
                    }
                    i++;
                }
                context.Result = new JsonResult(
                    ResponseHelper.Error_Msg_Ecode_Elevel_HttpCode(Msg)
                    );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
