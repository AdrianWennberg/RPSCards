using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace RPSCardsServer
{
    class Client
    {
        public int index;
        public string ip;
        public Socket socket;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];
        public string username;
        public IGame currentGame;

        public void StartClient()
        {
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            closing = false;
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int recieved = socket.EndReceive(ar);
                if (recieved <= 0)
                    CloseClient(index);
                else
                {
                    byte[] dataBuffer = new byte[recieved];
                    Array.Copy(_buffer, dataBuffer, recieved);
                    HandleNetworkData.HandleData(this, dataBuffer);

                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
                }
            }
            catch
            {
                CloseClient(index);
            }
        }

        private void CloseClient(int index)
        {
            if(String.IsNullOrEmpty(username) == false)
            {
                ServerTCP._clientsByUsername.Remove(username);
            }

            closing = true;
            Console.WriteLine("Connection form {0} has been closed.", ip, username);
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            socket = null;
        }

        public override string ToString()
        {
            return ip + (username == null ? "" : " (" + username + ")");
        }
    }
}
