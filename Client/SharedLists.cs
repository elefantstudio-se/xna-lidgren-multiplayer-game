using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    static class SharedLists
    {
        public static Dictionary<int, ObjectData> Players { get; set; }
        public static Dictionary<int, ObjectData> Projectiles { get; set; }
        public static string[] PlayerTextureNames = new[] {"p1", "p2", "p3", "p4"};
        public static string[] ProjectileTextureNames = new[] {"p1", "p2", "p3", "p4"};
    }
}
