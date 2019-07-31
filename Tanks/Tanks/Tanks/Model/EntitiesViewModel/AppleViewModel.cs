using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class AppleViewModel : Apple
    {
        public Sprite Sprite { get; private set; }
        public AppleViewModel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Sprite = new Sprite(0, 143, width, height);
        }

        public void Draw(Graphics g)
        {
            Sprite.Draw(g, X, Y);
        }
    }
}
