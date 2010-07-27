using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    class ProjectileRemote:Projectile
    {
        public ProjectileRemote(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories)
        {
        }

        public override void Update(GameTime gameTime, TransferableObjectData remoteData)
        {
            //if (!IsInScreen)
            //{
            //    IsValid = false;
            //}
            Position = remoteData.Position;
            base.Update(gameTime, remoteData);
        }
    }
}
