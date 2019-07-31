using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class BoomViewModel : Boom
    {
        public Sprite Sprite { get; private set; }
        public BoomViewModel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Sprite = new Sprite(0, 64, width, height, 3, 12);
        }

        public void Draw(Graphics g)
        {
            Sprite.Draw(g, X, Y);
        }

        public void SetSprite(float dx)
        {
            Sprite.SetFrame(dx);
        }

        public bool EndAnimation()
        {
            return Sprite.EndAnimation;
        }

    }
}
