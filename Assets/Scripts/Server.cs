using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

public class Server : MonoBehaviour {
    /// <summary>
    /// Socket
    /// </summary>
    Socket server;
    /// <summary>
    /// 端口
    /// </summary>
    public int port = 18001;
    /// <summary>
    /// 客户端引用列表
    /// </summary>
    List<SocketState> clients;
    /// <summary>
    /// 监视线程
    /// </summary>
    Thread tempThread;
    /// <summary>
    /// 是否开启线程
    /// </summary>
    bool isRuningThread = true;


    private void Start()
    {
        Initial();
    }

    private void Update()
    {
        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].Recv();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initial()
    {
        //新建Socket
        server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //绑定ip
        IPEndPoint ip = new IPEndPoint(IPAddress.Any,port);
        server.Bind(ip);
        //服务器连接后面最多排队个数
        server.Listen(100);
        tempThread = new Thread(recvThread);
        tempThread.Start();
        clients = new List<SocketState>();
    }

    /// <summary>
    /// 线程监视
    /// </summary>
    public void recvThread()
    {
        while (isRuningThread)
        {
            //接收
            server.BeginAccept(AcceptCallBack,null);
        }
    }

    /// <summary>
    /// 监视后处理
    /// </summary>
    /// <param name="ar"></param>
    public void AcceptCallBack(IAsyncResult ar)
    {
        //接收到原客户端
        Socket client = server.EndAccept(ar);
        //引用,调用后具有SocketState方法和属性
        SocketState tempState = new SocketState(client);
        //填入list列表
        clients.Add(tempState);
    }

    /// <summary>
    /// 退出应用时系统自动调用
    /// </summary>
    public void OnApplicationQuit()
    {
        isRuningThread = false;
        tempThread.Abort();
        for (int i=0;i<clients.Count;i++)
        {
            //关闭每个客户端的socket
            clients[i].Dispose();
        }
    }
}
