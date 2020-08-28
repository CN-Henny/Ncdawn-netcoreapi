using System.ComponentModel;

namespace Ncdawn.feignclient.Model
{
    public class Response
    {
        protected static string _msg = "Successful!";
        private string _message;

        [Description("e.g. 200:success; 500:system error; 404:not found; 401:Unauthorized ")]
        public int Code
        {
            get;
            set;
        } = 200;

        [Description("response message")]
        public string Message
        {
            get
            {
                if (string.IsNullOrEmpty(_message))
                {
                    return _msg;
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public dynamic Data { get; set; }

        public virtual Response SetError(string errorMessage)
        {
            Code = 500;
            _message = errorMessage;
            return this;
        }
    }
}
