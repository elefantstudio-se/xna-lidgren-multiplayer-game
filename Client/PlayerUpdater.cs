using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    class PlayerUpdater : IUpdateable
    {
        private const short CorrectionThreshold = 3;
        private const float InterpolationConstant = 0.2f;

        public Vector2 UpdatePosition(Vector2 local, Vector2 remote)
        {
            var difference = remote - local;
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
            return local + new Vector2(newX, newY);
        }

        public float UpdateAngle(float local, float remote)
        {
            return remote;
        }
    }
}
