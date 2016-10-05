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
        #region Attributes
        public List<NodeOSM> nodes = new List<NodeOSM>();
        public List<RoadSegmentOSM> segments = new List<RoadSegmentOSM>();

        #endregion


        #region Constructor
        public RoadNetwork()
        {

        }
        #endregion


    }
}
