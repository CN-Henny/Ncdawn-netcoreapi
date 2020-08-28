using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.Core.DotNetty
{
    public static class ServerSettings
    {
        public static bool IsSsl
        {
            get
            {
                string ssl = "";
                return !string.IsNullOrEmpty(ssl) && bool.Parse(ssl);
            }
        }

        public static int Port => int.Parse("");

        public static bool UseLibuv
        {
            get
            {
                string libuv = "";
                return !string.IsNullOrEmpty(libuv) && bool.Parse(libuv);
            }
        }
    }
}
