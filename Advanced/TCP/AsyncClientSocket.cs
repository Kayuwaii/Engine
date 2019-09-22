using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Engine
{
    namespace Advanced
    {
        /// <summary>
        /// This namespace contains classes that allow TCP connections.
        /// </summary>
        namespace TCP
        {
            public class AsyncClientSocket : IDisposable
            {
                private TcpClient _client;

                private StreamReader _sReader;
                private StreamWriter _sWriter;

                public delegate void ServerFull(AsyncClientSocket sender);

                public event ServerFull OnServerFull;

                public delegate void ServerWarning(AsyncClientSocket sender);

                public event ServerWarning OnServerWarning;

                public delegate void MessageRecieved(string message);

                public event MessageRecieved OnMessageRecieved;

                public AsyncClientSocket(String ipAddress, int portNum)
                {
                    _client = new TcpClient();
                    _client.Connect(ipAddress, portNum);
                }

                public void Start()
                {
                    bool usable = GetStatus();
                    if (usable) HandleCommunication();
                    else this.Dispose();
                }

                private bool GetStatus()
                {
                    _sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);

                    if (_client.Connected)
                    {
                        byte sDataIncomming = Byte.Parse(_sReader.ReadLine());
                        if (sDataIncomming == 5)
                        {
                            OnServerFull(this);
                            return false;
                        }
                        else if (sDataIncomming == 3) OnServerWarning(this);
                        return true;
                    }
                    else return false;
                }

                private void HandleCommunication()
                {
                    Thread t = new Thread(new ParameterizedThreadStart(ReadAsync));
                    t.Start(_client);
                }

                private void ReadAsync(object obj)
                {
                    _sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);

                    while (_client.Connected)
                    {
                        String sDataIncomming = _sReader.ReadLine();
                        OnMessageRecieved(sDataIncomming);
                    }
                }

                /// <summary>
                /// This method is used to send a message to the endpoint that the TCP Client connected to
                /// </summary>
                /// <param name="message">The message to send.</param>
                public void SendMessage(string message)
                {
                    _sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
                    Console.Write("&gt; ");
                    string sData = Console.ReadLine();

                    _sWriter.WriteLine(sData);
                    _sWriter.Flush();
                }

                public void Dispose()
                {
                    _client.Close();
                }
            }
        }
    }
}