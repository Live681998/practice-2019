using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class KolobokViewModel : Kolobok
    {
        public Sprite Sprite { get; private set; }
        public KolobokViewModel(int x, int y, int width, int height, Direction dir) : base(x, y, width, height, dir)
        {
            Sprite = new Sprite(0, 83, width, height, 2, 8);
        }

        public void SetSprite(float dx)
        {
            Sprite.SetFrame(dx);
        }

        public void Draw(Graphics g)
        {
            Sprite.SetPosition(0, 83 + (int)Direction * Height);
            Sprite.Draw(g, X, Y);
        }


    }
}
