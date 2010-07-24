using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

namespace Client
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

        public Projectile NewProjectile(long sessionID, int id, short index, Vector2 position, float angle)
        {
            return new Projectile(game, physicsSimulator, sessionID, id, textureFolder + textureNames[index], position, angle, zOrder, mass, speed, CollisionCategory.Cat3);
        }

        public RemoteProjectile NewRemoteProjectile(long sessionID, int id, short index, Vector2 position, float angle)
        {
            return new RemoteProjectile(game, physicsSimulator, sessionID, id, textureFolder + textureNames[index], position, angle, zOrder, mass, speed, CollisionCategory.Cat4);
        }
    }
}
