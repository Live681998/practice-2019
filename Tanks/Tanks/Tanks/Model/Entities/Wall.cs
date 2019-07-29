using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class Wall : StaticEntity
    {
        public Wall(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
}
