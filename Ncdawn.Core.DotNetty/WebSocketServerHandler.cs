using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Common.Utilities;
using DotNetty.Codecs.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using static DotNetty.Codecs.Http.HttpVersion;
using static DotNetty.Codecs.Http.HttpResponseStatus;
using DotNetty.Codecs.Http.WebSockets;
using DotNetty.Transport.Channels.Groups;
using DotNetty.Common.Concurrency;
using DotNetty.Common;
using System.Collections.Generic;
using System.Linq;
namespace Ncdawn.Core.DotNetty
{
    /// <summary>
    /// 服务端处理事件函数
    /// </summary>
    public class WebSocketServerHandler : SimpleChannelInboundHandler<Object> // ChannelHandlerAdapter 业务继承基类适配器 // (1)
    //public class DiscardServerHandler : ChannelHandlerAdapter // ChannelHandlerAdapter 业务继承基类适配器 // (1)
    {
        static Dictionary<string, IChannel> AppUsers = new Dictionary<string, IChannel>();

        static Dictionary<string, IChannel> WebUsers = new Dictionary<string, IChannel>();

        const string WebsocketPath = "/websocket";

        private readonly RedisHelper redisHelper;

        WebSocketServerHandshaker handshaker;

        //public DiscardServerHandler()
        //{
        //    redisHelper = new RedisHelper();
        //}

        static int JsonLen() => Encoding.UTF8.GetBytes("13").Length;

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}上线.");

            UserInfo.userInfoDic.Add(DateTime.Now.ToLongTimeString(), context.Channel);

