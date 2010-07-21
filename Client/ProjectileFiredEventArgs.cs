using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ProjectileFiredEventArgs : EventArgs
    {
        public Projectile Projectile { get; set; }

        public ProjectileFiredEventArgs(Projectile projectile)
        {
            Projectile = projectile;
        }
    }
}
