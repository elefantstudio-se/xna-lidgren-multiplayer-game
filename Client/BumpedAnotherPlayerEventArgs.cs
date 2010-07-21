using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    class BumpedAnotherPlayerEventArgs : EventArgs
    {
        public ObjectData OtherPlayer { get; set; }

        public BumpedAnotherPlayerEventArgs(ObjectData otherPlayer)
        {
            OtherPlayer = otherPlayer;
        }
    }
}
