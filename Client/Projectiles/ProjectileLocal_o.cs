using System;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework;

namespace Client.Projectiles
{
    class ProjectileLocal_o:Projectile_o
    {
        public event EventHandler<ProjectileHitPlayerEventArgs> PlayerHit = delegate { };

        public ProjectileLocal_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
            Geometry.OnCollision += OnCollision;
            //Body.ApplyForce((Position + Velocity));
            Body.ApplyImpulse(Velocity * Speed);
        }

        bool OnCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            return false;
        }
        public override void Update(GameTime gameTime, Shared.TransferableObjectData remoteData)
        {
            if (!IsInScreen)
            {
                IsValid = false;
            }
            base.Update(gameTime, remoteData);
            //Position += Velocity;
        }
    }
}
