using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model
{
    public class Log
    {
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        

        public Log(string name, int x, int y)
        {
            X = x;
            Y = y;
            Name = name;
        }
    }
}
