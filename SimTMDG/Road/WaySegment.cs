using SimTMDG.Tools;
using SimTMDG.Vehicle;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    class WaySegment : ITickable
    {
        #region ID
        /// <summary>
        /// WaySegment ID
        /// </summary>
        private int _id;

        public int Id
        {
            get { return _id; }

            set { _id = value; }
        }
        #endregion

        #region road properties

        #region primary properties
        /// <summary>
        /// Starting node of WaySegment
        /// </summary>
        public Node startNode;

        /// <summary>
        /// Ending node of WaySegment
        /// </summary>
        public Node endNode;

        /// <summary>
        /// WaySegment length from start node to end node
        /// </summary>
        private double _length;

        public double Length
        {
            get { return _length; }

            set { _length = value; }
        }
        #endregion


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
        /// Way / Road name
        /// </summary>
        private string _roadName;

        public string RoadName
        {
            get { return _roadName; }

            set { _roadName = value; }
        }
        #endregion

        #region vehicles on segment
        public List<IVehicle> vehicles = new List<IVehicle>();
        #endregion


        #region Constructor
        public WaySegment()
        {

        }


        public WaySegment(Node _startNode, Node _endNode)
        {
            startNode = _startNode;
            endNode = _endNode;
            _length = Vector2.GetDistance(startNode.Position, endNode.Position);
        }
        #endregion


        #region draw
        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.DarkGray, 1);
            g.DrawLine(pen, (Point)startNode.Position, (Point)endNode.Position);

            foreach (IVehicle v in vehicles)
            {
                v.Draw(g);
            }
        }

        public void Tick(double tickLength)
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Think(tickLength);
                vehicles[i].absCoord = vehicles[i].newCoord(startNode.Position, endNode.Position, vehicles[i].distance);


                if (vehicles[i].distance >= Length)
                {
                    vehicles.Remove(vehicles[i]);
                }
            }
        }

        public void Reset()
        {
        }
        #endregion
    }
}
