using System;

namespace Ncdawn.CommonService.Class.Common
{
    public class CustomException : ApplicationException
    {
        private string error;
        public CustomException()
        {

        }

        public CustomException(string msg) : base(msg)
        {
            this.error = msg;
        }
    }
}
