using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    class RemotePlayer:RemoteObject
    {
        private const short CorrectionThreshold = 3;
        private const float InterpolationConstant = 0.2f;

        public RemotePlayer(Game game, PhysicsSimulator physicsSimulator, long sessionID, int id, string imageAssetPath, Vector2 initialPosition, float initialAngle, float zOrder, float mass, float speed, TransferableObjectData remoteData) : base(game, physicsSimulator, sessionID, id, imageAssetPath, initialPosition, initialAngle, zOrder, mass, speed, remoteData)
        {
        }

        public override void Update(GameTime gametime)
        {
            Position = UpdatePosition();
            Angle = RemoteData.Angle;
        }

        Vector2 UpdatePosition()
        {
            var difference = RemoteData.Position - Position;
            float newX = difference.X * InterpolationConstant, newY = difference.Y * InterpolationConstant;
            if (difference.X > 0 && difference.X < CorrectionThreshold)
            {
                newX = difference.X;
            }

            if (difference.Y > 0 && difference.Y < CorrectionThreshold)
            {
                newY = difference.Y;
                if (difference.X > 0 && difference.X < CorrectionThreshold)
                {
                    newX = difference.X;
                }

                if (difference.Y > 0 && difference.Y < CorrectionThreshold)
                {
                    newY = difference.Y;
                }
            }
            return Position + new Vector2(newX, newY);
        }
    }
}