            base.HandlerAdded(context);
        }
        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}下线.");
            try
            {
                #region 删除用户管道
                bool flag = true;
                //删除用户管道
                foreach (var item in AppUsers)
                {
                    if (item.Value == context.Channel)
                    {
                        AppUsers.Remove(item.Key);
                        flag = false;
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                if (flag)
                {
                    foreach (var item in WebUsers)
                    {
                        if (item.Value == context.Channel)
                        {
                            AppUsers.Remove(item.Key);
                            flag = false;
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }
                }
                #endregion
            }
            catch
            {

            }
            base.HandlerRemoved(context);
        }

        protected override void ChannelRead0(IChannelHandlerContext context, object message) // (2)
        {
            if (message is IFullHttpRequest request)
            {
                this.HandleHttpRequest(context, request);
            }
            else if (message is WebSocketFrame frame)
            {
                this.HandleWebSocketFrame(context, frame);
            }
        }
        #region 管道开始读
        /// <summary>
        /// 管道开始读
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        public override void ChannelRead(IChannelHandlerContext context, object message) // (2)
        {


            #region 丢弃
            //try
            //{
            //     string asdsad;
            //}
            //finally
            //{
            //    ReferenceCountUtil.Release(message);
            //}
            #endregion

            #region  打印数据
            //var byteBuffer = message as IByteBuffer;
            //try
            //{
            //    if (byteBuffer != null)
            //    {
            //        Console.WriteLine("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
            //        Console.WriteLine("Received from server1: " + byteBuffer.ReadByte());
            //    }
            //}
            //finally
            //{
            //    byteBuffer.Release();
            //}
            #endregion

            #region  应答服务
            //var byteBuffer = message as IByteBuffer;
            //try
            //{
            //    if (byteBuffer != null)
            //    {
            //        Console.WriteLine("Received from Client: " + byteBuffer.ToString(Encoding.UTF8));
            //        Console.WriteLine("Received from ClientByte: " + byteBuffer.ReadByte());

            //    }
            //}
            //finally
            //{
            //}
            //IByteBuffer initialMessage = Unpooled.Buffer(256);
            //initialMessage.WriteBytes(Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ?? throw new InvalidOperationException())); // (2)
            //context.WriteAsync(initialMessage); // (4)
            #endregion

            #region WebSocket
            if (message is IFullHttpRequest request)
            {
                this.HandleHttpRequest(context, request);
            }
            else if (message is WebSocketFrame frame)
            {
                this.HandleWebSocketFrame(context, frame);
            }
            #endregion


        }
        #endregion

        #region 出现异常
        /// <summary>
        /// 出现异常
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
        #endregion

        #region 管道读取完成
        /// <summary>
        /// 管道读取完成
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush(); // (5)
        #endregion                                                                                        //public override void ChannelReadComplete(IChannelHandlerContext context) // (5)

        //{
        //    #region  应答服务
        //    try
        //    {
        //        Console.WriteLine("Received from server: ");
        //        context.WriteAsync("收到了");
        //        context.Flush();
        //    }
        //    finally
        //    {
        //    }
        //    #endregion
        //}

        void HandleHttpRequest(IChannelHandlerContext ctx, IFullHttpRequest req)
        {
            // Handle a bad request.
            if (!req.Result.IsSuccess)
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, BadRequest));
                return;
            }

            // Allow only GET methods.
            if (!Equals(req.Method, HttpMethod.Get))
            {
                SendHttpResponse(ctx, req, new DefaultFullHttpResponse(Http11, Forbidden));
                return;
            }

            // Send the demo page and favicon.ico
            if ("/".Equals(req.Uri))
            {
                IByteBuffer content = WebSocketServerBenchmarkPage.GetContent(GetWebSocketLocation(req));
                var res = new DefaultFullHttpResponse(Http11, OK, content);

                res.Headers.Set(HttpHeaderNames.ContentType, "text/html; charset=UTF-8");
                HttpUtil.SetContentLength(res, content.ReadableBytes);

                SendHttpResponse(ctx, req, res);
                return;
            }
            if ("/favicon.ico".Equals(req.Uri))
            {
                var res = new DefaultFullHttpResponse(Http11, NotFound);
                SendHttpResponse(ctx, req, res);
                return;
            }

            // Handshake
            var wsFactory = new WebSocketServerHandshakerFactory(
                GetWebSocketLocation(req), null, true, 5 * 1024 * 1024);
            this.handshaker = wsFactory.NewHandshaker(req);
            if (this.handshaker == null)
            {
                WebSocketServerHandshakerFactory.SendUnsupportedVersionResponse(ctx.Channel);
            }
            else
            {
                this.handshaker.HandshakeAsync(ctx.Channel, req);
            }
        }

        void HandleWebSocketFrame(IChannelHandlerContext ctx, WebSocketFrame frame)
        {
            // Check for closing frame
            if (frame is CloseWebSocketFrame)
            {
                this.handshaker.CloseAsync(ctx.Channel, (CloseWebSocketFrame)frame.Retain());
                return;
            }

            if (frame is PingWebSocketFrame)
            {
                ctx.WriteAsync(new PongWebSocketFrame((IByteBuffer)frame.Content.Retain()));
                return;
            }

            if (frame is TextWebSocketFrame)
            {
                // Echo the frame
                //String requestmsg = ((TextWebSocketFrame)frame).Text();



                return;
            }

            if (frame is BinaryWebSocketFrame)
            {
                // Echo the frame
                ctx.WriteAsync(frame.Retain());
            }
        }

        static void SendHttpResponse(IChannelHandlerContext ctx, IFullHttpRequest req, IFullHttpResponse res)
        {
            // Generate an error page if response getStatus code is not OK (200).
            if (res.Status.Code != 200)
            {
                IByteBuffer buf = Unpooled.CopiedBuffer(Encoding.UTF8.GetBytes(res.Status.ToString()));
                res.Content.WriteBytes(buf);
                buf.Release();
                HttpUtil.SetContentLength(res, res.Content.ReadableBytes);
            }

            // Send the response and close the connection if necessary.
            Task task = ctx.Channel.WriteAndFlushAsync(res);
            if (!HttpUtil.IsKeepAlive(req) || res.Status.Code != 200)
            {
                task.ContinueWith((t, c) => ((IChannelHandlerContext)c).CloseAsync(),
                    ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        static string GetWebSocketLocation(IFullHttpRequest req)
        {
            bool result = req.Headers.TryGet(HttpHeaderNames.Host, out ICharSequence value);
            string location = value.ToString() + WebsocketPath;

            if (ServerSettings.IsSsl)
            {
                return "wss://" + location;
            }
            else
            {
                return "ws://" + location;
            }
        }

        //public static string DecodeClientData(byte[] recBytes, int length)
        //{
        //    if (length < 2)
        //    {
        //        return string.Empty;
        //    }

        //    bool fin = (recBytes[0] & 0x80) == 0x80; //0x80 = 1000,0000  第1bit = 1表示最后一帧  
        //    if (!fin)
        //    {
        //        if (recBytes[1] == 0xff)
        //        {
        //        }
        //        else
        //            return string.Empty;
        //    }

        //    bool mask_flag = (recBytes[1] & 0x80) == 0x80; // 是否包含掩码  
        //    if (!mask_flag)
        //    {
        //        return string.Empty;// 不包含掩码的暂不处理
        //    }

        //    int payload_len = recBytes[1] & 0x7F; // 数据长度  

        //    byte[] masks = new byte[4];
        //    byte[] payload_data;

        //    if (payload_len == 126)
        //    {
        //        Array.Copy(recBytes, 4, masks, 0, 4);
        //        payload_len = (UInt16)(recBytes[2] << 8 | recBytes[3]);
        //        payload_data = new byte[payload_len];
        //        Array.Copy(recBytes, 8, payload_data, 0, payload_len);

        //    }
        //    else if (payload_len == 127)
        //    {
        //        Array.Copy(recBytes, 10, masks, 0, 4);
        //        byte[] uInt64Bytes = new byte[8];
        //        for (int i = 0; i < 8; i++)
        //        {
        //            uInt64Bytes[i] = recBytes[9 - i];
        //        }
        //        UInt64 len = BitConverter.ToUInt64(uInt64Bytes, 0);

        //        payload_data = new byte[len];
        //        for (UInt64 i = 0; i < len; i++)
        //        {
        //            payload_data[i] = recBytes[i + 14];
        //        }
        //    }
        //    else
        //    {
        //        Array.Copy(recBytes, 2, masks, 0, 4);
        //        payload_data = new byte[payload_len];
        //        Array.Copy(recBytes, 6, payload_data, 0, payload_len);

        //    }

        //    for (var i = 0; i < payload_len; i++)
        //    {
        //        payload_data[i] = (byte)(payload_data[i] ^ masks[i % 4]);
        //    }
        //    //var uuu = new byte[payload_data.Length * 3 / 4];
        //    //for (int i = 0; i < uuu.Length; i++)
        //    //{
        //    //    uuu[i] = payload_data[i];
        //    //}
        //    //Console.WriteLine("UUUUUU：" + Encoding.UTF8.GetString(uuu));
        //    return Encoding.UTF8.GetString(payload_data);
        //}

    }
}