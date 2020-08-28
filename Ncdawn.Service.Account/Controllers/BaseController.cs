using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Ncdawn.feignclient.Model;

namespace Ncdawn.Service.Account.Controllers
{
    [EnableCors("any")]
    [ServiceFilter(typeof(SupportFilter))]
    public class BaseServiceController : Controller
    {
        protected string GetOperationId()
        {
            string OperationId = string.Empty;
            OperationId = Request.Headers["OperationId"].ToString();
            return OperationId;
        }
    }
}
