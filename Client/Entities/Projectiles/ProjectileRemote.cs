using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    class ProjectileRemote:Projectile, IRemotelyUpdateable
    {
        public ProjectileRemote(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories)
        {
        }

        public void Update(GameTime gameTime, ITransferable remoteData)
        {
            var data = (TransferableObjectData) remoteData;
            //if (!IsInScreen)
            //{
            //    IsValid = false;
            //}
            Position = data.Position;
        }
    }
}
