using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework;

namespace Client.Projectiles
{
    class ProjectileLocal:Projectile
    {
        public event EventHandler<ProjectileHitPlayerEventArgs> PlayerHit = delegate { };
        public ProjectileLocal(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories)
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
        public override void Update(GameTime gameTime, Shared.TransferableObjectData remoteData)
        {
            //if (!IsInScreen)
            //{
            //    IsValid = false;
            //}
            base.Update(gameTime, remoteData);
            //Position += Velocity;
        }
    }
}
