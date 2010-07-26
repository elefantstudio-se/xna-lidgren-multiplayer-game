using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Players;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client.Players
{
    class RemotePlayer_o:Player_o
    {
        private const short DestinationThreshold = 1;
        private const float InterpolationConstant = 0.2f;

        public RemotePlayer_o(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, CollisionCategory collisionCategories) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, collisionCategories)
        {
        }

        public override void Update(GameTime gametime, TransferableObjectData remoteData)
        {
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
    }
}
