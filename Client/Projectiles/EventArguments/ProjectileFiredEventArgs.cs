using System;
using Client.Projectiles;

namespace Client
{
    class ProjectileFiredEventArgs : EventArgs
    {
        public ProjectileLocal Projectile { get; set; }

        public ProjectileFiredEventArgs(ProjectileLocal projectile)
        {
            Projectile = projectile;
        }
    }
}
