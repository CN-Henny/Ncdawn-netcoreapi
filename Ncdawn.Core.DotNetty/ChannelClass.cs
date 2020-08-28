using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.Core.DotNetty
{
    public static class UserInfo
    {
        public static Dictionary<string, IChannel> userInfoDic = new Dictionary<string, IChannel>();
    }
}
