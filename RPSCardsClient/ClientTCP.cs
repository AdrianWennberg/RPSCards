using System;
using System.Net;
using System.Net.Sockets;
using NetworkDataTypes;

namespace RPSCardsClient
{
    class ClientTCP
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        const int PORT = 5555;

        public static void ConnectToServer()
        {
            Console.WriteLine("Connecting to server...");
            HandleNetworkData.Initialize();
            _clientSocket.Connect("127.0.0.1", PORT);
            while (OnRecieve()) ;
        }

        private static bool OnRecieve()
        {
            try
            {
                byte[] sizeinfo = new byte[4];
                byte[] recievedBuffer = new byte[1024];

                int totalread = 0, currentread = 0;

                currentread = totalread = _clientSocket.Receive(sizeinfo);

                if (totalread <= 0)
                    return false;

                while (totalread < sizeinfo.Length && currentread > 0)
                {
                    currentread = _clientSocket.Receive(sizeinfo, totalread, sizeinfo.Length - totalread, SocketFlags.None);
                    totalread += currentread;
                }

                if (totalread < sizeinfo.Length)
                    return false;


                if (BitConverter.IsLittleEndian)
                    Array.Reverse(sizeinfo);

                int messageSize = BitConverter.ToInt32(sizeinfo);

                byte[] data = new byte[messageSize];
                totalread = currentread = _clientSocket.Receive(data, 0, data.Length, SocketFlags.None);

                while (totalread < messageSize && currentread > 0)
                {
                    currentread = _clientSocket.Receive(sizeinfo, totalread, messageSize - totalread, SocketFlags.None);
                    totalread += currentread;
                }

                if (totalread < messageSize)
                    return false;

                // handle network info
                HandleNetworkData.HandleData(data);

                return true;
            }
            catch { return false; }
        }

        public static void RequestUsername()
        {
            Console.Write("Input a username: ");

            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInt((int)ClientPackets.CRequestUsername);
            buffer.WriteString(Console.ReadLine().Trim());
            SendData(buffer);
        }

        public static void StartNewGame()
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInt((int)ClientPackets.CNewGame);
            SendData(buffer);
        }

        public static void SendData(PacketBuffer data)
        {
            _clientSocket.Send(data.ToArray());
            data.Dispose();
        }
    }
}