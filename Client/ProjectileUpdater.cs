using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    class ProjectileUpdater:IUpdateable
    {
        public Vector2 UpdatePosition(Vector2 local, Vector2 remote)
        {
            return remote;
        }

        public float UpdateAngle(float local, float remote)
        {
            return remote;
        }
    }
}
