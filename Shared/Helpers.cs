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
            var n = DateTime.Now.Ticks.ToString();
            return Convert.ToInt32(n.Substring(n.Length - 4,4));
        }
    }
}
