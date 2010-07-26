using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    abstract class Projectile_o:GameObject<TransferableObjectData>
    {
        protected Projectile_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
        }
    }
}
