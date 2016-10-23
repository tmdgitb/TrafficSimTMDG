using SimTMDG.Road;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OSMConverter
{
    [Serializable]
    public class RoadNetwork
    {
        #region Attributes
        [XmlAttribute("minlon")]
        public double minLon;
        [XmlAttribute("maxlon")]
        public double maxLon;
        [XmlAttribute("minlat")]
        public double minLat;
        [XmlAttribute("maxlat")]
        public double maxLat;


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
