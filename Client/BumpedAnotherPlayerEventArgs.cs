using System;
using Client.Players;

namespace Client
{
    class BumpedAnotherPlayerEventArgs : EventArgs
    {
        public PlayerRemote OtherPlayer { get; set; }

        public BumpedAnotherPlayerEventArgs(PlayerRemote otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
