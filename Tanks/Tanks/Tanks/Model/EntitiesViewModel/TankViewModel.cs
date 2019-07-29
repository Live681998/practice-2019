using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;

namespace Tanks.Model.EntitiesViewModel
{
    public class TankViewModel : Tank
    {
        public TankViewModel(int x, int y, int width, int height, Direction dir) : base(x, y, width, height, dir)
        {
        }
    }
}
