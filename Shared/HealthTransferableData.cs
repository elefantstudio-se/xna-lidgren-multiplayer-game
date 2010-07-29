using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class HealthTransferableData:ITransferable
    {

        public long SessionID { get; set; }
        public int ID { get; set; }
        public bool IsValid{ get; set;}
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
    }
}
