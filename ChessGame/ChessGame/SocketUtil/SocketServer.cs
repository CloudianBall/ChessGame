using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChessGame.SocketUtil
{
    public class SocketServer
    {
        private string _ip = string.Empty;
        private int _port = 0;
        private Socket _socket = null;
        private byte[] buffer = new byte[1024 * 1024 * 2];
        private Form1 form;
        public static string DisConnected = "{BA5D06A5-5887-4DC8-B068-CA342EA0BA9E}";
        public Socket ConnectedSocket { get; set; }
        
        public SocketServer(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public SocketServer(string ip, int port, Form1 form)
        {
            this._ip = ip;
            this._port = port;
            this.form = form;
        }
        public SocketServer(int port)
        {
            this._ip = "127.0.0.1";
            this._port = port;
        }
        public SocketServer(int port, Form1 form)
        {
            this._ip = "127.0.0.1";
            this._port = port;
            this.form = form;
        }
        public void StartListen()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(_ip);
                IPEndPoint endPoint = new IPEndPoint(address, _port);
                _socket.Bind(endPoint);
                _socket.Listen(1);
                //Console.WriteLine("监听{0}消息成功", _socket.LocalEndPoint.ToString());
                Form1.testTalkBoxText.Text += "Server:\r\n" + String.Format("监听{0}消息成功", _socket.LocalEndPoint.ToString()) + "\r\n";    // Single Thread can set control directly.
                Thread thread = new Thread(ListenClientConnect);
                thread.Start();
            }
            catch(Exception e)
            {

            }
        }

        public void Close()
        {
            if(ConnectedSocket != null && ConnectedSocket.Connected)
            {
                SendMessage(SocketClient.DisConnected); // Tell the client this connected socket should close. Because of the Shutdown's delay. 
                ConnectedSocket.Shutdown(SocketShutdown.Both);
                ConnectedSocket.Close();
            }
            _socket.Close();
        }

        private void ListenClientConnect()
        {
            try
            {
                while(true)
                {
                    Socket clientSocket = _socket.Accept();
                    this.ConnectedSocket = clientSocket;
                    clientSocket.Send(Encoding.UTF8.GetBytes("服务端发送的消息"));
                    this.form.SetTalkText("Server:\r\n" + "服务端发送的消息" + "\r\n"); // Mutiple Thread need delegate to change control.
                    Thread thread = new Thread(RecieveMessage);
                    thread.Start(clientSocket);
                }
            }
            catch (Exception e)
            {
                
            }
        }
        private void RecieveMessage(object socket)
        {
            Socket clientSocket = (Socket)socket;
            while(ConnectedSocket != null && ConnectedSocket.Connected)
            {
                try
                {
                    int length = clientSocket.Receive(buffer);
                    string msg = Encoding.UTF8.GetString(buffer, 0, length);
                    this.form.SetTalkText("Server:\r\n" + String.Format("接收客户端{0}，消息{1}", clientSocket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length)) + "\r\n");
                    if (msg.Equals(SocketServer.DisConnected))
                    {
                        if (ConnectedSocket != null && ConnectedSocket.Connected)
                        {
                            ConnectedSocket.Shutdown(SocketShutdown.Both);
                            ConnectedSocket.Close();
                        }
                        break; 
                    }
                    this.form.ProcessMsg(msg);
                }
                catch(Exception e)
                {
                    this.Close();
                    break;
                }
            }
        }

        public void SendMessage(string msg)
        {
            if(ConnectedSocket != null && ConnectedSocket.Connected)
            {
                try
                {
                    ConnectedSocket.Send(Encoding.UTF8.GetBytes(msg));
                    Form1.testTalkBoxText.Text += "Server:\r\n" + msg + "\r\n";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "   ooo");
                }
            }
        }
    }
}

/*
 * SocketServer server = new SocketServer(8888);
 * server.StartListen();
 * Console.ReadKey();

 */
