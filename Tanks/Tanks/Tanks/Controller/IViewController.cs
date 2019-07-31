using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Controller
{
    public interface IViewController
    {
        bool ActiveTimer { get; set; }

        int MapWidth { get; }
        int MapHeight { get; }

        int FormWidth { get; }

        void Render(bool isGame = true);
    }
}
