using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Client
{
    interface IDrawable
    {
        Vector2 Position { get; set; }
        float Angle { get; set; }

        void Draw(GameTime gametime);
    }
}
