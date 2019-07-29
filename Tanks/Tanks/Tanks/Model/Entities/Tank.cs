using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class Tank : DynamicEntity
    {
        public Tank(int x, int y, int width, int height, Direction dir) : base(x, y, width, height, dir)
        {
        }
    }
}
