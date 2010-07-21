using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Shared;

namespace Server
{
    class Program
    {
        private const int PORT = 8081;
        private static int screenWidth = 640;
        private static int screenHeight = 480;
        private static NetServer server;
        private static short nextPlayerIndex;
        private static readonly Random randomizer = new Random();
        private static Dictionary<long, TransferableObjectData> players;
        private static Dictionary<long, TransferableObjectData> projectiles;

        static void Main(string[] args)
        {
            players = new Dictionary<long, TransferableObjectData>();
            projectiles = new Dictionary<long, TransferableObjectData>();
            NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = PORT;

            server = new NetServer(config);
            server.Start();

            Console.WriteLine("Welcome to the Game Server");
            Console.WriteLine("Currently awaiting new connections");

            double nextSendUpdates = NetTime.Now;

            while (!Console.KeyAvailable || Console.ReadKey().Key != ConsoleKey.Escape)
            {
                NetIncomingMessage msg;
                while ((msg = server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest: DiscoveryRequest(msg);
                            break;
                        case NetIncomingMessageType.StatusChanged: StatusChanged(msg);
                            break;
                        case NetIncomingMessageType.Data: Data(msg);
                            break;
                    }

                    double now = NetTime.Now;
                    if (now > nextSendUpdates) //Send new updates
                    {
                        SendPlayersData();

                        nextSendUpdates += (1.0/30.0);
                    }
                }
                Thread.Sleep(1);
            }

            server.Shutdown("Server is shutting down");
        }

        static void DiscoveryRequest(NetIncomingMessage msg)
        {
            server.SendDiscoveryResponse(null, msg.SenderEndpoint);
        }

        static void StatusChanged(NetIncomingMessage msg)
        {
            NetConnectionStatus status = (NetConnectionStatus) msg.ReadByte();
            if (status == NetConnectionStatus.Connected) // A new player has connected
            {
                var newPlayerData = SendInitialData(msg.SenderConnection);
                players[newPlayerData.SessionID] = newPlayerData;
                InformAboutNewPlayer(newPlayerData);
            }
        }

        static void Data(NetIncomingMessage msg)
        {
            var data = msg.ReadString();
            switch (data)
            {
                case "player_data": ReceivedPlayerData(msg); break;
                case "projectile_data": ReceivedProjectileData(msg); break;
            }
        }

        static void ReceivedPlayerData(NetIncomingMessage msg)
        {
            var data = msg.ReadObjectData();
            if (players.ContainsKey(data.SessionID))
            {
                players[data.SessionID] = data;
            }
        }

        static void ReceivedProjectileData(NetIncomingMessage msg)
        {
            var data = msg.ReadObjectData();
            projectiles[data.SessionID] = data;
        }

        static TransferableObjectData SendInitialData(NetConnection receiver)
        {
            short playerIndex = nextPlayerIndex++;
            Vector2 initialPosition = new Vector2(randomizer.Next(screenWidth), randomizer.Next(screenHeight));
            var data = new TransferableObjectData(receiver.RemoteUniqueIdentifier, Shared.Helpers.GetNewID(), playerIndex, initialPosition, 0f, new Vector2(0, 10));
            NetOutgoingMessage om = server.CreateMessage();
            om.Write("new_connection");
            om.Write(data);
            server.SendMessage(om, receiver, NetDeliveryMethod.Unreliable);

            Console.WriteLine("New player connected:" + receiver.RemoteUniqueIdentifier);
            Console.WriteLine(String.Format("X: {0}, Y: {1}", initialPosition.X, initialPosition.Y));

            return data;
        }

        static void InformAboutNewPlayer(TransferableObjectData newPlayerData)
        {
            foreach (var player in server.Connections)
            {
                if (player.RemoteUniqueIdentifier == newPlayerData.SessionID)
                {
                    continue;
                }
                NetOutgoingMessage om = server.CreateMessage();
                om.Write("new_player");
                om.Write(newPlayerData);
                server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
            }
        }

        static void SendPlayersData()
        {
            foreach(NetConnection player in server.Connections)
            {
                foreach (NetConnection otherPlayer in server.Connections)
                {
                    if (player.RemoteUniqueIdentifier == otherPlayer.RemoteUniqueIdentifier || !players.ContainsKey(otherPlayer.RemoteUniqueIdentifier))
                    {
                        continue;
                    }
                    NetOutgoingMessage om = server.CreateMessage();
                    var data = players[otherPlayer.RemoteUniqueIdentifier];
                    om.Write("player_data");
                    om.Write(data);

                    server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                }

                foreach (var projectile in projectiles.Values)
                {
                    if (projectile.SessionID == player.RemoteUniqueIdentifier)
                    {
                        continue;
                    }
                    NetOutgoingMessage om = server.CreateMessage();
                    om.Write("projectile_data");
                    om.Write(projectile);

                    server.SendMessage(om, player, NetDeliveryMethod.Unreliable);
                }
            }
        }
    }
}
