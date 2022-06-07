using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ChessGame.SocketUtil
{
    public class SocketClient
    {
        private string _ip = string.Empty;
        private int _port = 0;
        private Socket _socket = null;
        private byte[] buffer = new byte[1024 * 1024 * 2];
        private Form1 form;
        public static string DisConnected = "{6846223E-E179-48F8-8A2D-43DB07FA94A4}";

        public SocketClient(string ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public SocketClient(int port)
        {
            this._ip = "127.0.0.1";
            this._port = port;
        }

        public SocketClient(int port, Form1 form)
        {
            this._ip = "127.0.0.1";
            this._port = port;
            this.form = form;
        }

        public void StartClient()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(_ip);
                IPEndPoint endPoint = new IPEndPoint(address, _port);
                _socket.Connect(endPoint);
                //Console.WriteLine("连接服务器成功");
                Form1.testTalkBoxText.Text += "Client:\r\n" + "连接服务器成功" + "\r\n";   // Single Thread can set control directly.

                Thread thread = new Thread(RecieveMessage);
                thread.Start(_socket);
            }
            catch(Exception e)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                Console.WriteLine(e.Message);
            }
            //Console.WriteLine("发送消息结束");
            //Form1.testTalkBoxText.Text += "Client:\r\n" + "发送消息结束" + "\r\n";
            //Console.ReadKey();
        }

        private void RecieveMessage(object socket)
        {
            Socket clientSocket = (Socket)socket;
            while (clientSocket != null && clientSocket.Connected)
            {
                try
                {
                    int length = clientSocket.Receive(buffer);
                    string msg = Encoding.UTF8.GetString(buffer, 0, length);
                    this.form.SetTalkText("Client:\r\n" + String.Format("接收服务端{0}，消息{1}", clientSocket.RemoteEndPoint.ToString(), msg) + "\r\n");   // Mutiple Thread need delegate to change control.
                    if(msg.Equals(SocketClient.DisConnected))
                    {
                        if (_socket.Connected)
                            _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        break;
                    }
                    this.form.ProcessMsg(msg);
                }
                catch (Exception e)
                {
                    this.Close();
                    break;
                }
            }
        }
        public void SendMessage(string msg)
        {
            if(_socket != null && _socket.Connected)
            {
                try
                {
                    _socket.Send(Encoding.UTF8.GetBytes(msg));
                    Form1.testTalkBoxText.Text += "Client:\r\n" + msg + "\r\n";
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message + "   ooo");
                }
            }
        }
        public void Close()
        {
            if(_socket != null)
            {
                SendMessage(SocketServer.DisConnected); // Tell the server this connected socket should close.
                if(_socket.Connected)
                    _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            
        }
    }
}

/*
 * SocketClient client = new SocketClient(8888);
 * client.StartClient();
 * Console.ReadKey();
 
 */