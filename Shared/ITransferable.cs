using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public interface ITransferable
    {
        int ID { get; set; }
        bool IsValid { get; set; }
    }
}
