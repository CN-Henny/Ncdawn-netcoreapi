using System.Net;

namespace Ncdawn.CommonService
{
     public abstract class ResponseHelper
     {
        public static object IsSuccess_Msg_Data_HttpCode(string Message, dynamic Data = null, int DataCount = 0, HttpStatusCode Code = HttpStatusCode.OK)
        {
            return new {Code = Code, Data = Data, DataCount = DataCount,Message = Message};
        }

        public static object Error_Msg_Ecode_Elevel_HttpCode(string Message, dynamic Data = null, int DataCount = 0, HttpStatusCode Code = HttpStatusCode.InternalServerError)
        {
            return new { Code = Code, Data = Data, DataCount = DataCount, Message = Message };
        }
     }
}