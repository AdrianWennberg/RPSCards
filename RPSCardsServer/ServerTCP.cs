using System;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using NetworkDataTypes;

namespace RPSCardsServer
{
    static class ServerTCP
    {
        static readonly int port = 5555;
        static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public const int MAX_PLAYERS = 1024;
        public static Client[] _clients = new Client[MAX_PLAYERS];
        public static Dictionary<string, Client> _clientsByUsername = new Dictionary<string, Client>();
        public static IGame[] _games = new IGame[MAX_PLAYERS/2];

        public static Mutex _waitingMutex = new Mutex();
        public static Client[] _waiting = new Client[1];

        public static void Init()
        {
            Console.WriteLine("Initializing server");
            HandleNetworkData.Initialize();
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            _serverSocket.Listen(10);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            for(int i = 0; i < MAX_PLAYERS; i++)
                _clients[i] = new Client();
        }

        static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);

            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            for(int i = 0; i < MAX_PLAYERS; i++)
            {
                if(_clients[i].socket == null)
                {
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    _clients[i].StartClient();
                    Console.WriteLine("New conection from '{0}' recieved",_clients[i].ip);
                    SendConnectionOK(i);
                    return;
                }
            }

        }

        public static void SendDataTo(int index, byte[] data)
        {
            byte[] sizeinfo = BitConverter.GetBytes(data.Length);

            if(BitConverter.IsLittleEndian)
                Array.Reverse(sizeinfo);

            _clients[index].socket.Send(sizeinfo);
            _clients[index].socket.Send(data);
        }

        public static void SendConnectionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInt((int)ServerPackets.SConnectionOK);
            buffer.WriteString(string.Format("Sucessful connection! {0}", _clients[index].ip));
            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();    
        }
    }
}