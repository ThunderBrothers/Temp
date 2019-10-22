using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SocketConnect : MonoBehaviour
{

    public UdpClient udpClient;
    public UdpClient udpListen;
    public List<string> sendMessages = new List<string>();
    public List<string> receiveMessages = new List<string>();

    private Thread clinetThread;
    private bool clinetThreadIsRun = false;
    private Thread listenThread;
    private bool listenThreadIsRun = false;
    private int port = 8085;
    private string identifier = "identifier";

    private string appName = "Temp";
    private string currentPID = "localUnity";

    // Start is called before the first frame update
    void Start()
    {
        InitClient();
        StartListen();
        GetIdentification();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetMouseButtonDown(0))
        {
            string posTra = identifier + "$" + currentPID + "$" + Input.mousePosition.ToString();
            sendMessages.Add(posTra);
        }
    }

    private void InitClient()
    {
        if (clinetThread != null && clinetThread.IsAlive)
        {
            return;
        }
        Debug.Log("Start handle SendMessgae");
        clinetThread = new Thread(()=> {
            udpClient = new UdpClient();
            while (clinetThreadIsRun)
            {
                Thread.Sleep(10);
                //Debug.LogWarning("SendCheck "+ sendMessages.Count);
                try
                {
                    lock (sendMessages)
                    {
                        if (sendMessages.Count > 0)
                        {
                            for (int i = 0;i < sendMessages.Count;i++)
                            {
                                byte[] buf = Encoding.Unicode.GetBytes(sendMessages[i]);
                                udpClient.Send(buf, buf.Length, new IPEndPoint(IPAddress.Broadcast, port));
                                Debug.LogError("Send msg" + sendMessages[i]);
                            }
                        }
                        sendMessages.Clear();
                    }
                }catch(Exception e)
                {
                    Debug.Log(e);
                }
            }
        });
        clinetThread.IsBackground = true;
        clinetThread.Start();
        clinetThreadIsRun = true;
    }

    private void StartListen()
    {
        if (listenThread != null && listenThread.IsAlive)
        {
            return;
        }
        Debug.Log("Start handle ReceiveMessgae");

        listenThread = new Thread(()=> {
            udpListen = new UdpClient();
            while (listenThreadIsRun)
            {
                Thread.Sleep(10);
                //Debug.LogWarning("SendListen " + receiveMessages.Count);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, port);
                byte[] bufReceive = udpListen.Receive(ref endPoint);
                string msg = Encoding.Unicode.GetString(bufReceive,0,bufReceive.Length);
                if (msg.Contains(identifier))
                {
                    Debug.LogError("Receive msg" + msg);
                    receiveMessages.Add(msg);
                }
            }
        });
        listenThread.IsBackground = true;
        listenThread.Start();
        listenThreadIsRun = true;
    }
    private void OnApplicationQuit()
    {
        Stop();
    }
    private void Stop()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
        clinetThread?.Abort();
        clinetThreadIsRun = false;

        if (udpListen!= null)
        {
            udpListen.Close();
        }
        listenThread?.Abort();
        listenThreadIsRun = false;
    }

    private void GetIdentification()
    {
#if UNITY_EDITOR
        appName = "Unity";
#endif
        Process[] processes = Process.GetProcesses();
        for (int i = 0;i < processes.Length;i++)
        {
            if (appName == processes[i].ProcessName)
            {
                currentPID = processes[i].Id.ToString();
                Debug.Log("Find PID = " + currentPID);
                break;
            }
        }
    }

}
