using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Shared;
namespace Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Color BackgroundColor { get; set; }

        private readonly int port = 8081;
        private readonly string host;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private NetClient client;
        private RemoteObjectList remotePlayerList;
        private RemoteObjectList remoteProjectileList;
        private Player localPlayer;
        private double nextSendUpdate = NetTime.Now;
        private double updateInterval = (1.0/1000.0);

        public Game1(string host, int port)
        {
            this.port = port;
            this.host = host;
            graphics = new GraphicsDeviceManager(this) { PreferredBackBufferWidth = 640, PreferredBackBufferHeight = 480 };
            Content.RootDirectory = "Content";
            NetPeerConfiguration config = new NetPeerConfiguration("xnaapp");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            BackgroundColor = Color.CornflowerBlue;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            remotePlayerList = new RemoteObjectList(this, new PlayerUpdater());
            remoteProjectileList = new RemoteObjectList(this, new ProjectileUpdater());
            SharedLists.Players = remotePlayerList.ObjectsData;
            SharedLists.Projectiles = remotePlayerList.ObjectsData;

            client.DiscoverKnownPeer(host, port);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (localPlayer != null)
            {
                localPlayer.Update(gameTime);
            }
            remotePlayerList.Update(gameTime);
            remoteProjectileList.Update(gameTime);
            if (NetTime.Now > nextSendUpdate)
            {
                if (localPlayer != null)
                {
                    //Send periodic updates
                    SendProjectilesData();
                    nextSendUpdate += updateInterval;
                }
            }
            ReadMessages();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            if (localPlayer != null)
            {
                localPlayer.Draw(gameTime);
            }
            remotePlayerList.Draw(gameTime);
            remoteProjectileList.Draw(gameTime);
            base.Draw(gameTime);
        }

        void ReadMessages()
        {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse: client.Connect(msg.SenderEndpoint); break;
                    case NetIncomingMessageType.Data: ReadData(msg); break;
                }
            }
        }

        void ReadData(NetIncomingMessage msg)
        {
            var type = msg.ReadString();
            switch (type)
            {
                case "new_connection": NewServerConnection(msg); break;
                case "player_data": UpdateOtherPlayer(msg); break;
                case "projectile_data": UpdateProjectile(msg); break;
            }
        }

        void NewServerConnection(NetIncomingMessage msg)
        {
            
            var data = msg.ReadObjectData();
            localPlayer = new Player(this, data.SessionID, data.ID, data.Index,  SharedLists.PlayerTextureNames[data.Index], data.Position, 0, 0.5f, new KeyboardControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space), new Vector2(0,10));
            localPlayer.PlayerUpdated += (s, e) => SendLocalPlayerData();
            localPlayer.ProjectileFired += (s, e) =>
                                               {
                                                   Console.WriteLine("fire");
                                               };
        }

        void UpdateOtherPlayer(NetIncomingMessage msg)
        {
            var playerData = msg.ReadObjectData();
            if (remotePlayerList.Exists(playerData.ID))
            {
                remotePlayerList.UpdateData(playerData);
            } else
            {
                remotePlayerList.Add(playerData, Content.Load<Texture2D>("Players/Avatars/" + SharedLists.PlayerTextureNames[playerData.Index]), new Vector2(0, 10));
            }
        }

        void UpdateProjectile(NetIncomingMessage msg)
        {
            var projectileData = msg.ReadObjectData();
            if (remoteProjectileList.Exists(projectileData.ID))
            {
                remoteProjectileList.UpdateData(projectileData);
            } else
            {
                remoteProjectileList.Add(projectileData,Content.Load<Texture2D>("Players/Projectiles/" + SharedLists.ProjectileTextureNames[projectileData.Index]),Vector2.Zero);
            }
        }

        void SendLocalPlayerData()
        {
            NetOutgoingMessage om = client.CreateMessage();
            om.Write("player_data");
            om.Write(new TransferableObjectData(localPlayer.SessionID, localPlayer.ID, localPlayer.Index, localPlayer.Position, localPlayer.Angle));
            client.SendMessage(om, NetDeliveryMethod.Unreliable);
        }

        void SendProjectilesData()
        {
            foreach (var projectile in localPlayer.Projectiles)
            {
                NetOutgoingMessage om = client.CreateMessage();
                om.Write("projectile_data");
                om.Write(new TransferableObjectData(localPlayer.SessionID, projectile.ID, localPlayer.Index,projectile.Position,projectile.Angle));
                client.SendMessage(om, NetDeliveryMethod.Unreliable);
            }
        }
    }
}