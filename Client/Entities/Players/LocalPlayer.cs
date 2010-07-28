using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Projectiles;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Players
{
    class LocalPlayer:Player
    {
        public enum MoveDirection
        {
            Forward = 1,
            Backward = -1
        }

        public LocalPlayer(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories, short index, KeyboardControls controls, ProjectileFactory projectileFactory) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories, index)
        {
            Controls = controls;
            this.projectileFactory = projectileFactory;
            Projectiles = new List<ProjectileLocal>();
            Geometry.OnCollision += OnCollision;
            Body.LinearDragCoefficient = 100;
            Body.RotationalDragCoefficient = 6000;
        }

        public List<ProjectileLocal> Projectiles{ get; set;}

        private KeyboardControls Controls { get; set; }
        private int rotationSpeed = 5;
        private double lastShotFired;
        private double fireInterval = 300;
        private readonly ProjectileFactory projectileFactory;

        bool OnCollision(Geom geom1, Geom geom2, ContactList contactList)
        {
            return true;
        }

        public void Move(MoveDirection direction)
        {
            //Position += Velocity * (int)direction;
            Body.ApplyImpulse(Velocity * Speed * (int)direction);
        }

        public void Rotate(float angle)
        {
            Angle += angle;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputKeys.Contains(Controls.Forward))
            {
                Move(MoveDirection.Forward);
            }

            if (InputKeys.Contains(Controls.Backward))
            {
                Move(MoveDirection.Backward);
            }

            if (InputKeys.Contains(Controls.RotateLeft))
            {
                Rotate(-MathHelper.ToRadians(rotationSpeed));
            }

            if (InputKeys.Contains(Controls.RotateRight))
            {
                Rotate(MathHelper.ToRadians(rotationSpeed));
            }

            if (InputKeys.Contains(Controls.Shoot))
            {
                Fire(gameTime);
            }

            Projectiles.ForEach(p => p.Update(gameTime));
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Projectiles.ForEach(p => { Console.WriteLine(DateTime.Now.Millisecond + ": drawing"); p.Draw(gameTime); });
        }

        bool Fire(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalMilliseconds;
            if (now - lastShotFired < fireInterval)
            {
                return false;
            }
            lastShotFired = now;
            ProjectileLocal newProjectile = projectileFactory.NewProjectile(SessionID, Helpers.GetNewID(), Index, Position, Angle);
            newProjectile.Fire();
            Projectiles.Add(newProjectile);
            return true;
        }

        public void RemoveProjectile(ProjectileLocal projectile)
        {
            Projectiles.Remove(projectile);
        }
    }
}
