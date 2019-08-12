using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class TankViewModel : Tank
    {
        public Sprite Sprite { get; private set; }
        public TankViewModel(int x, int y, int width, int height, Direction dir) : base(x, y, width, height, dir)
        {
            Sprite = new Sprite((int)dir * width, 0, width, height, 2, 8);
        }

        public void Draw(Graphics g)
        {
            Sprite.SetPosition((int)Direction * Width, 0);
            Sprite.Draw(g, X, Y);
        }
    }
}
