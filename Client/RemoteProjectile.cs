using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class RemoteProjectile:GameObject
    {
        public RemoteProjectile(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
            Body.ApplyImpulse(Velocity * Speed);
        }

        public override void Update(GameTime gameTime, TransferableObjectData remoteData)
        {
            //Position = remoteData.Position;
            base.Update(gameTime, remoteData);
        }
    }
}
