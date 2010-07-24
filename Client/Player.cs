using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;

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
        public event EventHandler PlayerUpdated = delegate { };

        private KeyboardControls Controls { get; set; }
        private int rotationSpeed = 5;

        public Player(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, short index, KeyboardControls controls) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed)
        {
            Index = index;
            Controls = controls;
        }

        public void Move(MoveDirection direction)
        {
            Position += Velocity * (int)direction;
        }

        public void Rotate(float angle)
        {
            Angle += angle;
        }

        public override void Update(GameTime gameTime)
        {
            bool playerUpdated = false;
            if (InputKeys.Contains(Controls.Forward))
            {
                Move(MoveDirection.Forward);
                playerUpdated = true;
            }

            if (InputKeys.Contains(Controls.Backward))
            {
                Move(MoveDirection.Backward);
                playerUpdated = true;
            }

            if (InputKeys.Contains(Controls.RotateLeft))
            {
                Rotate(-MathHelper.ToRadians(rotationSpeed));
                playerUpdated = true;
            }

            if (InputKeys.Contains(Controls.RotateRight))
            {
                Rotate(MathHelper.ToRadians(rotationSpeed));
                playerUpdated = true;
            }

            /*if (InputKeys.Contains(Controls.Shoot))
            {
                Fire(gameTime);
            }

            Projectiles.ForEach(p => p.Update(gameTime));
            for (int i = 0; i < Projectiles.Count; i++)
            {
                if (!Projectiles[i].IsActive)
                {
                    //Projectiles.Remove(Projectiles[i]);
                }
            }*/

            if (playerUpdated)
            {
                PlayerUpdated(this, EventArgs.Empty);
            }
            base.Update(gameTime);
        }
    }
}
