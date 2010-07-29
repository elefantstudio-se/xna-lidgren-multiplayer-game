using Client.Players;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

namespace Client.Factories
{
    class PlayerFactory
    {
        private Game game;
        private PhysicsSimulator physicsSimulator;
        private float zOrder, mass, speed;
        private string textureFolder;
        private string[] textureNames;
        ProjectileFactory projectileFactory;

        public PlayerFactory(Game game, PhysicsSimulator physicsSimulator, float zOrder, float mass, float speed, string textureFolder, string[] textureNames, ProjectileFactory projectileFactory)
        {
            this.game = game;
            this.physicsSimulator = physicsSimulator;
            this.zOrder = zOrder;
            this.mass = mass;
            this.speed = speed;
            this.textureFolder = textureFolder;
            this.textureNames = textureNames;
            this.projectileFactory = projectileFactory;
        }

        public LocalPlayer NewPlayer(long sessionID, int id, short index, Vector2 position, float angle, KeyboardControls controls)
        {
            return new LocalPlayer(game, sessionID, id, textureFolder + textureNames[index], position, angle, physicsSimulator, speed, mass, CollisionCategory.Cat1, index, controls, projectileFactory);
        }

        public PlayerRemote NewRemotePlayer(long sessionID, int id, short index, Vector2 position, float angle)
        {
            return new PlayerRemote(game, sessionID, id, textureFolder + textureNames[index], position, angle, physicsSimulator, speed, mass, CollisionCategory.Cat1, index);
        }
    }
}