using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
