using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class WallViewModel : Wall
    {
        public Sprite Sprite { get; private set; }
        public WallViewModel(int x, int y, int width, int height, bool destroyable) : base(x, y, width, height, destroyable)
        {
            if (destroyable)
            {
                Sprite = new Sprite(0, 16, width, height);
            }
            else
            {
                Sprite = new Sprite(0, 32, width, height);
            }
        }
        public void Draw(Graphics g)
        {
            Sprite.Draw(g, X, Y);
        }
    }
}
