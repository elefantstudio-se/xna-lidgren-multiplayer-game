using System;

namespace Client.Players.EventArguments
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