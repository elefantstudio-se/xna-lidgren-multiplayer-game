using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    interface IUpdateable
    {
        Vector2 UpdatePosition(Vector2 local, Vector2 remote);
        float UpdateAngle(float local, float remote);
        bool IsStillValid(Rectangle bounds);
    }
}
