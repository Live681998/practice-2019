using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Model.Entities
{
    public class DynamicEntity : BaseEntity
    {
        public static int speed = 100;
        public Direction Direction;

        public DynamicEntity(int x, int y, int width, int height, Direction dir)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Direction = dir;
        }

        public void ChangeDirection()
        {
            if (Direction == Direction.RIGHT)
            {
                Direction = Direction.LEFT;
            }
            else if (Direction == Direction.UP)
            {
                Direction = Direction.DOWN;
            }
            else if (Direction == Direction.LEFT)
            {
                Direction = Direction.RIGHT;
            }
            else if (Direction == Direction.DOWN)
            {
                Direction = Direction.UP;
            }
        }

        public void ChangeDirection(Direction dir)
        {
            Direction = dir;
        }

        virtual public void Move(float dt)
        {
            if (Direction == Direction.UP)
            {
                Y -= Convert.ToInt32(speed * dt);
            }
            else if (Direction == Direction.RIGHT)
            {
                X += Convert.ToInt32(speed * dt);
            }
            else if (Direction == Direction.DOWN)
            {
                Y += Convert.ToInt32(speed * dt);
            }
            else
            {
                X -= Convert.ToInt32(speed * dt);
            }
        }

        public DynamicEntity Clone()
        {
            return new DynamicEntity(X, Y, Width, Height, Direction);
        }

        
    }
}
