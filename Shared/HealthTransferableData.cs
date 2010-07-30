using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
namespace Shared
{
    public class HealthTransferableData:ITransferable
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public bool IsValid{ get; set;}
        public NetDeliveryMethod DeliveryMethod
        {
            get { return NetDeliveryMethod.UnreliableSequenced; }
        }

        public void WriteToMessage(NetOutgoingMessage message)
        {
            message.Write(SessionID);
            message.Write(ID);
            message.Write(IsValid);
            message.Write(PlayerIndex);
            message.Write(Value);
        }

        public int PlayerIndex { get; set; }
        public float Value { get; set; }

        public HealthTransferableData(long sessionID, int id, bool isValid, int playerIndex, float value)
        {
            SessionID = sessionID;
            ID = id;
            IsValid = isValid;
            PlayerIndex = playerIndex;
            Value = value;
        }

        public HealthTransferableData(NetIncomingMessage message)
        {
            SessionID = message.ReadInt64();
            ID = message.ReadInt32();
            IsValid = message.ReadBoolean();
            PlayerIndex = message.ReadInt32();
            Value = message.ReadFloat();
        }
    }
}
