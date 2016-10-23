using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OSMConverter
{
    public class SegmentLaneOSM
    {
        #region Attributes

        #region ID
        /// <summary>
        /// WaySegment ID
        /// </summary>

        private static int idIndex = 0;
        private int _id = -1;

        public int Id
        {
            get { return _id; }

            set { _id = value; }
        }

        public int LaneIdx = 0;
        #endregion

        #region primary properties
        /// <summary>
        /// Starting node of WaySegment
        /// </summary>
        [XmlIgnore]
        public NodeOSM startNode;

        /// <summary>
        /// Ending node of WaySegment
        /// </summary>
        [XmlIgnore]
        public NodeOSM endNode;

        public int startNodeIdx;
        public int endNodeIdx;

        /// <summary>
        /// WaySegment length from start node to end node
        /// </summary>
        [XmlIgnore]
        private double _length;
        [XmlIgnore]
        public double Length
        {
            get { return _length; }

            set { _length = value; }
        }
        #endregion

        /// <summary>
        /// WaySegment target average speed
        /// </summary>
        [XmlIgnore]
        private int _avgSpeed;
        [XmlIgnore]
        public int AvgSpeed
        {
            get { return _avgSpeed; }

            set { _avgSpeed = value; }
        }

        /// <summary>
        /// Way highway key / properties
        /// </summary>
        [XmlIgnore]
        private string _highway;
        [XmlIgnore]
        public string Highway
        {
            get { return _highway; }

            set { _highway = value; }
        }

        /// <summary>
        /// Way / Road name
        /// </summary>
        [XmlIgnore]
        private string _roadName;
        [XmlIgnore]
        public string RoadName
        {
            get { return _roadName; }

            set { _roadName = value; }
        }
        #endregion




        #region Constructor
        public SegmentLaneOSM()
        {
            _id = idIndex++;
        }

        public SegmentLaneOSM(NodeOSM _startNode, NodeOSM _endNode, int laneIdx)
        {
            _id = idIndex++;
            LaneIdx = laneIdx;
            startNode = _startNode;
            endNode = _endNode;
            _length = Vector2.GetDistance(startNode.Position, endNode.Position);

            Vector2 difference = new Vector2();
            difference.X = endNode.Position.X - startNode.Position.X;
            difference.Y = endNode.Position.Y - startNode.Position.Y;
        }
        #endregion



        #region Methods
        #endregion
    }
}
