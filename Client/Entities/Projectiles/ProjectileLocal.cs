using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    class ProjectileLocal : Projectile, IUpdateSender
    {
        public event EventHandler<ProjectileHitPlayerEventArgs> PlayerHit = delegate { };
        public ProjectileLocal(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories)
            : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories)
        {
            Geometry.OnCollision += OnCollision;
            //Body.ApplyForce((Position + Velocity));
        }

        public void Fire()
        {
            Body.ApplyImpulse(Velocity * Speed);
        }
        bool OnCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            //if (!IsInScreen)
            //{
            //    IsValid = false;
            //}
            base.Update(gameTime);
            //Position += Velocity;
        }

        public void SendUpdates(NetClient client)
        {
            NetOutgoingMessage om = client.CreateMessage();
            om.Write("projectile_data");
            om.Write(new ProjectileTransferableData(client.UniqueIdentifier,ID,IsValid,Position,Angle));
            client.SendMessage(om, NetDeliveryMethod.UnreliableSequenced);
        }
    }
}
