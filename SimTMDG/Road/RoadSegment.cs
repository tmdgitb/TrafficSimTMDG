using SimTMDG.Tools;
using SimTMDG.Vehicle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Road
{
    class RoadSegment:ITickable
    {
        #region Variables
        #region position
        private Vector2 _startPoint;

        public Vector2 StartPoint
        {
            get { return _startPoint; }

            set { _startPoint = value; }
        }

        private Vector2 _endPoint;

        public Vector2 EndPoint
        {
            get { return _endPoint; }

            set { _endPoint = value; }
        }

        #endregion

        #region Vehicles
        public List<IVehicle> vehicles = new List<IVehicle>();
        #endregion

        #endregion


        #region contructor
        public RoadSegment()
        {
            _startPoint = new Vector2(0, 0);
            _endPoint   = new Vector2(0, 0);
        }

        public RoadSegment(Vector2 from, Vector2 to)
        {
            _startPoint = from;
            _endPoint   = to;
        }
        #endregion


        #region methods
        public Double Distance()
        {
            return Vector2.GetDistance(_startPoint, _endPoint);
        }


        #endregion


        #region draw
        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.DarkGray, 0.125f);
            g.DrawLine(pen, (Point)_startPoint, (Point)_endPoint);

            foreach (IVehicle v in vehicles)
            {
                v.Draw(g);
            }
        }

        public void Tick(double tickLength)
        {
            //foreach (IVehicle v in vehicles)
            //{
            //    v.Think(tickLength);
            //    v.absCoord = v.newCoord(_startPoint, _endPoint, v.distance);

            //    if (v.distance >= EndPoint.X)
            //    {
            //        //v.distance = StartPoint.X;
                    
            //    }
            //}
            for(int i = 0; i < vehicles.Count; i++)
            {
                vehicles[i].Think(tickLength);
                vehicles[i].newCoord();
                

                if (vehicles[i].distance >= Length())
                {
                    vehicles.Remove(vehicles[i]);
                }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        #endregion

        Random rnd = new Random();
        public void tempGenerateVeh(int vehNum) {
            vehicles.RemoveRange(0, vehicles.Count);

            for(int i=0; i< vehNum; i++)
            {
                vehicles.Add(new IVehicle());
                vehicles[i].distance = i * (vehicles[i].length * 5);
                vehicles[i].newCoord();
                vehicles[i].rotation = Vector2.AngleBetween(_startPoint, _endPoint);
                Debug.WriteLine("V rotate " + vehicles[i].rotation + "(" + _startPoint.X + ", " + _startPoint.Y + ") - (" + _endPoint.X + ", " + _endPoint.Y + ")");

                vehicles[i].color = Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256));
            }
        }

        public Double Length()
        {
            return Vector2.GetDistance(_startPoint, _endPoint);
        }
    }
}
