using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;

namespace Shared
{
    public class ClientDisconnectedTransferableData:ITransferable
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public bool IsValid { get; set; }
        public short PlayerIndex { get; set; }

        public NetDeliveryMethod DeliveryMethod
        {
            get { return NetDeliveryMethod.ReliableOrdered; }
        }

        public void WriteToMessage(NetOutgoingMessage message)
        {
            message.Write(SessionID);
            message.Write(ID);
            message.Write(IsValid);
            message.Write(PlayerIndex);
        }

        public ClientDisconnectedTransferableData(NetIncomingMessage message)
        {
            SessionID = message.ReadInt64();
            ID = message.ReadInt32();
            IsValid = message.ReadBoolean();
            PlayerIndex = message.ReadInt16();
        }

        public ClientDisconnectedTransferableData(long sessionID, short playerIndex)
        {
            SessionID = sessionID;
            PlayerIndex = playerIndex;
        }
    }
}
