using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class SocketState{
    /// <summary>
    /// Socket
    /// </summary>
    Socket client;
    /// <summary>
    /// 是否是服务器
    /// </summary>
    public bool isServer = true;
    /// <summary>
    /// 接收的数据
    /// </summary>
    byte[] recvBytes;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="tempClient"></param>
    public SocketState(Socket tempClient)
    {
        this.client = tempClient;
    }
    /// <summary>
    /// 初始化，传入Socket
    /// </summary>
    /// <param name="tempClient">Socket</param>
    /// <param name="isServer">是否是服务器</param>
    public SocketState(Socket tempClient,bool isServer)
    {
        this.client = tempClient;
        this.isServer = isServer;
    }


    //连接Connect
    #region 
    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="ip">IP</param>
    /// <param name="port">端口</param>
    public void Connect(string ip,int port)
    {
        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip),port);
        client.BeginConnect(ipPoint, ConnectCallBack, null);
    }
    /// <summary>
    /// 连接后回调
    /// </summary>
    /// <param name="ar"></param>
    public void ConnectCallBack(IAsyncResult ar)
    {
        client.EndAccept(ar);
    }
    #endregion


    //发送
    #region
    /// <summary>
    /// 发送数据
    /// </summary>
    /// <param name="buffer">二进制数据</param>
    public void Send(byte[] buffer)
    {
        client.BeginSend(buffer,0,buffer.Length,SocketFlags.None,SendCallBack,null);
    }
    /// <summary>
    /// 发送数据回调
    /// </summary>
    /// <param name="ar"></param>
    public void SendCallBack(IAsyncResult ar)
    {
        //发送个数
        int sendCount = client.EndSend(ar);
    }
    #endregion


    //接收Receive
    #region
    public void Recv()
    {
        recvBytes = new byte[1024];
        client.BeginReceive(recvBytes,0,recvBytes.Length,SocketFlags.None,RecvCallBack,null);
    }

    public void RecvCallBack(IAsyncResult ar)
    {
        //接收的数据的长度
        int recvCount = client.EndReceive(ar);
        //临时的数据存储
        byte[] tempRecvBytes = new byte[recvCount];
        Buffer.BlockCopy(recvBytes,0,tempRecvBytes,0,recvCount);
        string recvString = System.Text.Encoding.Default.GetString(recvBytes);
        if (isServer)
        {
            //发回去
            string mySenderStr = "server  sender ==" + recvString;
            Debug.Log("====== ==" + mySenderStr);
            byte[] tmpSender = System.Text.Encoding.Default.GetBytes(mySenderStr);
            Send(tmpSender);
        }
        else
        {
            Debug.Log("client  recv str ==" + recvString);
        }
    }
    #endregion

    public void Dispose()
    {
        client.Close();
    }
}

