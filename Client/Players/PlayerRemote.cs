using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Players
{
    class PlayerRemote:Player
    {
        private const short DestinationThreshold = 1;
        private const float InterpolationConstant = 0.2f;

        public PlayerRemote(Game game, long sessionID, int id, string imageAssetPath, Vector2 position, float angle, PhysicsSimulator physicsSimulator, float speed, float mass, CollisionCategory collisionCategories, short index) : base(game, sessionID, id, imageAssetPath, position, angle, physicsSimulator, speed, mass, collisionCategories, index)
        {
        }

        public override void Update(GameTime gametime, TransferableObjectData remoteData)
        {
            Console.WriteLine(remoteData.Position);
            UpdatePosition(remoteData);
            UpdateAngle(remoteData);
        }

        void UpdatePosition(TransferableObjectData remoteData)
        {
            if (remoteData == null || Position == remoteData.Position)
            {
                return;
            }
            Position = remoteData.Position;
            /*
            var difference = remoteData.Position - Position;
            float newX = Position.X, newY = Position.Y;
            if (difference.X < DestinationThreshold)
            {
                newX = remoteData.Position.X;
            }
            if (difference.Y < DestinationThreshold)
            {
                newY = remoteData.Position.Y;
            }
            Vector2 newPosition = new Vector2(newX, newY);
            if (newPosition != remoteData.Position) //we haven't arrived yet
            {
                Body.ApplyImpulse(Velocity * Speed);
            } else
            {
                Body.Position = newPosition;
            }
            */
 

            /*
            var difference = remoteData.Position - Position;
            float newX = difference.X * InterpolationConstant, newY = difference.Y * InterpolationConstant;
            if (difference.X > 0 && difference.X < DestinationThreshold)
            {
                newX = difference.X;
            }

            if (difference.Y > 0 && difference.Y < DestinationThreshold)
            {
                newY = difference.Y;
                if (difference.X > 0 && difference.X < DestinationThreshold)
                {
                    newX = difference.X;
                }

                if (difference.Y > 0 && difference.Y < DestinationThreshold)
                {
                    newY = difference.Y;
                }
            }
            Position += new Vector2(newX, newY);
            */
        }

        void UpdateAngle(TransferableObjectData remoteData)
        {
            if (remoteData == null)
            {
                return;
            }
            Angle = remoteData.Angle;
        }
        public override void Draw(GameTime gametime)
        {
            Console.WriteLine(Position);
            base.Draw(gametime);
        }
    }
}
