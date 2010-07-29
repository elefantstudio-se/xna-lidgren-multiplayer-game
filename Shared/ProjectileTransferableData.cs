using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shared
{
    public class ProjectileTransferableData:ITransferable
    {
        public long SessionID { get; set; }

        public int ID { get; set; }
        public bool IsValid{ get; set; }
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
    }
}
