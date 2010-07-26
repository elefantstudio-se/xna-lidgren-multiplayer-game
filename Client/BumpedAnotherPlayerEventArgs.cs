using System;
using Client.Players;

namespace Client
{
    class BumpedAnotherPlayerEventArgs : EventArgs
    {
        public RemotePlayer OtherPlayer { get; set; }

        public BumpedAnotherPlayerEventArgs(RemotePlayer otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
