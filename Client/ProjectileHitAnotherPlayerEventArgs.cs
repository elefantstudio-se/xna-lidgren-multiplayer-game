using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ProjectileHitAnotherPlayerEventArgs:EventArgs
    {
        public ObjectData OtherPlayer { get; set; }
        public ProjectileHitAnotherPlayerEventArgs(ObjectData otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
