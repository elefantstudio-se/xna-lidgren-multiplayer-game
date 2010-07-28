using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
    interface IRemotelyUpdateable
    {
        void Update(GameTime gameTime, ITransferable remoteData);
    }
}
