using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Model.Entities;
using Tanks.Model.EntitiesViewModel;

namespace Tanks.Model
{
    public class GameEngine
    {


        public static bool IsColllision(BaseEntity entity1, BaseEntity entity2)
        {
            return (entity1.X + entity1.Width > entity2.X) &&
                   (entity1.X <= entity2.X + entity2.Width) &&
                   (entity1.Y + entity1.Height > entity2.Y) &&
                   (entity1.Y <= entity2.Y + entity2.Height);
        }
    }
}
