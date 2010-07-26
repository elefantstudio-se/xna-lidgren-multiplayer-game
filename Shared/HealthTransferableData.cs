using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public class HealthTransferableData:ITransferable
    {
        public int ID { get; set; }
        public bool IsValid{ get; set;}
        public float Value { get; set; }

        public HealthTransferableData(int id, bool isValid, float value)
        {
            ID = id;
            IsValid = isValid;
            Value = value;
        }
    }
}
