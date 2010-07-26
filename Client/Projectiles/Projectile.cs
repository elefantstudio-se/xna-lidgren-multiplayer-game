using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    abstract class Projectile:PhysicsGameObject<TransferableObjectData>
    {
        protected Projectile(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath, position, physicsSimulator, speed, mass, collisionCategories)
        {
        }
    }
}
