using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shared
{
    public class TransferableObjectData
    {
        public long SessionID { get; set; }
        public int ID { get; set; }
        public short Index { get; set; }
        public Vector2 Position { get; set; }
        public float Angle { get; set; }

        public TransferableObjectData(long sessionId, int id, short index, Vector2 position, float angle)
        {
            SessionID = sessionId;
            ID = id;
            Index = index;
            Position = position;
            Angle = angle;
        }
    }
}
