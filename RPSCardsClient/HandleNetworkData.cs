using System;
using System.Collections.Generic;
using NetworkDataTypes;

namespace RPSCardsClient
{
    static class HandleNetworkData
    {
        private delegate void Packet_(PacketBuffer data);
        private static Dictionary<int, Packet_> Packets;

        public static void Initialize()
        {
            Packets = new Dictionary<int, Packet_>(){
                {(int)ServerPackets.SConnectionOK, HandleConnectionOK},
                {(int)ServerPackets.SUsernameResponse, HandleUsernameResponse }
            };
        }

        public static void HandleData(byte[] data)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteBytes(data);
            int packeageNumber = buffer.ReadInt();

            if (Packets.TryGetValue(packeageNumber, out Packet_ Packet))
                Packet.Invoke(buffer);
        }

        private static void HandleConnectionOK(PacketBuffer buffer)
        {
            string msg = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine(msg);
            ClientTCP.RequestUsername();
        }

        private static void HandleUsernameResponse(PacketBuffer buffer)
        {
            if(buffer.ReadByte() == 0)
            {
                string msg = buffer.ReadString();
                buffer.Dispose();

                Console.WriteLine(msg);
                ClientTCP.RequestUsername();
            }
            else
            {
                Console.WriteLine("Sucessfully set username: {0}", buffer.ReadString());
                buffer.Dispose();
                ClientTCP.StartNewGame();
            }
        }
    }
}