using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour {
    SocketState client;
    // Use this for initialization
    void Start () {
        Socket tempSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        client = new SocketState(tempSocket,false);
        client.Connect("127.0.0.1",18001);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            byte[] tmpSender = System.Text.Encoding.Default.GetBytes("123456");
            Debug.Log("client  begin sender");
            client.Send(tmpSender);
        }
        if (client!=null)
        {
            client.Recv();
        }
	}

    public void OnApplicationQuit()
    {
        client.Dispose();
    }
}
