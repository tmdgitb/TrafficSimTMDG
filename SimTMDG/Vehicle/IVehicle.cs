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

        #endregion

        public Double length = 7;
        public Double width = 3;
        public Double distance = 0.0;
        public Double speed = 70;
        public Double acc = 0;
        public Double orientation;
        public Color color = Color.Black;
        public Vector2 absCoord = new Vector2();
        public Double rotation = 0;



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
                        //new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y + (width / 2))),
                        //new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y - (width / 2))),
                        //new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y - (width / 2))),
                        //new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y + (width / 2)))

                        
                        RotatePoint(new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y + (width / 2))), normal, rotation),
                        RotatePoint(new PointF((float)(absCoord.X + (length / 2)), (float)(absCoord.Y - (width / 2))), normal, rotation),
                        RotatePoint(new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y - (width / 2))) , normal, rotation),
                        RotatePoint(new PointF((float)(absCoord.X - (length / 2)), (float)(absCoord.Y + (width / 2))) , normal, rotation)


                        
                        
                        

                        //new PointF((float)(absCoord.X + (length / 2) * Math.Cos(-rotation) - (width / 2) * Math.Sin(-rotation)),
                        //           (float)(absCoord.Y + (width / 2) * Math.Cos(-rotation) + (width / 2) * Math.Sin(-rotation))),           // UL
                        //new PointF((float)(absCoord.X - (length / 2) * Math.Cos(-rotation) - (width / 2) * Math.Sin(-rotation)),
                        //           (float)(absCoord.Y + (width / 2) * Math.Cos(-rotation) - (width / 2) * Math.Sin(-rotation))),           // UR
                        //new PointF((float)(absCoord.X - (length / 2) * Math.Cos(-rotation) + (width / 2) * Math.Sin(-rotation)),
                        //           (float)(absCoord.Y - (width / 2) * Math.Cos(-rotation) - (width / 2) * Math.Sin(-rotation))),           // BR
                        //new PointF((float)(absCoord.X + (length / 2) * Math.Cos(-rotation) + (width / 2) * Math.Sin(-rotation)),
                        //           (float)(absCoord.Y - (width / 2) * Math.Cos(-rotation) + (width / 2) * Math.Sin(-rotation)))            // BL
                        
                        //state.positionAbs  -  8 * normal,
                        //state.positionAbs  +  8 * normal,
                        //state.positionAbs  -  length * orientation  +  8 * normal,
                        //state.positionAbs  -  length * orientation  -  8 * normal,
                        
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


        #region think
        public void Think(double tickLength)
        {
            speed += acc * tickLength;
            distance += speed * tickLength;

            Debug.WriteLine(hashcode + " - " + distance);
        }
        #endregion

        public PointF newCoord(Vector2 start, Vector2 end, Double traveled)
        {
            Vector2 difference = new Vector2();
            difference.X = end.X - start.X;
            difference.Y = end.Y - start.Y;

            PointF toReturn = new PointF();

            float distance = (float) Vector2.GetDistance(start, end);

            toReturn.X = (float)(difference.X * traveled) / distance + (float) start.X;
            toReturn.Y = (float)(difference.Y * traveled) / distance + (float) start.Y;


            return toReturn;
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

    }
}
