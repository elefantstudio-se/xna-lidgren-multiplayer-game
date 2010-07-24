using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
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
        public static Rectangle Screen { get; set; }

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
        private PhysicsSimulator physicsSimulator;
        private float playerMass = 5;
        private float playerSpeed = 5;

        private Texture2D boxTex;
        private Body boxBody;
        private Geom boxGeom;
        private Vector2 boxOrigin;

        public Game1(string host, int port)
        {
            this.port = port;
            this.host = host;
            graphics = new GraphicsDeviceManager(this) { PreferredBackBufferWidth = 640, PreferredBackBufferHeight = 480 };
            Screen = new Rectangle(0, 0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
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
            IsFixedTimeStep = true;
            TargetElapsedTime = new TimeSpan(0,0,0,0,10); //10ms --> 100 fps for physics update
            physicsSimulator = new PhysicsSimulator(Vector2.Zero);

            BackgroundColor = Color.CornflowerBlue;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            remotePlayerList = new RemoteObjectList(this, physicsSimulator, new PlayerUpdater());
            remoteProjectileList = new RemoteObjectList(this, physicsSimulator, new ProjectileUpdater());
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
            boxTex = Content.Load<Texture2D>("box");
            boxBody = BodyFactory.Instance.CreateRectangleBody(physicsSimulator, boxTex.Width, boxTex.Height, 1);
            boxBody.Position = new Vector2(100);
            boxGeom = GeomFactory.Instance.CreateRectangleGeom(physicsSimulator, boxBody, boxTex.Width, boxTex.Height);
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
                    SendLocalPlayerData();
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

            if (localPlayer != null)
            {
                localPlayer.Draw(gameTime);
            }
            remotePlayerList.Draw(gameTime);
            remoteProjectileList.Draw(gameTime);
            spriteBatch.Draw(boxTex, boxBody.Position,null,Color.White,boxBody.Rotation, new Vector2(boxTex.Width/2, boxTex.Height/2),1, SpriteEffects.None,1);
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
            localPlayer = new Player(this, physicsSimulator, data.SessionID, data.ID, "Players/Avatars/" + SharedLists.PlayerTextureNames[data.Index],data.Position,0,0.5f,playerMass,playerSpeed,data.Index,new KeyboardControls(Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Space));
            //localPlayer.PlayerUpdated += (s, e) => SendLocalPlayerData();
            //localPlayer.ProjectileFired += (s, e) =>
            //                                   {
            //                                       Console.WriteLine("fire");
            //                                   };
        }

        void UpdateOtherPlayer(NetIncomingMessage msg)
        {
            var playerData = msg.ReadObjectData();
            if (remotePlayerList.Exists(playerData.ID))
            {
                remotePlayerList.UpdateData(playerData);
            } else
            {
                //remotePlayerList.Add(playerData, Content.Load<Texture2D>("Players/Avatars/" + SharedLists.PlayerTextureNames[playerData.Index]), new Vector2(0, 10));
                remotePlayerList.Add(playerData, Content.Load<Texture2D>("Players/Avatars/" + SharedLists.PlayerTextureNames[playerData.Index]), new Vector2(0, 10), playerMass);
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
                Console.WriteLine("new bullet received");
                //remoteProjectileList.Add(projectileData,Content.Load<Texture2D>("Players/Projectiles/" + SharedLists.ProjectileTextureNames[projectileData.Index]),Vector2.Zero);
                remoteProjectileList.Add(projectileData,Content.Load<Texture2D>("Players/Projectiles/" + SharedLists.ProjectileTextureNames[projectileData.Index]),Vector2.Zero, 2);
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
            //foreach (var projectile in localPlayer.Projectiles)
            //{
            //    NetOutgoingMessage om = client.CreateMessage();
            //    om.Write("projectile_data");
            //    om.Write(new TransferableObjectData(localPlayer.SessionID, projectile.ID, localPlayer.Index,projectile.Position,projectile.Angle));
            //    client.SendMessage(om, NetDeliveryMethod.UnreliableSequenced);
            //}
        }
    }
}