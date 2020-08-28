using DotNetty.Codecs.Http.WebSockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ncdawn.Core.DotNetty
{
    public class SendMessage
    {
        public void SendWebsocketMessage(string msg)
        {
            foreach(var item in UserInfo.userInfoDic)
            {
                item.Value.WriteAndFlushAsync(new TextWebSocketFrame(msg));
            }
        }
    }


}
