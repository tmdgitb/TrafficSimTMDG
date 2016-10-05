using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMConverter
{
    [Serializable]
    public class RoadSegmentOSM
    {

        #region Attributes

        #region ID
        /// <summary>
        /// RoadSegment ID
        /// </summary>

        private static int idIndex = 0;
        private int _id = -1;

        public int Id
        {
            get { return _id; }

            set { _id = value; }
        }
        #endregion


        #region primary properties
        /// <summary>
        /// Starting node of RoadSegment
        /// </summary>
        public NodeOSM startNode;

        /// <summary>
        /// Ending node of RoadSegment
        /// </summary>
        public NodeOSM endNode;


        /// <summary>
        /// RoadSegment length from start node to end node
        /// </summary>
        private double _length;

        public double Length
        {
            get { return _length; }

            set { _length = value; }
        }

        
        /// <summary>
        /// List of lane in this RoadSegment
        /// </summary>
        public List<SegmentLaneOSM> lanes = new List<SegmentLaneOSM>();

        /// <summary>
        /// The width of each lane in this RoadSegment
        /// </summary>
        public double laneWidth = 3.5;


        /// <summary>
        /// Previous RoadSegment that connect to this startNode
        /// </summary>
        public List<RoadSegmentOSM> nextSegment = new List<RoadSegmentOSM>();

        /// <summary>
        /// Next RoadSegment that connect to this endNode
        /// </summary>
        public List<RoadSegmentOSM> prevSegment = new List<RoadSegmentOSM>();
        #endregion


        #region secondary properties
        /// <summary>
        /// WaySegment target average speed
        /// </summary>
        private int _avgSpeed;

        public int AvgSpeed
        {
            get { return _avgSpeed; }

            set { _avgSpeed = value; }
        }

        /// <summary>
        /// Way highway key / properties
        /// </summary>
        private string _highway;

        public string Highway
        {
            get { return _highway; }

            set { _highway = value; }
        }

        /// <summary>
        /// Oneway / Reverse / Bi-way
        /// </summary>
        private string _oneWay;

        public string OneWay
        {
            get { return _oneWay; }

            set { _oneWay = value; }
        }

        /// <summary>
        /// Way / Road name
        /// </summary>
        private string _roadName;

        public string RoadName
        {
            get { return _roadName; }

            set { _roadName = value; }
        }

        /// <summary>
        /// Number of lanes
        /// </summary>
        private int _numLanes;
        public int NumLanes
        {
            get { return _numLanes; }

            set { _numLanes = value; }
        }
        #endregion


        #endregion



        #region Constructor
        public RoadSegmentOSM()
        {
            _id = idIndex++;
        }

        public RoadSegmentOSM(NodeOSM _startNode, NodeOSM _endNode, int numlanes, string highway, string oneway)
        {
            _id = idIndex++;
            OneWay = oneway;
            startNode = _startNode;
            endNode = _endNode;
            _length = Vector2.GetDistance(startNode.Position, endNode.Position);

            Vector2 difference = new Vector2();
            difference.X = endNode.Position.X - startNode.Position.X;
            difference.Y = endNode.Position.Y - startNode.Position.Y;

            _highway = highway;

            // Num lanes assumptions based on: http://wiki.openstreetmap.org/wiki/Key:lanes#Assumptions
            if ((_highway == "motorway") || (_highway == "trunk"))
            {
                if ((oneway == "true") || (oneway == "-1"))
                {
                    _numLanes = 2;
                }
                else
                {
                    _numLanes = 4;
                }
            }
            else if ((_highway == "primary") || (_highway == "secondary")
               || (_highway == "tertiary") || (_highway == "residential"))
            {
                if ((oneway == "true") || (oneway == "-1"))
                {
                    _numLanes = 1;
                }
                else
                {
                    _numLanes = 2;
                }
            }
            else
            {
                _numLanes = 1;
            }

            // If numlanes provided in the tag, use it instead of assumtions
            if (numlanes != -1)
            {
                _numLanes = numlanes;
            }

            generateLanes();
        }
        #endregion



        #region Methods


        #region helper

        void generateLanes()
        {
            double maxShift = (double)(NumLanes - 1) / 2 * laneWidth;
            for (int i = 0; i < NumLanes; i++)
            {
                double shift = i * laneWidth - maxShift;
                lanes.Add(generateSegmentLane(shift, i, true));
            }
        }

        SegmentLaneOSM generateSegmentLane(double distance, int i, Boolean forward)
        {
            double angle = (Math.PI / 2) - Vector2.AngleBetween(this.startNode.Position, this.endNode.Position);

            Vector2 shift = new Vector2(distance * Math.Cos(angle), distance * Math.Sin(angle));
            NodeOSM newStart = new NodeOSM(new Vector2(this.startNode.Position.X + shift.X, this.startNode.Position.Y - shift.Y));
            NodeOSM newEnd = new NodeOSM(new Vector2(this.endNode.Position.X + shift.X, this.endNode.Position.Y - shift.Y));

            SegmentLaneOSM toReturn;

            if (forward)
            {
                toReturn = new SegmentLaneOSM(newStart, newEnd, i);
            }
            else
            {
                toReturn = new SegmentLaneOSM(newEnd, newStart, i);
            }

            return toReturn;
        }
        #endregion

        #endregion

    }
}
