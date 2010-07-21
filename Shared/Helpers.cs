using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
    public static class Helpers
    {
        public static int GetNewID()
        {
            return Convert.ToInt32(DateTime.Now.Ticks.ToString().Substring(0, 4));
        }
    }
}
