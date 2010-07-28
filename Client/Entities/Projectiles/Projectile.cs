using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    abstract class Projectile:PhysicsGameObject
    {
        protected Projectile(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories)
        {
        }
    }
}
