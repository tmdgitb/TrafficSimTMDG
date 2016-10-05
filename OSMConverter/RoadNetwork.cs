using SimTMDG.Road;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMConverter
{
    [Serializable]
    public class RoadNetwork
    {
        public List<NodeOSM> nodes = new List<NodeOSM>();
        public List<RoadSegmentOSM> segments = new List<RoadSegmentOSM>();
    }
}
