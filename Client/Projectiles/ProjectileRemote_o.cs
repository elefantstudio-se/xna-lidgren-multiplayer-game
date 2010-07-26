using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Projectiles
{
    class ProjectileRemote_o:Projectile_o
    {
        public ProjectileRemote_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
            //Body.ApplyImpulse(Velocity * Speed);
            //Geometry.OnCollision = (g1, g2, l) => false;
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
