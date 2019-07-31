using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using System.Drawing;

namespace Tanks.Model.EntitiesViewModel
{
    public class BulletViewModel : Bullet
    {
        public Sprite Sprite { get; private set; }
        public BulletViewModel(int x, int y, int width, int height, Direction dir, BaseEntity shooter) : base(x, y, width, height, dir, shooter)
        {
            Sprite = new Sprite(0, 159, width, height);
        }

        public void Draw(Graphics g)
        {
            Sprite.Draw(g, X, Y);
        }
    }
}
