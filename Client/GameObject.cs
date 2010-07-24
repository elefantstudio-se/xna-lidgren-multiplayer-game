using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Client
{
    abstract class GameObject
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

        private Texture2D Texture;
        private Body Body;
        private Geom Geometry;
        private Vector2 Origin;

        protected SpriteBatch spriteBatch;

        protected GameObject(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed)
        {
            SessionID = sessionID;
            ID = id;
            spriteBatch = (SpriteBatch) game.Services.GetService(typeof (SpriteBatch));
            ZOrder = zOrder;
            Speed = speed;

            Texture = game.Content.Load<Texture2D>(imageAssetPath);
            uint[] colorData = new uint[Texture.Width * Texture.Height];
            Texture.GetData(colorData);
            Vertices vertices = Vertices.CreatePolygon(colorData, Texture.Width, Texture.Height);
            Origin = vertices.GetCentroid();
            Body = BodyFactory.Instance.CreatePolygonBody(physicsSimulator, vertices, mass);
            Body.Position = initialPosition;
            Body.Rotation = initialAngle;
            Geometry = GeomFactory.Instance.CreatePolygonGeom(physicsSimulator, Body, vertices, 0);
        }

        public virtual void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Body.Position, null, Color.White, Body.Rotation, Origin, 1, SpriteEffects.None, ZOrder);
        }

        public virtual void Update(GameTime gameTime) { }

        protected Keys[] InputKeys
        {
            get
            {
                return Keyboard.GetState().GetPressedKeys();
            }
        }
    }
}
