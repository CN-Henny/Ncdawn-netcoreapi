public class IMMessage
{
    //应用ID
    public string appId { get; set; }

    //版本
    public string version { get; set; }

    //管道类型
    public string pipetype { get; set; }

    //用户ID
    public string uid { get; set; }

    //消息类型 0：登录 1：文字消息   退出
    public int msgType { get; set; }

    //接收方
    public string receiveId { get; set; }

    //消息内容
    public string msg { get; set; }

    //消息状态
    public string msgStatus { get; set; }

    //发送时间
    public string SendTime { get; set; }

    //时间戳标识
    public string appIduidTimeStamp { get; set; }

    public IMMessage()
    {

    }

    
   
    public string toString()
    {
        return "{" +
                "appId=" + appId +
                ", version=" + version +
                ", uid=" + uid +
                ", msgType=" + msgType +
                ", receiveId=" + receiveId +
                ", msg='" + msg + '\'' +
                ", msgStatus='" + msgStatus + '\'' +
                ", SendTime='" + SendTime + '\'' +
                ", appIduidTimeStamp='" + appIduidTimeStamp + '\'' +
                '}';
    }

    public string ToString()
    {
        return "{" +
                ", uid=" + uid +
                ", msgType=" + msgType +
                ", receiveId=" + receiveId +
                ", msg='" + msg + '\'' +
                ", SendTime='" + SendTime + '\'' +
                '}';
    }
}

public class user
{
    public string id { get; set; }

    public string name { get; set; }
}