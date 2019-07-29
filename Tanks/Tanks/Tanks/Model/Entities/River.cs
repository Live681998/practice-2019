using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class River : StaticEntity
    {
        public River(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
}
