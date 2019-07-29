using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;

namespace Tanks.Model.EntitiesViewModel
{
    public class WallViewModel : Wall
    {
        public WallViewModel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
    }
}
