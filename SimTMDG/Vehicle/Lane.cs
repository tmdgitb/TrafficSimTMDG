using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    static class Lane
    {
        public const int NO_CHANGE = 0;
        public const int TO_LEFT = -1;
        public const int TO_RIGHT = 1;


        public const int NONE = -2;
        public const int LANE1 = 1;
        public const int LANE2 = 2;
        public const int LANE3 = 3;
        public const int LANE4 = 4;
        public const int LANE5 = 5;

        public const int MOST_INNER = LANE1;
    }
}
