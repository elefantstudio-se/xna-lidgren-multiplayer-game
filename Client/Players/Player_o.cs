using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Players
{
    abstract class Player_o:GameObject<TransferableObjectData>
    {
        protected Player_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
        }
    }
}
