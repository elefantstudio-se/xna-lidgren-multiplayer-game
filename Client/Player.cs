using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class Player:GameObject
    {
        public enum MoveDirection
        {
            Forward = 1,
            Backward = -1
        }
        public short Index { get; set; }
        public List<Projectile> Projectiles{ get; set;}

        private KeyboardControls Controls { get; set; }
        private int rotationSpeed = 5;
        private double lastShotFired;
        private double fireInterval = 300;
        private ProjectileFactory projectileFactory;

        public Player(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, short index, KeyboardControls controls, ProjectileFactory projectileFactory, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
            Index = index;
            Controls = controls;
            this.projectileFactory = projectileFactory;
            Projectiles = new List<Projectile>();
            Geometry.OnCollision += OnCollision;
            Body.LinearDragCoefficient = 100;
            Body.RotationalDragCoefficient = 6000;
        }

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

        public override void Update(GameTime gameTime, TransferableObjectData remoteData)
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

            Projectiles.ForEach(p => p.Update(gameTime, null));
            /*
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (!Projectiles[i].IsActive)
                {
                    //Projectiles.Remove(Projectiles[i]);
                }
            }*/
            base.Update(gameTime, null);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Projectiles.ForEach(p => p.Draw(gameTime));
        }

        bool Fire(GameTime gameTime)
        {
            double now = gameTime.TotalGameTime.TotalMilliseconds;
            if (now - lastShotFired < fireInterval)
            {
                return false;
            }
            lastShotFired = now;
            Projectile newProjectile = projectileFactory.NewProjectile(SessionID, Helpers.GetNewID(), Index, Position, Angle);
            Projectiles.Add(newProjectile);
            return true;
        }
    }
}
