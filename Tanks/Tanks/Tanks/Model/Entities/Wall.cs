using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class Wall : StaticEntity
    {
        public bool Destroyable { get; private set; }
        public Wall(int x, int y, int width, int height, bool destroyable) : base(x, y, width, height)
        {
            Destroyable = destroyable;
        }
    }
}
