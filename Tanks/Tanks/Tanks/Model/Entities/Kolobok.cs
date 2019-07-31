using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class Kolobok : DynamicEntity, IShootingEntity
    {
        public Kolobok(int x, int y, int width, int height, Direction dir) : base(x, y, width, height, dir)
        {
        }

        public float Recharge { get; set; }
    }
}
