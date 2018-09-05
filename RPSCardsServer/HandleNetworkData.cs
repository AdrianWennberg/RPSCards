using System;
using System.Collections.Generic;
using NetworkDataTypes;

namespace RPSCardsServer
{
    static class HandleNetworkData
    {
        private delegate void Packet_(Client client, PacketBuffer data);
        private static Dictionary<int, Packet_> Packets;

        public static void Initialize()
        {
            Packets = new Dictionary<int, Packet_>(){
                { (int)ClientPackets.CRequestUsername, HandleSetUsername },
                { (int)ClientPackets.CNewGame, HandleNewGame },
            };
        }

        public static void HandleData(Client client, byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer(data);
            int packeageNumber = buffer.ReadInt();

            if(Packets.TryGetValue(packeageNumber, out Packet_ Packet))
                Packet.Invoke(client, buffer);
        }

        private static void HandleSetUsername(Client client, PacketBuffer buffer)
        {
            string username = buffer.ReadString();
            buffer.Dispose();

            PacketBuffer response = new PacketBuffer();
            response.WriteInt((int)ServerPackets.SUsernameResponse);

            if (String.IsNullOrEmpty(username))
            {
                response.WriteByte(0);
                response.WriteString("Please provide a valid username");
            }
            else
            {


                lock (ServerTCP._clientsByUsername)
                {
                    if (ServerTCP._clientsByUsername.ContainsKey(username))
                    {
                        response.WriteByte(0);
                        response.WriteString("{username} is allready taken");
                    }

                    else
                    {
                        Console.WriteLine("{0} has recieved the username: {1}", client.ip, username);

                        if (String.IsNullOrEmpty(client.username) == false)
                            ServerTCP._clientsByUsername.Remove(client.username);

                        client.username = username;
                        ServerTCP._clientsByUsername.Add(username, client);

                        response.WriteByte(1);
                        response.WriteString(client.username);
                    }
                }
            }

            ServerTCP.SendDataTo(client.index, response.ToArray());
            response.Dispose();
        }

        private static void HandleNewGame(Client client, PacketBuffer data)
        {
            data.Dispose();

            Client opponent;

            lock (ServerTCP._waiting)
            {
                // Create game or add player to game
                if (ServerTCP._waiting[0] == null)
                {
                    ServerTCP._waiting[0] = client;
                    return;
                }
                else
                {
                    opponent = ServerTCP._waiting[0];
                    ServerTCP._waiting = null;
                }
            }

            for (int i = 0; i < ServerTCP.MAX_PLAYERS/2; i++)
            {
                if(ServerTCP._games[i] == null)
                {
                    // Start Game
                    ServerTCP._games[i] = new Game(client.username, opponent.username);
                    client.currentGame = ServerTCP._games[i];
                    opponent.currentGame = ServerTCP._games[i];

                    Console.WriteLine("Starting a new game between {0} and {1}", client, opponent);

                    // Tell players about game
                    

                    break;
                }
            }
        }
    }
}