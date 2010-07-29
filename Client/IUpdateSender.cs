using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Shared;

namespace Client
{
    interface IUpdateSender
    {
        void SendUpdates(NetClient client);
    }
}
