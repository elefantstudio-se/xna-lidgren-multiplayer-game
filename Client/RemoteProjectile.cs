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
            //Geometry.OnCollision = (g1, g2, l) => false;
        }

        public override void Update(GameTime gameTime, TransferableObjectData remoteData)
        {
            if (remoteData == null)
            {
                //Console.WriteLine(DateTime.Now.Millisecond + "; returning null");
                return;
            }
            //if (!IsInScreen)
            //{
            //    IsValid = false;
            //}
            Position = remoteData.Position;
            //Console.WriteLine(DateTime.Now.Millisecond + ":  " + Position);
            base.Update(gameTime, remoteData);
        }
    }
}
