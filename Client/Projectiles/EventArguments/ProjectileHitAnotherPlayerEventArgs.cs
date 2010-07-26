using System;
using Client.Players;

namespace Client
{
    class ProjectileHitPlayerEventArgs:EventArgs
    {
        public RemotePlayer OtherPlayer { get; set; }
        public ProjectileHitPlayerEventArgs(RemotePlayer otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
