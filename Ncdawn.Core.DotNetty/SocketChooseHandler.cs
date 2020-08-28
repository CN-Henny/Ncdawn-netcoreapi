using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace Ncdawn.Core.DotNetty
{
    public class SocketChooseHandler : SimpleChannelInboundHandler<Object>
    {
        private static String WEBSOCKET_PREFIX = "GET /";
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            //解析报文开头
            var byteBuffer = message as IByteBuffer;
            string StrMsg = byteBuffer.ToString(Encoding.UTF8);
            context.Channel.Pipeline.Remove(this);
            if (StrMsg.StartsWith(WEBSOCKET_PREFIX))
            {
                //ws
                context.Channel.Pipeline.Remove("TCPHandler");
            }
            else
            {
                //tcp
                context.Channel.Pipeline.Remove("WebSocketHandler");
            }
            //解码
            context.FireChannelRead(message);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, object msg)
        {
            throw new NotImplementedException();
        }
    }

}
