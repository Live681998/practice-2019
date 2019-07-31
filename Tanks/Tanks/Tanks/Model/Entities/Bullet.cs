using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class Bullet : DynamicEntity
    {
        public BaseEntity Shooter { get; private set; }
        public Bullet(int x, int y, int width, int height, Direction dir, BaseEntity shooter) : base(x, y, width, height, dir)
        {
            Shooter = shooter;
        }

        public override void Move(float dt)
        {
            base.Move(3*dt);
        }
    }
}
