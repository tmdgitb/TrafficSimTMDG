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
    [Serializable]
    public class WaySegment : ITickable
    {
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
        public List<IVehicle> vehToRemove = new List<IVehicle>();
        #endregion


        #region Constructor
        public WaySegment()
        {
            _id = idIndex++;
        }


        public WaySegment(Node _startNode, Node _endNode)
        {
            _id = idIndex++;
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
        }

        public void Tick(double tickLength)
        {
            SortVehicles();

            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Think(tickLength);
            }

            RemoveAllVehiclesInRemoveList();

            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Move(tickLength);
            }

            RemoveAllVehiclesInRemoveList();

            
        }

        public void RemoveAllVehiclesInRemoveList()
        {
            foreach (IVehicle v in vehToRemove)
            {
                vehicles.Remove(v);
            }

            vehToRemove.Clear();
        }

        public void Reset()
        {
            foreach (IVehicle v in vehicles)
            {
                v.Reset();
            }
        }
        #endregion


        #region sort vehicles
        public void SortVehicles()
        {
            if (vehicles.Count > 1)
            {
                vehicles.Sort((x, y) => x.distance.CompareTo(y.distance));

                for (int i = 0; i < vehicles.Count; i++)
                {
                    vehicles[i].vehiclesIndex = i;
                }
            }
        }
        #endregion
    }
}
