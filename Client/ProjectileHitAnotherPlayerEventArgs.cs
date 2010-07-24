using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class ProjectileHitAnotherPlayerEventArgs:EventArgs
    {
        public RemotePlayer OtherPlayer { get; set; }
        public ProjectileHitAnotherPlayerEventArgs(RemotePlayer otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
