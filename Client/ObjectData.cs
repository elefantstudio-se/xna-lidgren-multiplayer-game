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
using Shared;

namespace Client
{
    class ObjectData
    {
        public TransferableObjectData RemoteData { get; set; }
        public Vector2 Origin { get; set; }
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

        public Texture2D Texture { get; set; }
        private Body Body;
        private Geom Geometry;

        public ObjectData(TransferableObjectData remoteData, PhysicsSimulator physicsSimulator, Texture2D texture, Vector2 centerOffset, float mass)
        {
            Texture = texture;
            uint[] colorData = new uint[Texture.Width * Texture.Height];
            Texture.GetData(colorData);
            Vertices vertices = Vertices.CreatePolygon(colorData, Texture.Width, Texture.Height);
            Origin = vertices.GetCentroid();
            Body = BodyFactory.Instance.CreatePolygonBody(physicsSimulator, vertices, mass);
            Geometry = GeomFactory.Instance.CreatePolygonGeom(physicsSimulator, Body, vertices, 0);

            RemoteData = remoteData;
        }
    }
}
