using System;
using System.Collections.Generic;
using System.Linq;
using Client.Entities;
using Client.Factories;
using Client.Players;
using FarseerGames.FarseerPhysics;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared;
namespace Client
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Color BackgroundColor { get; set; }
        public static Rectangle Screen { get; set; }

        private readonly int port;
        private readonly string host;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private NetClient client;
        private RemoteObjectList RemoteObjectsList;
        private LocalObjectList LocalObjectList;
        private Dictionary<long, PlayerRemote> remotePlayers;
        private PlayerFactory playerFactory;
        private ProjectileFactory projectileFactory;
        private HealthBarFactory healthBarFactory;
        private LocalPlayer localPlayer;
        private HealthBar localHealthBar;
        private double nextSendUpdate = NetTime.Now;
        private double updateInterval = (1.0/1000.0);
        private PhysicsSimulator physicsSimulator;
        private float playerZOrder = 0.5f;
        private float playerMass = 5;
        private float playerSpeed = 25;

        public Game1(string host, int port)
        {
            this.port = port;
            this.host = host;
            graphics = new GraphicsDeviceManager(this) { PreferredBackBufferWidth = 640, PreferredBackBufferHeight = 480 };
            Screen = new Rectangle(0, 0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Content.RootDirectory = "Content";
            var config = new NetPeerConfiguration("xnaapp");
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
            IsFixedTimeStep = true;
            TargetElapsedTime = new TimeSpan(0,0,0,0,10); //10ms --> 100 fps for physics update
            physicsSimulator = new PhysicsSimulator(Vector2.Zero);

            BackgroundColor = Color.CornflowerBlue;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            RemoteObjectsList = new RemoteObjectList();
            LocalObjectList = new LocalObjectList();
            remotePlayers = new Dictionary<long, PlayerRemote>();

            projectileFactory = new ProjectileFactory(this, physicsSimulator, playerZOrder, 5, 50, "Players/Projectiles/",SharedLists.ProjectileTextureNames);
            playerFactory = new PlayerFactory(this, physicsSimulator, 0, playerMass, playerSpeed, "Players/Avatars/",SharedLists.PlayerTextureNames, projectileFactory);
            healthBarFactory = new HealthBarFactory(this, 70, 20, 100, 100);


            client.DiscoverKnownPeer(host, port);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            NetOutgoingMessage om = client.CreateMessage();
            om.Write(Helpers.TransferType.ClientDisconnect);
            om.Write(new ClientDisconnectedTransferableData(client.UniqueIdentifier, localPlayer.Index));
            client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
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

            LocalObjectList.Update(gameTime);
            RemoteObjectsList.Update(gameTime);

            if (NetTime.Now > nextSendUpdate)
            {
                if (localPlayer != null) //Send periodic updates
                {
                    SendProjectilesData();
                    SendLocalPlayerData();
                    SendHealthData();
                    nextSendUpdate += updateInterval;
                }
            }
            ReadMessages();
            physicsSimulator.Update(gameTime.ElapsedGameTime.Milliseconds * .001f);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            LocalObjectList.Draw(gameTime);
            RemoteObjectsList.Draw(gameTime);
            spriteBatch.End();
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
            client.Recycle(msg);
            }
        }

        void ReadData(NetIncomingMessage msg)
        {
            var type = msg.ReadTransferType();
            switch (type)
            {
                case Helpers.TransferType.NewConnection: NewServerConnection(msg); break;
                case Helpers.TransferType.PlayerUpdate: UpdateOtherPlayer(msg); break;
                case Helpers.TransferType.ProjectileUpdate: UpdateProjectile(msg); break;
                case Helpers.TransferType.HealthUpdate:UpdateHealthBar(msg); break;
            }
        }

        void NewServerConnection(NetIncomingMessage msg)
        {
            var data = new PlayerTransferableData(msg);
            localPlayer = playerFactory.NewPlayer(data.SessionID, data.ID, data.Index, data.Position, data.Angle, new KeyboardControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space));
            localHealthBar = healthBarFactory.NewHealthBar(client.UniqueIdentifier, Helpers.GetNewID(), localPlayer.Index, new Vector2(localPlayer.Index*150 + 50, 25));
            LocalObjectList.Add(localPlayer,localHealthBar);
        }

        void UpdateOtherPlayer(NetIncomingMessage msg)
        {
            var playerData = new PlayerTransferableData(msg);
            if (RemoteObjectsList.Exists(playerData.ID))
            {
                RemoteObjectsList.UpdateData(playerData);
            }
            else
            {
                PlayerRemote newPlayer = playerFactory.NewRemotePlayer(playerData.SessionID, playerData.ID, playerData.Index, playerData.Position, playerData.Angle);
                RemoteObjectsList.Add(newPlayer, playerData);
                remotePlayers.Add(newPlayer.SessionID,newPlayer);
            }
        }

        void UpdateProjectile(NetIncomingMessage msg)
        {
            var projectileData = new ProjectileTransferableData(msg);
            if (RemoteObjectsList.Exists(projectileData.ID))
            {
                RemoteObjectsList.UpdateData(projectileData);
            }
            else
            {
                RemoteObjectsList.Add(projectileFactory.NewRemoteProjectile(projectileData.SessionID, projectileData.ID, remotePlayers[projectileData.SessionID].Index, projectileData.Position, projectileData.Angle), projectileData);
            }
        }

        void UpdateHealthBar(NetIncomingMessage msg)
        {
            var healthData = new HealthTransferableData(msg);
            if (RemoteObjectsList.Exists(healthData.ID))
            {
                RemoteObjectsList.UpdateData(healthData);
            }
            else
            {
                var newHealthBar = healthBarFactory.NewHealthBar(client.UniqueIdentifier, healthData.ID, healthData.PlayerIndex, new Vector2(healthData.PlayerIndex*150 + 50, 25));
                RemoteObjectsList.Add(newHealthBar,healthData);
            }
        }

        void SendLocalPlayerData()
        {
            localPlayer.SendUpdates(client);
        }

        void SendProjectilesData()
        {
            for (int i = localPlayer.Projectiles.Count; i > 0; i--)
            {
                var projectile = localPlayer.Projectiles.ElementAt(i - 1);
                projectile.SendUpdates(client);
                if (!projectile.IsValid)
                {
                    localPlayer.RemoveProjectile(projectile);
                }
            }
        }

        void SendHealthData()
        {
            localHealthBar.SendUpdates(client);
        }
    }
}