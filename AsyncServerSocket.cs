using Engine.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
            /// <summary>
            /// Basic TCP "Server" class.
            /// </summary>
            public class AsyncServerSocket
            {
                private TcpListener _server;
                private bool _isRunning;
                private bool _isFull;
                private bool _isWarning;
                private int MAX_CONNECTIONS;
                public int CURRENT_CONNECTIONS = 0;
                private DateTime CREATION_TIME;
                public List<Client> Clients;

                public delegate void ClientConnected(Client sender);

                public event ClientConnected OnClientConnected;

                public void SetMAX_CONNECTIONS(int value)
                {
                    MAX_CONNECTIONS = value;
                }

                public AsyncServerSocket(int port)
                {
                    _server = new TcpListener(IPAddress.Any, port);
                    _server.Start();
                    MAX_CONNECTIONS = 10;
                    _isRunning = true;
                    _isFull = false;
                    _isWarning = false;
                    Clients = new List<Client>();
                }

                public void Start()
                {
                    CREATION_TIME = DateTime.Now;
                    while (_isRunning)
                    {
                        // wait for client connection
                        TcpClient connection = _server.AcceptTcpClient();
                        CURRENT_CONNECTIONS++;
                        Client newClient = new Client(CURRENT_CONNECTIONS, connection);
                        OnClientConnected(newClient);
                        CheckServerSatus();
                        SendServerStatus(newClient);

                        // client found.

                        if (_isFull)
                        {
                            ServerFull(newClient);
                            newClient = null;
                        }
                        else
                        {
                            Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                            t.Start(newClient);
                        }
                    }
                }

                private void HandleClient(object c)
                {
                    Client client = (Client)c;
                    client.OnConnectionClosed += Client_OnConnectionClosed;
                    Clients.Add(client);
                    client.Start();
                }

                private void Client_OnConnectionClosed(Client sender)
                {
                    CommandLine.ErrorMsg("Client " + sender.ID + " Disconected.");
                }

                private void CheckServerSatus()
                {
                    _isRunning = _server.Server.IsBound;
                    _isFull = CURRENT_CONNECTIONS > MAX_CONNECTIONS;
                    _isWarning = General.getPercent(MAX_CONNECTIONS, CURRENT_CONNECTIONS) >= 70 && CURRENT_CONNECTIONS <= MAX_CONNECTIONS;
                }

                private void SendServerStatus(Client newClient)
                {
                    byte status = 0;

                    bool[] stats = { _isRunning, _isWarning, _isFull };

                    // This assumes the array never contains more than 8 elements!
                    int index = 0;

                    // Loop through the array
                    foreach (bool b in stats)
                    {
                        // if the element is 'true' set the bit at that position
                        if (b)
                            status |= (byte)(1 << (index));

                        index++;
                    }

                    newClient.SendMessage(status.ToString());
                }

                private void ServerFull(Client obj)
                {
                    obj._client.Close();
                    CURRENT_CONNECTIONS--;
                }

                public void Stop()
                {
                    _isRunning = false;
                }

                public ServerStatus GetStatus()
                {
                    return new ServerStatus(this);
                }

                #region CHILD CLASSES

                public class ServerStatus
                {
                    public bool Running;
                    public bool Full;
                    public bool Warning;
                    public int MAX_CONNECTIONS;
                    public int CURRENT_CONNECTIONS;
                    public double USAGE_PERCENT;
                    public TimeSpan TimeRunning;

                    public ServerStatus(AsyncServerSocket server)
                    {
                        Running = server._isRunning;
                        Full = server._isFull;
                        Warning = server._isWarning;
                        MAX_CONNECTIONS = server.MAX_CONNECTIONS;
                        CURRENT_CONNECTIONS = server.CURRENT_CONNECTIONS;
                        USAGE_PERCENT = General.getPercent(MAX_CONNECTIONS, CURRENT_CONNECTIONS);
                        TimeRunning = DateTime.Now - server.CREATION_TIME;
                    }
                }

                public class Client
                {
                    public int ID;
                    public TcpClient _client;

                    public delegate void MessageRecieved(Client sender, string message);

                    public event MessageRecieved OnMessageRecieved;

                    public delegate void ConnectionClosed(Client sender);

                    public event ConnectionClosed OnConnectionClosed;

                    public Client(int iD, TcpClient client)
                    {
                        ID = iD;
                        _client = client;
                    }

                    public void Start()
                    {
                        StreamReader sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
                        // you could use the NetworkStream to read and write,
                        // but there is no forcing flush, even when requested

                        do
                        {
                            try
                            {
                                // reads from stream
                                string sData = sReader.ReadLine();
                                // shows content on the console.
                                OnMessageRecieved(this, sData);
                            }
                            catch (Exception ex)
                            {
                                //Console.WriteLine(ex.Message);
                            }
                        } while (_client.Connected);
                        OnConnectionClosed(this);
                    }

                    public void SendMessage(string message)
                    {
                        StreamWriter sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
                        sWriter.WriteLine(message);
                        sWriter.Flush();
                    }
                }

                #endregion CHILD CLASSES
            }
        }
    }
}