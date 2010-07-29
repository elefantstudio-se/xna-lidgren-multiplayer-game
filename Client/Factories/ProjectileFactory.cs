using Client.Projectiles;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

namespace Client.Factories
{
    class ProjectileFactory
    {
        private Game game;
        private PhysicsSimulator physicsSimulator;
        private float zOrder, mass, speed;
        private string textureFolder;
        private string[] textureNames;

        public ProjectileFactory(Game game, PhysicsSimulator physicsSimulator, float zOrder, float mass, float speed, string textureFolder, string[] textureNames)
        {
            this.game = game;
            this.physicsSimulator = physicsSimulator;
            this.zOrder = zOrder;
            this.mass = mass;
            this.speed = speed;
            this.textureFolder = textureFolder;
            this.textureNames = textureNames;
        }

        public ProjectileLocal NewProjectile(long sessionID, int id, short index, Vector2 position, float angle)
        {
            return new ProjectileLocal(game, sessionID, id, textureFolder + textureNames[index], position, angle, physicsSimulator, speed, mass, CollisionCategory.Cat3);
        }

        public ProjectileRemote NewRemoteProjectile(long sessionID, int id, short index, Vector2 position, float angle)
        {
            return new ProjectileRemote(game, sessionID, id, textureFolder + textureNames[index], position, angle, physicsSimulator, speed, mass, CollisionCategory.Cat3);
        }
    }
}