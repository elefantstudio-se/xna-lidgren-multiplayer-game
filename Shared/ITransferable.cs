using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public interface ITransferable
    {
        long SessionID { get; set; }
        int ID { get; set; }
        bool IsValid { get; set; }
    }
}
