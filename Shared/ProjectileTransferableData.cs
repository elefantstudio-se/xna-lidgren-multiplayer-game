using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Shared
{
    public class ProjectileTransferableData : ITransferable
    {
        public long SessionID { get; set; }

        public int ID { get; set; }
        public bool IsValid { get; set; }
        public NetDeliveryMethod DeliveryMethod
        {
            get { return NetDeliveryMethod.UnreliableSequenced; }
        }

        public void WriteToMessage(NetOutgoingMessage message)
        {
            message.Write(SessionID);
            message.Write(ID);
            message.Write(IsValid);
            message.Write(Position);
            message.Write(Angle);
        }

        public Vector2 Position { get; set; }
        public float Angle { get; set; }

        public ProjectileTransferableData(long sessionID, int id, bool isValid, Vector2 position, float angle)
        {
            SessionID = sessionID;
            ID = id;
            IsValid = isValid;
            Position = position;
            Angle = angle;
        }

        public ProjectileTransferableData(NetIncomingMessage message)
        {
            SessionID = message.ReadInt64();
            ID = message.ReadInt32();
            IsValid = message.ReadBoolean();
            Position = message.ReadVector2();
            Angle = message.ReadFloat();
        }
    }
}
