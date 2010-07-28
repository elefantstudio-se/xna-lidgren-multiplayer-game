using System;
using Client.Players;

namespace Client
{
    class ProjectileHitPlayerEventArgs:EventArgs
    {
        public PlayerRemote OtherPlayer { get; set; }
        public ProjectileHitPlayerEventArgs(PlayerRemote otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
