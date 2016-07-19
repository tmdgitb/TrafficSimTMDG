using SimTMDG.Road;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    [Serializable]
    public class IVehicle
    {
        /// <summary>
		/// Enum, aller implementierten IVehicles
		/// </summary>
		[Serializable]
        public enum VehicleTypes
        {
            /// <summary>
            /// Auto
            /// </summary>
            CAR,
            /// <summary>
            /// Bus
            /// </summary>
            BUS,
            /// <summary>
            /// Straßenbahn
            /// </summary>
            TRAM
        };

        #region Hashcodes von Vehicles
        /*
		 * Wir benötigen für Fahrzeuge Hashcodes zur schnellen quasi-eindeutigen Identifizierung. Da sich der Zustand eines Fahrzeuges quasi
		 * ständig ändert, gibt es leider keine zuverlässigen Felder die zur Hashwertberechnung dienen können.
		 * 
		 * Also bedienen wir uns einen alten, nicht umbedingt hübschen, aber bewährten Tricks:
		 * IVehicle verwaltet eine statische Klassenvariable hashcodeIndex, die mit jeder Instanziierung eines Vehicles inkrementiert wird und 
		 * als eindeutiger Hashcode für das Fahrzeug dient. Es muss insbesondere sichergestellt werden, dass bei jeder abgeleiteten Klasse entweder
		 * der Elternkonstruktor aufgerufen wird, oder sich die abgeleitete Klasse selbst um einen gültigen Hashcode kümmert.
		 */

        /// <summary>
        /// Klassenvariable welche den letzten vergebenen hashcode speichert und bei jeder Instanziierung eines Objektes inkrementiert werden muss
        /// </summary>
        private static int hashcodeIndex = 0;

        /// <summary>
        /// Hashcode des instanziierten Objektes
        /// </summary>
        private int hashcode = -1;

        /// <summary>
        /// gibt den Hashcode des Fahrzeuges zurück.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashcode;
        }

        /// <summary>
        /// Standardkonstruktor, der nichts tut als den Hashcode zu setzen.
        /// </summary>
        public IVehicle()
        {
            hashcode = hashcodeIndex++;
            //_statistics.startTime = GlobalTime.Instance.currentTime;
            //_statistics.startTimeOnNodeConnection = GlobalTime.Instance.currentTime;
        }

        public IVehicle(WaySegment cs, Color c, List<WaySegment> r)
        {
            hashcode = hashcodeIndex++;
            currentSegment = cs;
            color = c;
            Routing = new Routing();
            for(int i = 0; i < r.Count; i++)
            {
                Routing.Push(r[i]);
            }

            newCoord();
            RotateVehicle(currentSegment.startNode, currentSegment.endNode);
        }

        #endregion

        public Double length = 7;
        public Double width = 3;
        public Double distance = 0.0;
        private Double rearPos;
        public Double speed = 70;
        public Double acc = 0;
        public Double orientation;
        public Color color = Color.Black;
        public Vector2 absCoord = new Vector2();
        public Double rotation = 0;
        private WaySegment currentSegment;

        public double RearPos
        {
            get
            {
                return distance - (length / 2);
            }

            set
            {
                rearPos = value;
            }
        }



        #region draw
        protected virtual GraphicsPath BuildGraphicsPath()
        {
            GraphicsPath toReturn = new GraphicsPath();
            //Vector2 direction = state.orientation;
            //Vector2 orientation = direction.Normalized;
            //Vector2 normal = direction.RotatedClockwise.Normalized;

            PointF normal = new PointF((float)absCoord.X, (float)absCoord.Y);

            PointF[] ppoints =
                {                        
                    RotatePoint(new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y + (width / 2))), normal, rotation),
                    RotatePoint(new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y - (width / 2))), normal, rotation),
                    RotatePoint(new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y - (width / 2))) , normal, rotation),
                    RotatePoint(new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y + (width / 2))) , normal, rotation)                       
                };
            
            toReturn.AddPolygon(ppoints);

            return toReturn;
        }

        SolidBrush fillBrush = new SolidBrush(Color.Black);

        public virtual void Draw(Graphics g)
        {
            GraphicsPath gp = BuildGraphicsPath();
            fillBrush.Color = color;
            g.FillPath(fillBrush, gp);
        }

        #endregion

        #region route
        private Routing routing;

        internal Routing Routing
        {
            get { return routing; }

            set { routing = value; }
        }

        internal WaySegment CurrentSegment
        {
            get
            {
                return currentSegment;
            }

            set
            {
                currentSegment = value;
            }
        }

        

        #endregion


        #region think
        public void Think(double tickLength)
        {
            
        }

        #region move
        public void Move(double tickLength)
        {
            speed += acc * tickLength;
            distance += speed * tickLength;

            //Debug.WriteLine(hashcode + " - " + distance);

            #region vehicle change segment
            /// Check if vehicles move past the segment
            if (RearPos >= CurrentSegment.Length)
            {
                double newDistance = distance - currentSegment.Length;
                if (Routing.Route.Count > 0)
                {
                    Routing.Route.RemoveLast();
                    if (Routing.Route.Count > 0)
                    {
                        RemoveFromCurrentSegment(Routing.Route.Last.Value, newDistance);
                    }
                    else
                    {
                        RemoveFromCurrentSegment(null, 0);
                    }
                }else
                {
                    RemoveFromCurrentSegment(null, 0);
                }

                
            }
            #endregion
        }
        #endregion

        #endregion

        private void RemoveFromCurrentSegment(WaySegment nextSegment, double startPosition)
        {
            currentSegment.vehToRemove.Add(this);

            if (nextSegment != null)
            {
                distance = startPosition;
                nextSegment.vehicles.Add(this);
                currentSegment = nextSegment;
                RotateVehicle(currentSegment.startNode, currentSegment.endNode);
            }
            else
            {
                // Vehicle died
            }
        }

        public void newCoord()
        {
            Vector2 difference = new Vector2();
            difference.X = currentSegment.endNode.Position.X - currentSegment.startNode.Position.X;
            difference.Y = currentSegment.endNode.Position.Y - currentSegment.startNode.Position.Y;

            PointF toReturn = new PointF();

            //float segmentLength = (float) Vector2.GetDistance(start, end);

            toReturn.X = (float)(difference.X * this.distance) / (float) currentSegment.Length + (float) currentSegment.startNode.Position.X;
            toReturn.Y = (float)(difference.Y * this.distance) / (float) currentSegment.Length + (float) currentSegment.startNode.Position.Y;


            absCoord = toReturn;
        }

        public PointF RotatePoint(PointF point, PointF origin, Double angle)
        {
            float translated_X = point.X - origin.X;
            float translated_Y = point.Y - origin.Y;
            PointF rotated = new PointF
            {
                X = (float)(translated_X * Math.Cos(angle) - translated_Y * Math.Sin(angle)) + origin.X,
                Y = (float)(translated_X * Math.Sin(angle) + translated_Y * Math.Cos(angle)) + origin.Y
            };
            return rotated;
        }

        public void RotateVehicle(Node startPoint, Node endPoint)
        {
            rotation = Vector2.AngleBetween(startPoint.Position, endPoint.Position);
        }

    }
}
