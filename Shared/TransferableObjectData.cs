using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Shared
{
    public class TransferableObjectData:ITransferable
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public short Index { get; set; }
        public Vector2 Position { get; set; }
        public float Angle { get; set; }
        public bool IsValid { get; set; }
        public NetDeliveryMethod DeliveryMethod
        {
            get { return NetDeliveryMethod.UnreliableSequenced;}
        }

        public void WriteToMessage(NetOutgoingMessage message)
        {
            message.Write(this);
        }

        public TransferableObjectData(long sessionId, int id, short index, Vector2 position, float angle, bool isValid)
        {
            SessionID = sessionId;
            ID = id;
            Index = index;
            Position = position;
            Angle = angle;
            IsValid = isValid;
        }
    }
}
