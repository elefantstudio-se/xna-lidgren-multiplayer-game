using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class PhysicsGameObject:DrawableGameObject
    {
        public float Mass { get; set; }
        public PhysicsSimulator PhysicsSimulator;
        public Body Body { get; set;}
        public Geom Geometry{ get; set;}

        public PhysicsGameObject(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath)
        {
            PhysicsSimulator = physicsSimulator;
            Speed = speed;
            Mass = mass;

            uint[] colorData = new uint[Texture.Width * Texture.Height];
            Texture.GetData(colorData);
            Vertices vertices = Vertices.CreatePolygon(colorData, Texture.Width, Texture.Height);
            Origin = vertices.GetCentroid();
            Body = BodyFactory.Instance.CreatePolygonBody(PhysicsSimulator, vertices, mass);
            Position = position;
            Angle = angle;
            Geometry = GeomFactory.Instance.CreatePolygonGeom(physicsSimulator, Body, vertices, 0);
            Geometry.CollisionCategories = collisionCategories;
            Geometry.CollidesWith = CollidesWith(collisionCategories);
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

        public sealed override Vector2 Position
        {
            get { return Body.Position; }
            set { Body.Position = value; }
        }

        public sealed override float Angle
        {
            get { return Body.Rotation; }
            set { Body.Rotation = value; }
        }

        public override void Destroy()
        {
            Body.Dispose();
            Geometry.Dispose();
        }
    }
}
