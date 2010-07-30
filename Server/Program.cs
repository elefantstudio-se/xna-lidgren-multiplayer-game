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
        private static short maxPlayers = 4;
        private static bool[] usedPlayerPositions;
        private static readonly Random randomizer = new Random();
        private static Dictionary<long, PlayerTransferableData> players;
        private static Dictionary<long, ProjectileTransferableData> projectiles;
        private static Dictionary<long, HealthTransferableData> playerHealth;

        static void Main(string[] args)
        {
            usedPlayerPositions = new bool[maxPlayers];
            players = new Dictionary<long, PlayerTransferableData>();
            projectiles = new Dictionary<long, ProjectileTransferableData>();
            playerHealth = new Dictionary<long, HealthTransferableData>();

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
                        SendHealthData();
                        SendProjectilesData();

                        for (int i = projectiles.Count; i > 0; i--)
                        {
                            var currentProjectile = projectiles.Values.ElementAt(i - 1);
                            if (!currentProjectile.IsValid)
                            {
                                projectiles.Remove(currentProjectile.ID);
                            }
                        }
                        nextSendUpdates += (1.0 / 30.0);
                    }
                    server.Recycle(msg);
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
            NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
            if (status == NetConnectionStatus.Connected) // A new player has connected
            {
                var newPlayerData = SendInitialData(msg.SenderConnection);
                players[newPlayerData.SessionID] = newPlayerData;
                SendPlayersData();
            }
        }

        static void Data(NetIncomingMessage msg)
        {
            var data = msg.ReadTransferType();
            switch (data)
            {
                case Helpers.TransferType.PlayerUpdate: ReceivedPlayerData(msg); break;
                case Helpers.TransferType.ProjectileUpdate: ReceivedProjectileData(msg); break;
                case Helpers.TransferType.HealthUpdate: ReceivedHealthData(msg); break;
                case Helpers.TransferType.ClientDisconnect: ReceivedPlayerDisconnection(msg); break;
            }
        }

        static void ReceivedPlayerDisconnection(NetIncomingMessage msg)
        {
            var data = new ClientDisconnectedTransferableData(msg);
            usedPlayerPositions[data.PlayerIndex] = false;
            players.Remove(data.SessionID);
            Console.WriteLine(String.Format("Player {0} disconnected [{1}]", data.PlayerIndex, data.SessionID));
        }
        static void ReceivedPlayerData(NetIncomingMessage msg)
        {
            var data = new PlayerTransferableData(msg);
            players[data.SessionID] = data;
            SendPlayersData();
        }

        static void ReceivedProjectileData(NetIncomingMessage msg)
        {
            var data = new ProjectileTransferableData(msg);
            projectiles[data.ID] = data;
        }

        static void ReceivedHealthData(NetIncomingMessage msg)
        {
            var data = new HealthTransferableData(msg);
            playerHealth[data.ID] = data;
        }

        static PlayerTransferableData SendInitialData(NetConnection receiver)
        {
            short playerIndex = GetNextAvailablePlayerPosition();
            if (playerIndex == -1)
            {
                //No more positions available
                //TODO: do something when not more seats are available
            }
            usedPlayerPositions[playerIndex] = true;
            Vector2 initialPosition = new Vector2(randomizer.Next(screenWidth), randomizer.Next(screenHeight));
            var data = new PlayerTransferableData(receiver.RemoteUniqueIdentifier, Helpers.GetNewID(), playerIndex, initialPosition, 0f, true);
            NetOutgoingMessage om = server.CreateMessage();
            om.Write(Helpers.TransferType.NewConnection);
            om.Write(data);
            server.SendMessage(om, receiver, NetDeliveryMethod.Unreliable);

            Console.WriteLine(String.Format("Player {0} connected [{1}]", data.Index, data.SessionID));
            return data;
        }

        static void SendUpdates<T>(Dictionary<long, T> dataCollection, Helpers.TransferType type) where T:ITransferable
        {
            foreach (var client in server.Connections)
            {
                foreach (var entity in dataCollection.Values)
                {
                    if (entity.SessionID == client.RemoteUniqueIdentifier)
                    {
                        continue;
                    }
                    NetOutgoingMessage om = server.CreateMessage();
                    om.Write(type);
                    entity.WriteToMessage(om);
                    server.SendMessage(om, client, entity.DeliveryMethod);
                }
            }
        }
        static void SendProjectilesData()
        {
            SendUpdates(projectiles, Helpers.TransferType.ProjectileUpdate);
        }

        static void SendPlayersData()
        {
            SendUpdates(players, Helpers.TransferType.PlayerUpdate);
        }

        static void SendHealthData()
        {
            SendUpdates(playerHealth, Helpers.TransferType.HealthUpdate);
        }

        static short GetNextAvailablePlayerPosition()
        {
            for (short i = 0; i < usedPlayerPositions.Length; i++)
            {
                if (!usedPlayerPositions[i])
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
