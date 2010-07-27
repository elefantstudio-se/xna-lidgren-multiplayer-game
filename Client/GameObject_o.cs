﻿using System;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared;

namespace Client
{
    abstract class GameObject_o<T> where T:ITransferable
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public float ZOrder { get; set; }
        public float Speed { get; set; }
        public Vector2 Velocity
        {
            get
            {
                return new Vector2((float)Math.Sin(Body.Rotation),-(float)Math.Cos(Body.Rotation)) * Speed;
            }
        }

        public Vector2 Position
        {
            get
            {
                return Body.Position;
            }
            set
            {
                Body.Position = value;
            }
        }

        public float Angle
        {
            get
            {
                return Body.Rotation;
            }
            set
            {
                Body.Rotation = value;
            }
        }
        public bool IsValid { get; set; }

        private Texture2D Texture;
        protected Body Body;
        protected Geom Geometry;
        public Vector2 Origin{ get; set;}

        private Rectangle screenBounds;
        protected Game Game{ get; set;}
        protected SpriteBatch spriteBatch;
        protected PhysicsSimulator PhysicsSimulator { get; set; }


        protected Rectangle RelativeBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            }
        }
        protected bool IsInScreen
        {
            get
            {
                return screenBounds.Intersects(RelativeBounds);
            }
        }

        public float Width
        {
            get
            {
                return Texture.Width;
            }
        }

        public float Height
        {
            get
            {
                return Texture.Height;
            }
        }

        public Vector2 Scale { get; set; }

        protected GameObject_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories)
        {
            Game = game;
            PhysicsSimulator = physicsSimulator;
            SessionID = sessionID;
            ID = id;
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            ZOrder = zOrder;
            Speed = speed;
            IsValid = true;

            Texture = game.Content.Load<Texture2D>(imageAssetPath);
            uint[] colorData = new uint[Texture.Width * Texture.Height];
            Texture.GetData(colorData);
            Vertices vertices = Vertices.CreatePolygon(colorData, Texture.Width, Texture.Height);
            Origin = vertices.GetCentroid();
            Body = BodyFactory.Instance.CreatePolygonBody(physicsSimulator, vertices, mass);
            Body.Position = initialPosition;
            Body.Rotation = initialAngle;
            Geometry = GeomFactory.Instance.CreatePolygonGeom(physicsSimulator, Body, vertices, 0);
            Geometry.CollisionCategories = collisionCategories;
            Geometry.CollidesWith = CollidesWith(collisionCategories);
            screenBounds = game.GraphicsDevice.ScissorRectangle;
            Scale = Vector2.One;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (IsInScreen)
            {
                spriteBatch.Draw(Texture, Body.Position, null, Color.White, Body.Rotation, Origin, Scale, SpriteEffects.None, ZOrder);
            }
        }

        public virtual void Update(GameTime gameTime, T remoteData) { }

        public void Dispose()
        {
            Body.Dispose();
            Geometry.Dispose();
        }

        protected Keys[] InputKeys
        {
            get
            {
                return Keyboard.GetState().GetPressedKeys();
            }
        }

        static CollisionCategory CollidesWith(CollisionCategory categories)
        {
            ///Categories:
            /// 1: Local Player
            /// 2: Remote Players
            /// 3: Local Projecties
            /// 4: Remote Projectiles
            switch (categories)
            {
                case CollisionCategory.Cat1: return CollisionCategory.All & ~CollisionCategory.Cat3;
                case CollisionCategory.Cat2: return CollisionCategory.All & ~CollisionCategory.Cat4;
                case CollisionCategory.Cat3: return CollisionCategory.All & ~CollisionCategory.Cat1;
                case CollisionCategory.Cat4: return CollisionCategory.All & ~CollisionCategory.Cat2;
                default: return CollisionCategory.All;
            }
        }
    }
}

