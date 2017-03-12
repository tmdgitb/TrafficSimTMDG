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
    public class IVehicle : IDM
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
            dumb = false;
            //_statistics.startTime = GlobalTime.Instance.currentTime;
            //_statistics.startTimeOnNodeConnection = GlobalTime.Instance.currentTime;
        }

        public IVehicle(RoadSegment cs, int laneIndex, Color c, List<RoadSegment> r)
        {
            hashcode = hashcodeIndex++;
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;
            color = c;
            Routing = new Routing();
            Routing.PushAll(r);

            Random rnd = new Random();
            _physics = new IVehicle.Physics(14, 14, 0); //( rnd.Next(7, 14), 14, 0);

            dumb = false;

            newCoord();
            RotateVehicle(currentSegment.startNode, currentSegment.endNode);
        }

        public void Reuse(RoadSegment cs, int laneIndex, List<RoadSegment> r)
        {
            distance = 0;
            _state.currentSegment = cs;
            _state.laneIdx = laneIndex;
            Routing.Route.Clear();
            Routing.PushAll(r);

            newCoord();
            RotateVehicle(currentSegment.startNode, currentSegment.endNode);
        }

        #endregion


        #region Physics
        
        /// <summary>
		/// Physik des Fahrzeuges
		/// </summary>
		public IVehicle.Physics _physics; // PROTECTED
        /// <summary>
        /// Physik des Fahrzeuges
        /// </summary>
        public IVehicle.Physics physics
        {
            get { return _physics; }
            set { _physics = value; }
        }

        /// <summary>
        /// Struktur, die Wunschgeschwindigkeit, Geschwindigkeit und Beschleunigung kapselt
        /// </summary>
        public struct Physics
        {
            /// <summary>
            /// Erstellt ein neues Physics Record
            /// </summary>
            /// <param name="d">Wunschgeschwindigkeit</param>
            /// <param name="v">Geschwindigkeit</param>
            /// <param name="a">Beschleunigung</param>
            public Physics(double d, double v, double a)
            {
                targetVelocity = d;
                velocity = v;
                acceleration = a;
                multiplierTargetVelocity = 1;
            }

            /// <summary>
            /// gewünschte Gecshwindigkeit des Autos 
            /// </summary>
            public double targetVelocity;

            /// <summary>
            /// Geschwindigkeit des Fahrzeuges
            /// (sehr wahrscheinlich gemessen in Änderung der Position/Tick)
            /// </summary>
            public double velocity;

            /// <summary>
            /// Beschleunigung des Fahrzeugens
            /// (gemessen in Änderung der Geschwindigkeit/Tick)
            /// </summary>
            public double acceleration;

            /// <summary>
            /// Multiplikator für die Wunschgeschwindigkeit.
            /// Kann benutzt werden, um kurzzeitig schneller zu fahren (etwa um einen Spurwechsel zu machen)
            /// </summary>
            public double multiplierTargetVelocity;
        }

        /// <summary>
		/// Target Velocity of this vehicle.
		/// Currently only a shortcut to the target velocity of the current NodeConnection of this vehicle.
		/// </summary>
		public double targetVelocity
        {
            get
            {
                return (currentSegment.TargetSpeed != -1) ? Math.Min(_physics.targetVelocity, currentSegment.TargetSpeed) : _physics.targetVelocity;
                //return _physics.targetVelocity;
            }
        }

        /// <summary>
        /// effektive gewünschte Gecshwindigkeit des Autos (mit Multiplikator multipliziert)
        /// </summary>
        public double effectiveDesiredVelocity
        {
            get { return targetVelocity * _physics.multiplierTargetVelocity; }
        }

        #endregion


        #region State
        /// <summary>
        /// Statusrecord, kapselt aktuelle NodeConnection+Position
        /// </summary>
        public struct State
        {
            /// <summary>
            /// Konstruktor, der nur die Position initialisiert. Der Rest ist uninitialisiert!
            /// </summary>
            /// <param name="p">Zeitposition auf der Linie</param>
            public State(double p)
            {
                currentSegment = null;
                laneIdx = -1;
                targetLaneIdx = -1;
                prevLaneIdx = -1;
                m_Position = p;
                //m_PositionAbs = null;
                //m_Orientation = null;
                //m_letVehicleChangeLine = false;
                //m_tailPositionOfOtherVehicle = 0;
                //m_vehicleThatLetsMeChangeLine = null;
                //m_vehicleToChangeLine = null;
                _freeDrive = true;
            }

            /// <summary>
            /// Standardkonstruktor, benötigt eine Nodeconnection und Zeitposition. Der Rest wird intern berechnet
            /// </summary>
            /// <param name="nc">aktuelle NodeConnection</param>
            /// <param name="p">Zeitposition auf nc</param>
            public State(RoadSegment cs, int laneIndex, double p)
            {
                currentSegment = cs;
                laneIdx = laneIndex;
                targetLaneIdx = -1;
                prevLaneIdx = -1;
                m_Position = p;

                //double t = currentNodeConnection.lineSegment.PosToTime(p);
                //m_PositionAbs = currentNodeConnection.lineSegment.AtTime(t);
                //m_Orientation = currentNodeConnection.lineSegment.DerivateAtTime(t);
                //m_letVehicleChangeLine = false;
                //m_tailPositionOfOtherVehicle = 0;
                //m_vehicleThatLetsMeChangeLine = null;
                //m_vehicleToChangeLine = null;
                _freeDrive = true;
            }

            /// <summary>
            /// die Line, auf der sich das Fahrzeug gerade befindet
            /// </summary>
            public RoadSegment currentSegment;

            /// <summary>
            /// Lane index in current RoadSegment
            /// </summary>
            public int laneIdx;

            /// <summary>
            /// Target lane index in current RoadSegment
            /// </summary>
            public int targetLaneIdx;

            /// <summary>
            /// Previous lane index in current RoadSegment
            /// </summary>
            public int prevLaneIdx;

            /// <summary>
            /// relative Position auf der Line
            /// </summary>
            private double m_Position;
            /// <summary>
            /// Zeitposition auf der Linie
            /// </summary>
            public double position
            {
                get { return m_Position; }
                set
                {
                    m_Position = value;
                    if (currentSegment != null)
                    {
                        //double t = currentNodeConnection.lineSegment.PosToTime(m_Position);
                        //m_PositionAbs = newCoord();
                        //m_Orientation = currentNodeConnection.lineSegment.DerivateAtTime(t);
                    }
                }
            }

            ///// <summary>
            ///// absolute Position auf der Linie
            ///// </summary>
            //private Vector2 m_PositionAbs;
            ///// <summary>
            ///// absolute Position auf der Linie in Weltkoordinaten
            ///// </summary>
            //public Vector2 positionAbs
            //{
            //    get { return m_PositionAbs; }
            //}

            ///// <summary>
            ///// Richtung in die das Fahrzeug fährt
            ///// </summary>
            //private Vector2 m_Orientation;
            ///// <summary>
            ///// Richtung (Ableitung) an der aktuellen Position
            ///// </summary>
            //public Vector2 orientation
            //{
            //    get { return m_Orientation; }
            //}

            /// <summary>
            /// gibt ein das Auto umschließendes RectangleF zurück
            /// </summary>
            //public RectangleF boundingRectangle
            //{
            //    get
            //    {
            //        return new RectangleF(
            //            m_PositionAbs - new Vector2(15, 15),
            //            new Vector2(30, 30)
            //            );
            //    }
            //}

            /// <summary>
            /// Flag whether this vehicle can drive freely or is obstructed (traffic light, slower vehicle in front)
            /// </summary>
            public bool _freeDrive;
        }


        /// <summary>
		/// aktueller State des Fahrezeuges
		/// </summary>
        public IVehicle.State _state;

        /// <summary>
        /// aktueller State des Fahrezeuges
        /// </summary>
        public IVehicle.State state
        {
            get { return _state; }
            set { _state = value; }
        }
        

        /// <summary>
        /// aktuelle Bogenlängenposition auf der aktuellen NodeConnection
        /// </summary>
        public double currentPosition
        {
            get { return state.position; }
        }

        /// <summary>
        /// aktuelle NodeConnection
        /// </summary>
        public RoadSegment currentSegment
        {
            get { return state.currentSegment; }
        }

        #endregion

        public int vehiclesIndex = 0;
        public Double length = 7;
        public Double width = 3;
        public Double distance = 0.0;
        private Double rearPos;
        private Double frontPos;
        //public Double speed = 70;
        //public Double acc = 0;
        public Double orientation;
        public Color color = Color.Black;
        public Vector2 absCoord = new Vector2();
        public Double rotation = 0;
        //private WaySegment currentSegment;
        private bool alreadyMoved = false;

        public double RearPos
        {
            get { return distance - (length / 2); }
            set { rearPos = value; }
        }

        public double FrontPos
        {
            get { return distance + (length / 2); }
            set { frontPos = value; }
        }

        #region debugVeh
        public RectangleF positionRect
        {
            get { return new RectangleF((float)absCoord.X - 1, (float)absCoord.Y - 1, 2, 2); }
        }
        private bool debugVeh = false;


        public void ToggleDebug()
        {
            debugVeh = !debugVeh;
        }

        #endregion


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
            //if (state.laneIdx == 0)
            //{
            //    fillBrush.Color = Color.Blue;
            //}
            //else if (state.laneIdx == 1)
            //{
            //    fillBrush.Color = Color.Red;
            //}
            //else if (state.laneIdx == 2)
            //{
            //    fillBrush.Color = Color.Green;
            //}

            g.FillPath(fillBrush, gp);

            if (debugVeh)
            {
                #region debug draw
                Font debugFont = new Font("Calibri", 6);
                Brush blackBrush = new SolidBrush(Color.Black);
                Rectangle layoutRect = new Rectangle((int)absCoord.X + 100, (int)absCoord.Y, 80, 160);

                string leadVdId = "";
                string leadVdS = "";
                string newleadVdId = "";
                string newleadVdS = "";
                string newlagVdId = "";
                string newlagVdS = "";
                //string lagVdId = "";
                //string lagVdS = "";

                if (leadVd != null)
                {
                    if (leadVd.vehicle != null)
                    {
                        leadVdId = leadVd.vehicle.hashcode.ToString();
                    }

                    leadVdS = leadVd.distance.ToString("#.#");
                }

                if (newLeadVd != null)
                {
                    if (newLeadVd.vehicle != null)
                    {
                        newleadVdId = newLeadVd.vehicle.hashcode.ToString();
                    }

                    newleadVdS = newLeadVd.distance.ToString("#.#");
                }

                if (newLagVd != null)
                {
                    if (newLagVd.vehicle != null)
                    {
                        newlagVdId = newLagVd.vehicle.hashcode.ToString();
                    }

                    if (newLagVd.distance == double.MaxValue)
                    {
                        newlagVdS = "Max";
                    }else
                    {
                        newlagVdS = newLagVd.distance.ToString("#.#");
                    }

                }

                //if (lagVd != null)
                //{
                //    if (lagVd.vehicle != null)
                //    {
                //        lagVdId = lagVd.vehicle.hashcode.ToString();
                //    }
                //    lagVdS = lagVd.distance.ToString();
                //}

                //string debugString = hashcode.ToString() + " - LastTimeLC: " + lastTimeLc + " - iProcLC: " + inProcessOfLC.ToString() + " - LCDelay : " + tLaneChangeDelay;
                //string debugString = hashcode.ToString() + " - lv: " + leadVdId + " - nlv: " + newleadVdId; // + " - s: " + leadVdS;
                //g.DrawString(hashcode.ToString(), debugFont, blackBrush, layoutRect);

                // TODO : add a complete debug param for checking LC / MOBIL process
                // TODO : only display the ID on the moving debug param

                string debugString = hashcode.ToString() + "@prev[" + state.prevLaneIdx + "] " + " a: " + physics.acceleration +
                                        //"|| TL s: " + distanceToTrafficLight.ToString("#.#") +
                                        " || ldv: " + leadVdId + " -s : " + leadVdS +
                                        " || nldv: " + newleadVdId + " - s: " + newleadVdS +
                                        " || nlgv: " + newlagVdId + " - s: " + newlagVdS;   

                //g.Transform = new Matrix(1, 0, 0, 1, 0, 0);
                g.DrawString(debugString, debugFont, blackBrush, layoutRect);

                #endregion
            }
        }

        #endregion

        #region route
        private Routing routing;

        internal Routing Routing
        {
            get { return routing; }

            set { routing = value; }
        }

        //internal WaySegment CurrentSegment
        //{
        //    get
        //    {
        //        return currentSegment;
        //    }

        //    set
        //    {
        //        currentSegment = value;
        //    }
        //}



        #endregion

        #region temp_Dumb
        public bool dumb;
        #endregion

        #region think
        public void Think(double tickLength)
        {
            double acceleration;

            if (!dumb)
            {
                List<RoadSegment> route = new List<RoadSegment>();
                foreach (RoadSegment ws in routing.Route)
                    route.Add(ws);
                            
                acceleration = Think(route, tickLength);
            } else { acceleration = 0; }
            
            Accelerate(acceleration);
        }

        Boolean thinkAboutLC = true;
        Boolean inProcessOfLC = false;
        double lastTimeLc = 6;
        double timerLC = 0;
        VehicleDistance leadVd;
        VehicleDistance lagVd;
        double distanceToTrafficLight;


        public double Think(List<RoadSegment> route, double tickLength)
        {
            if (route.Count == 0)
                return 0;

            double lookaheadDistance = Constants.SearchRange; // Constants.lookaheadDistance;
            //double intersectionLookaheadDistance = 384; // Constants.intersectionLookaheadDistance;
            //double stopDistance = -1;
            //_state._freeDrive = true;

            //thinkAboutLineChange;
            double lowestAcceleration = 0;


            #region Vehicle in Front

            leadVd = findVehicleInFront(route);

            if (leadVd != null && leadVd.distance < lookaheadDistance)
            {
                lookaheadDistance = leadVd.distance;

                if (leadVd.vehicle != null)
                {
                    lowestAcceleration = CalculateAcceleration(physics.velocity, effectiveDesiredVelocity, lookaheadDistance, this.physics.velocity - leadVd.vehicle.physics.velocity);
                }
                else
                {
                    lowestAcceleration = CalculateAcceleration(physics.velocity, effectiveDesiredVelocity, lookaheadDistance, physics.velocity);
                }
            }
            else
            {
                // free acceleration
                lowestAcceleration = CalculateAcceleration(physics.velocity, effectiveDesiredVelocity);
            }

            #endregion

            #region intersection
            //if (route[1].prevSegment.Count > 1) // Move this, not efficient
            //{
            //    if (route[0].Length - (distance + length / 2) <= route[1].startNode.intersectionRadius)
            //    {

            //        VehicleDistance closestVeh = null;

            //        for (int i = 0; i < route[1].prevSegment.Count; i++)
            //        {
            //            for (int j = 0; i < route[1].prevSegment[i].lanes.Count; j++)
            //            {
            //                if (closestVeh == null)
            //                {
            //                    closestVeh = new VehicleDistance(
            //                        route[i].prevSegment[i].lanes[j].vehicles[route[i].prevSegment[i].lanes[j].vehicles.Count],
            //                        route[i].prevSegment[i].lanes[j].Length - route[i].prevSegment[i].lanes[j].vehicles[route[i].prevSegment[i].lanes[j].vehicles.Count].distance);
            //                }
            //                else if (route[i].prevSegment[i].lanes[j].Length - route[i].prevSegment[i].lanes[j].vehicles[route[i].prevSegment[i].lanes[j].vehicles.Count]
            //                    .distance
            //                    < closestVeh.distance)
            //                {
            //                    closestVeh.vehicle = route[i].prevSegment[i].lanes[j].vehicles[route[i].prevSegment[i].lanes[j].vehicles.Count];
            //                    closestVeh.distance = route[i].prevSegment[i].lanes[j].Length - route[i].prevSegment[i].lanes[j].vehicles[route[i].prevSegment[i].lanes[j].vehicles.Count]
            //                    .distance;
            //                }
            //            }
            //        }

            //        route[1].startNode.registeredVeh = closestVeh.vehicle;
            //    }
                
            //}
            #endregion


            #region Traffic lights

            double TLAcc = double.MaxValue;
            // Check for red traffic lights on route
            distanceToTrafficLight = GetDistanceToNextTrafficLightOnRoute(route, this.distance, Constants.SearchRange, true);
            //intersectionLookaheadDistance = distanceToTrafficLight;

            // If the next TrafficLight is closer than the next vehicle, no free line change shall be performed
            if (distanceToTrafficLight < lookaheadDistance)
            {
                lookaheadDistance = distanceToTrafficLight;
                thinkAboutLC = false;
                TLAcc = CalculateAcceleration(physics.velocity, effectiveDesiredVelocity, lookaheadDistance, physics.velocity);
                _state._freeDrive = false;
            }

            if (TLAcc < lowestAcceleration)
            {
                lowestAcceleration = TLAcc;
            }

            #endregion


            //return Math.Max(lowestAcceleration, -1.5 * bSafe);
            return lowestAcceleration;
        }

        public int MakeLaneChange(double tickLength)
        {
            int toReturn = Lane.NO_CHANGE;

            if (inProcessOfLC)
            {
                updateLaneChangeDelay(tickLength);
            } else if (lastTimeLc <= 5)
            {
                lastTimeLCTick(tickLength);
            } else { 
                bool doLC = LaneChange(tickLength);

                if (doLC)
                {
                    setLane(state.targetLaneIdx);
                    getLane(state.laneIdx).AddVehicle(this);
                    getLane(state.prevLaneIdx).RemoveVehicle(this);
                    //inProcessOfLC = true;
                }

            }

            return toReturn;
        }

        public bool LaneChange(double tickLength)
        {

            List<RoadSegment> route = new List<RoadSegment>();
            foreach (RoadSegment ws in routing.Route)
                route.Add(ws);

            #region lane changing
            if ((!inProcessOfLC) && (currentSegment.lanes.Count > 1) && (lastTimeLc > 5)) //
            {
                thinkAboutLC = true;
            }
            else
            {
                thinkAboutLC = false;
            }

            //if (inProcessOfLaneChange())
            //{
            //    updateLaneChangeDelay(tickLength);
            //}
            //else if ((currentSegment.lanes.Count > 1))
            //{
            if (thinkAboutLC)
            {
                int direction = 0;
                double accToLeft = double.MinValue;
                double accToRight = double.MinValue;

                // Check lane + 1 (right lane)
                if (state.laneIdx < currentSegment.lanes.Count - 1)
                {
                    direction = Lane.TO_RIGHT;
                    accToRight = ConsiderLaneChange(direction, tickLength, route, physics.acceleration);
                }

                // Check lane - 1 (left lane)
                if (state.laneIdx > 0)
                {
                    direction = Lane.TO_LEFT;
                    accToLeft = ConsiderLaneChange(direction, tickLength, route, physics.acceleration);
                }

                if (inProcessOfLC)
                {
                    if (accToLeft > accToRight)
                    {
                        _state.targetLaneIdx = state.laneIdx + Lane.TO_LEFT;
                        //Accelerate(accToLeft);
                        return true;
                    }
                    else
                    {
                        _state.targetLaneIdx = state.laneIdx + Lane.TO_RIGHT;
                        //Accelerate(accToRight);
                        return true;
                    }
                }

            }
            #endregion

            return false;
        }


        VehicleDistance newLeadVd = null;
        VehicleDistance newLagVd = null;
        // TODO
        private double ConsiderLaneChange(int direction, double tickLength, List<RoadSegment> route, double lowestAcceleration)
        {
            #region consider lane change
            // Find idx of new Lead
            //int newLeadIdx = currentLane(direction).findVehicleInFront(distance);
            newLeadVd = findVehicleInFront(route, direction);

            double newTLDistance = GetDistanceToNextTrafficLightOnRoute(route, distance, Constants.SearchRange, true);            


            newLagVd = findVehicleInBehind(direction);
            //VehicleDistance lagVd = findVehicleInBehind();
            lagVd = findVehicleInBehind();

            //double acc = Think(route, tickLength);
            double sNewLead;
            double sNewLag;
            double sLead;
            double sLag;
            double vNewLead;
            double vNewLag;
            double vLead;
            double vLag;
            double accNew;
            double accNewLag;
            double newAccNewLag;
            double accLag;
            double newAccOldLag;

            /// If new lead in target lane is found
            if (newLeadVd != null)
            {
                sNewLead = newLeadVd.distance;

                if (sNewLead < newTLDistance)
                {
                    if (newLeadVd.vehicle != null)
                    {
                        vNewLead = newLeadVd.vehicle.physics.velocity;
                    }
                    else
                    {
                        vNewLead = physics.velocity;
                    }
                }else
                {
                    sNewLead = newTLDistance;
                    vNewLead = 0;
                }

            }
            /// If new lead in target lane is NOT found
            else
            {
                sNewLead = Constants.SearchRange;
                vNewLead = physics.velocity;
            }

            /// If new follower in target lane is found
            if (newLagVd != null)
            {
                sNewLag = newLagVd.distance;

                if (newLagVd.vehicle != null)
                {
                    vNewLag = newLagVd.vehicle.physics.velocity;
                    accNewLag = newLagVd.vehicle.physics.acceleration;

                    newAccNewLag = CalculateAcceleration(vNewLag, effectiveDesiredVelocity, sNewLag, vNewLag - physics.velocity);
                }
                else
                {
                    vNewLag = 0;
                    accNewLag = 0;
                    newAccNewLag = 0;
                }
            }
            /// If new follower in target lane is NOT found
            else
            {
                sNewLag = Constants.SearchRange;
                vNewLag = 0;
                accNewLag = 0;
                newAccNewLag = 0;
            }

            /// If lead in current lane is found
            if (leadVd != null)
            {
                sLead = leadVd.distance;

                if (leadVd.vehicle != null)
                {
                    vLead = leadVd.vehicle.physics.velocity;
                }
                else
                {
                    vLead = double.MaxValue;
                }
            }
            /// If follower in current lane is NOT found
            else
            {
                sLead = Constants.SearchRange;
                vLead = double.MaxValue;
            }

            /// If follower in current lane is found
            if (lagVd != null)
            {
                sLag = lagVd.distance;

                if (lagVd.vehicle != null)
                {
                    vLag = lagVd.vehicle.physics.velocity;
                    accLag = lagVd.vehicle.physics.acceleration;

                    newAccOldLag = CalculateAcceleration(
                        vLag,
                        lagVd.vehicle.effectiveDesiredVelocity,
                        sLag + sLead + this.length,
                        vLag - vLead
                    );
                }
                else
                {
                    vLag = 0;
                    accLag = 0;
                    newAccOldLag = 0;
                }
            }
            /// If follower in current lane is NOT found
            else
            {
                sLag = Constants.SearchRange;
                vLag = 0;
                accLag = 0;
                newAccOldLag = 0;
            }
            

            accNew = CalculateAcceleration(physics.velocity, effectiveDesiredVelocity, sNewLead, physics.velocity - vNewLead);


            if (SafetyCriterion(newAccNewLag) && (sNewLag > 0) && (sNewLead >0)) // this.length / 2
            {
                if (LaneChangeDecision(accNew, physics.acceleration, newAccNewLag, accNewLag, newAccOldLag, accLag))
                {
                    // Do line change
                    //thinkAboutLineChange = true;
                    //color = Color.BlueViolet;
                    //doLaneChange(direction, tickLength);
                    lowestAcceleration = accNew;
                    //_state.targetLaneIdx = state.laneIdx + direction;
                    inProcessOfLC = true;
                }
            }
            #endregion

            return lowestAcceleration;
        }

        private void setLane(int targetLane)
        {
            _state.prevLaneIdx = state.laneIdx;
            _state.laneIdx = targetLane;
            _state.targetLaneIdx = Lane.NONE;
        }


        private void doLaneChange(double tickLength)
        {
            //currentLane(direction).vehicles.Add(this);
            ////currentLane().vehicles.Remove(this);
            //currentLane().vehToRemove.Add(this);
            //_state.laneIdx = state.laneIdx + direction;
            ////thinkAboutLineChange = false;
            //resetDelay(tickLength);

            //_state.targetLaneIdx = state.laneIdx + direction;
            //_state.prevLaneIdx = state.laneIdx;
            //_state.laneIdx = state.targetLaneIdx;
            currentLane().RemoveVehicle(this);
            currentLane(state.targetLaneIdx - state.laneIdx).AddVehicle(this);
            updateLaneChangeDelay(tickLength);
            //currentLane().vehToRemove.Add(this);

            _state.prevLaneIdx = state.laneIdx;
            _state.laneIdx = state.targetLaneIdx;
            _state.targetLaneIdx = -1;

            //resetDelay(tickLength);
        }

        // TODO Move it to proper place
        private double FINITE_LANE_CHANGE_TIME_S = 4;
        private double tLaneChangeDelay = 0;

        private Boolean inProcessOfLaneChange()
        {
            //if ((tLaneChangeDelay >= FINITE_LANE_CHANGE_TIME_S) && (state.targetLaneIdx != -1))
            //{
            //    _state.prevLaneIdx = state.laneIdx;                
            //    _state.laneIdx = state.targetLaneIdx;
            //    _state.targetLaneIdx = -1;

            //    currentLane().vehToRemove.Add(this);
            //}

            // newCoordLaneChange

            return (tLaneChangeDelay > 0 && tLaneChangeDelay < FINITE_LANE_CHANGE_TIME_S);
        }

        private void resetDelay(double dt)
        {
            tLaneChangeDelay = 0;
            lastTimeLc = 0;
            //updateLaneChangeDelay(dt);
        }

        public void updateLaneChangeDelay(double dt)
        {
            tLaneChangeDelay += dt;

            if (tLaneChangeDelay > 1)
            {
                laneChangeFinished(dt);
            }
        }

        public void laneChangeFinished(double dt)
        {
            //doLaneChange(dt);

            //currentLane().vehToRemove.Add(this);
            //currentLane().RemoveVehicle(this);
            //_state.prevLaneIdx = state.laneIdx;
            //_state.laneIdx = state.targetLaneIdx;

            //getLane(state.prevLaneIdx).RemoveVehicle(this);

            inProcessOfLC = false;
            resetDelay(dt);
        }

        public void lastTimeLCTick(double dt)
        {
            lastTimeLc += dt;
        }


        // TODO : Check
        private VehicleDistance findVehicleInFront(List<RoadSegment> route, int direction = 0)
        {
            VehicleDistance vd = null;
            double searchedDistance = 0;

            /// Search in current segment in current RoadSegment   
            //if (direction == 0)
            //{
            //    if ((currentLane().vehicles.Count > 1) && (this.vehiclesIndex != currentLane().vehicles.Count - 1))
            //    {
            //        IVehicle veh = currentLane().vehicles[vehiclesIndex + 1];
            //        double returnDistance = veh.FrontPos - (veh.length + this.FrontPos);
            //        vd = new VehicleDistance(veh, returnDistance);
            //    }
            //}
            /// Search in current segment +direction(left or right segment) in current RoadSegment
            //else
            //{
                if (currentLane(direction).vehicles.Count > 0)
                {
                    int vehIdx = currentLane(direction).findVehicleInFront(this.distance);

                    if (vehIdx != -1)
                    {
                        IVehicle veh = currentLane(direction).vehicles[vehIdx]; 
                        double returnDistance = veh.FrontPos - (veh.length + this.FrontPos);
                        vd = new VehicleDistance(veh, returnDistance);
                    }
                }
            //}


            /// No front veh in current segment, search in next RoadSegment
            if (vd == null)
            {
                /// add current lane length remaining distance to accumulated searched distance
                searchedDistance += route[0].Length - (this.FrontPos);
                /// search in next segment
                vd = findVehFrontNext(route, searchedDistance, direction);
            }else if (vd.distance < 0)
            {
                vd.distance = 0;
            }


            return vd;
        }

        private VehicleDistance findVehFrontNext(List<RoadSegment> route, double searchedDistance, int direction = 0)
        {
            VehicleDistance vd = null;

            /// search in remaining segment on route
            for (int i = 1; i < route.Count; i++)
            {
                /// if within reach
                if (searchedDistance < Constants.SearchRange)
                {
                    /// if the route has the connected searched lane (have no fewer lane)
                    if (route[i].lanes.Count >= state.laneIdx + 1 + direction)
                    {
                        /// if the searched lane has a vehicle
                        if (route[i].lanes[state.laneIdx + direction].vehicles.Count > 0)
                        {
                            /// returnDistance = distance of veh to its startpoint + accumulated search distance
                            double returnDistance = (route[i].lanes[state.laneIdx + direction].vehicles[0].distance
                                - route[i].lanes[state.laneIdx + direction].vehicles[0].length / 2) + (searchedDistance);

                            /// returnVehicle = most behind vehicle
                            vd = new VehicleDistance(route[i].lanes[state.laneIdx + direction].vehicles[0], returnDistance);

                            /// front veh is found, break the search loop
                            break;
                        }
                        /// no vehicle in this lane segment
                        else
                        {
                            /// accumulate search distance, get ready for next loop
                            searchedDistance += route[i].Length;
                        }
                    }
                    /// the route has fewer lane than target (no connection to prev searched lane)
                    else
                    {
                        /// return null
                        vd = new VehicleDistance(null, searchedDistance);
                        /// break the loop
                        break;
                    }
                }
                /// past the searching reach
                else
                {
                    /// return null
                    vd = new VehicleDistance(null, searchedDistance);
                    /// break the loop
                    break;
                }
            }

            return vd;
        }

        #region old find Front
        // TODO
        private VehicleDistance findVehicleInFront()
        {
            //if (this.currentSegment.vehicles.Count < 2)
            //{
            //    return null;
            //}

            //for (int i = 0; i < this.currentSegment.vehicles.Count; i++)
            //{
            //    if (this.currentSegment.vehicles[i].distance > this.distance)
            //    {

            //    }
            //}


            VehicleDistance vd = null;
            if (this.currentLane().vehicles.Count > 1)
            {
                for (int i = 0; i < this.currentLane().vehicles.Count; i++)
                {
                    if (this.currentLane().vehicles[i].distance > this.distance)
                    {
                        if (vd == null)
                        {
                            vd = new VehicleDistance(this.currentLane().vehicles[i],
                                    this.currentLane().vehicles[i].distance 
                                    - (this.distance + (this.currentLane().vehicles[i].length / 2) + (this.length / 2))
                                );
                        }
                        else if (this.currentLane().vehicles[i].distance < vd.vehicle.distance)
                        {
                            vd.vehicle = this.currentLane().vehicles[i];
                            vd.distance = this.currentLane().vehicles[i].distance 
                                - (this.distance + (this.currentLane().vehicles[i].length / 2) + (this.length / 2));
                        }
                    }
                }
            }

            if (vd == null)
            {
                if (currentLane().vehicles.Count > 1)
                {
                    for (int i = 0; i < currentLane().vehicles.Count; i++)
                    {
                        if (currentLane().vehicles[i].distance > this.distance)
                        {

                            if (vd == null)
                            {
                                double distanceToFront = (this.currentSegment.Length - (this.distance + (this.length / 2)))
                                                         + (currentLane().vehicles[i].distance
                                                         - (currentLane().vehicles[i].length / 2));

                                vd = new VehicleDistance(currentLane().vehicles[i], distanceToFront);
                            }
                            else if (currentLane().vehicles[i].distance < vd.vehicle.distance)
                            {
                                double distanceToFront = (this.currentSegment.Length - (this.distance + (this.length / 2)))
                                                         + (currentLane().vehicles[i].distance
                                                         - (currentLane().vehicles[i].length / 2));

                                vd.vehicle = currentLane().vehicles[i];
                                vd.distance = distanceToFront;
                            }
                        }
                    }
                }
            }

            //frontVeh = currentSegment.vehicles[0];


            return vd;
        }
        #endregion

        // TODO
        private VehicleDistance findVehicleInBehind(int direction = 0)
        {
            VehicleDistance vd = null;
            double searchedDistance = 0;

            /// Search in current segment in current RoadSegment   
            //if (direction == 0)
            //{
            //    /// if this veh is not the most behind in current lane
            //    if (this.vehiclesIndex != 0)
            //    {
            //        IVehicle veh = currentLane().vehicles[vehiclesIndex - 1];
            //        double returnDistance = this.distance - (veh.frontPos + (this.length / 2));
            //        vd = new VehicleDistance(veh, returnDistance);
            //    }
            //}
            ///// Search in current segment + direction (left or right segment) in current RoadSegment
            //else
            //{
                if (currentLane(direction).vehicles.Count > 0)
                {
                    int vehIdx = currentLane(direction).findVehicleBehind(this.distance);

                    // If the behind vehicle is found
                    if (vehIdx != -1)
                    {
                        IVehicle veh = currentLane(direction).vehicles[vehIdx];
                        double returnDistance = this.RearPos - veh.FrontPos; //this.distance - (veh.FrontPos + (this.length / 2));
                        vd = new VehicleDistance(veh, returnDistance);
                    }
                }
            //}

            /// No front veh in current segment, search in next RoadSegment
            if (vd == null)
            {
                /// add current lane length remaining distance to accumulated searched distance
                //searchedDistance += currentLane(direction).Length - (this.FrontPos);
                searchedDistance = this.RearPos;
                /// search in next segment
                vd = findVehBehindNext(this.currentSegment, searchedDistance, direction);
            }

            return vd;

            //VehicleDistance vd = null;

            ///// Search in current RoadSegment
            //if (direction == 0)
            //{
            //    if (this.vehiclesIndex > 0)
            //    {
            //        vd = new VehicleDistance(
            //                currentLane().vehicles[this.vehiclesIndex - 1],
            //                (this.distance - (this.length / 2)) - currentLane().vehicles[this.vehiclesIndex - 1].frontPos
            //            );
            //    }
            //}

            //if ((currentSegment.lanes.Count + 1 > state.laneIdx + direction) && (state.laneIdx + direction >= 0) && (vehiclesIndex != 0))
            //{
            //    if (currentLane(direction).vehicles.Count > 1)
            //    {
            //        int vdIdx = currentLane(direction).findVehicleBehind(this.distance);
            //        if (vdIdx != -1)
            //        {
            //            vd = new VehicleDistance(currentLane(direction).vehicles[vdIdx], this.RearPos - currentLane(direction).vehicles[vdIdx].FrontPos);
            //            return vd;
            //        }
            //    }
            //}

            //if (currentSegment.prevSegment.Count > 0)
            //{
            //    vd = findVehicleInBehindRecursive(currentSegment, 0, direction);
            //}


            #region old method
            //if ((currentSegment.lanes.Count + 1 > state.laneIdx + direction) && (state.laneIdx + direction >= 0) && (vehiclesIndex != 0))
            //{
            //    if (currentLane(direction).vehicles.Count > 1)
            //    {
            //        int vdIdx = currentLane(direction).findVehicleBehind(this.distance);
            //        if (vdIdx != -1)
            //        {
            //            vd = new VehicleDistance(currentLane(direction).vehicles[vdIdx], this.RearPos - currentLane(direction).vehicles[vdIdx].FrontPos);
            //            return vd;
            //        }
            //    }
            //}

            //if (currentSegment.prevSegment.Count > 0)
            //{
            //    vd = findVehicleInBehindRecursive(currentSegment, 0, direction);
            //}
            #endregion

            //return vd;
        }

        private VehicleDistance findVehBehindNext(RoadSegment currSegment, double searchedDistance, int direction = 0)
        {
            //VehicleDistance vd = null;
            VehicleDistance tempVd = new VehicleDistance(null, double.MaxValue);
            VehicleDistance tempVd2 = null;

            /// Foreach prevSegment
            for (int i = 0; i < currSegment.prevSegment.Count; i++)
            {

                if (searchedDistance < Constants.SearchRange)
                {
                    /// If there's target lane in prev segment
                    if (currSegment.prevSegment[i].lanes.Count > state.laneIdx + direction)
                    {
                        SegmentLane searchedLane = currSegment.prevSegment[i].lanes[state.laneIdx + direction];

                        /// If there's a veh in searched lane segment
                        if (searchedLane.vehicles.Count > 0)
                        {
                            double curDistance = searchedLane.Length - searchedLane.vehicles[searchedLane.vehicles.Count - 1].FrontPos;

                            /// If current found veh is nearer than previously found veh
                            if (curDistance < tempVd.distance)
                            {
                                tempVd.vehicle = searchedLane.vehicles[searchedLane.vehicles.Count - 1];
                                tempVd.distance = curDistance;
                            }
                        }else
                        {
                            tempVd2 = findVehBehindNext(currSegment.prevSegment[i], searchedDistance + searchedLane.Length, direction);

                            if (tempVd2.distance < tempVd.distance)
                            {
                                tempVd = tempVd2;
                            }
                        }

                    }
                }
            }

            return tempVd;
        }


        // TODO
        private VehicleDistance findVehicleInBehindRecursive(RoadSegment currSegment, double searchedDistance, int direction = 0)
        {
            VehicleDistance vd = null;
            double tempDistance = 168;

            /// Foreach prevSegment
            for (int i = 0; i < currSegment.prevSegment.Count; i++)
            {
                /// If there's target lane in prev segment
                if (currSegment.prevSegment[i].lanes.Count > state.laneIdx + direction)
                {
                    /// If there's a vehicle in target lane in prev segment
                    if (currSegment.prevSegment[i].lanes[state.laneIdx + direction].vehicles.Count > 0)
                    {
                        tempDistance = this.RearPos +
                            currSegment.prevSegment[i].Length -
                            currSegment.prevSegment[i].lanes[state.laneIdx + direction].vehicles[currSegment.prevSegment[i].lanes[state.laneIdx + direction].vehicles.Count - 1].FrontPos;

                        /// Only save the result if the vehicle is closer
                        if ((vd == null)||(tempDistance < vd.distance))                                   
                        {
                            /// Return furthest vehicle
                            vd = new VehicleDistance(null, 0);
                            vd.vehicle = currSegment.prevSegment[i].lanes[state.laneIdx + direction].vehicles[currSegment.prevSegment[i].lanes[state.laneIdx + direction].vehicles.Count - 1];
                            vd.distance = tempDistance;
                        }
                    }
                    /// Search for prev segment in prev segment :p
                    else
                    {
                        searchedDistance += currentSegment.Length;
                        vd = findVehicleInBehindRecursive(currSegment.prevSegment[i], searchedDistance, direction);
                    }
                }
            }

            return vd;
        }

        /// <summary>
		/// Searches for the next TrafficLight on the vehicle's route within the given distance.
		/// </summary>
		/// <param name="route">Route of the Vehicle.</param>
		/// <param name="arcPos">Current arc position of the vehicle on the first NodeConnection on <paramref name="route"/>.</param>
		/// <param name="distance">Distance to cover during search.</param>
		/// <param name="considerOnlyRed">If true, only red traffic lights will be considered.</param>
		/// <returns>The distance to the next TrafficLight on the vehicle's route that covers the given constraints. <paramref name="distance"/> if no such TrafficLight exists.</returns>
		private double GetDistanceToNextTrafficLightOnRoute(List<RoadSegment> route, double arcPos, double distance, bool considerOnlyRed)
        {
            //Debug.Assert(route.Count > 0);

            //double doneDistance = -arcPos;
            //for(int i=route.Count - 1; i < 0; i--)
            //{
            //    doneDistance += route[i].Length;
            //    if (doneDistance >= distance)
            //        return distance;

            //    if (route[i].endNode.tLight != null)
            //    {
            //        if(route[i].endNode.tLight.trafficLightState == TrafficLight.State.RED)
            //        {
            //            return doneDistance;
            //        }
            //    }
            //    //if (ws.endNode.tLight != null && (!considerOnlyRed || ws.endNode.tLight.trafficLightState == TrafficLight.State.RED))
            //    //    return doneDistance;
            //}

            //return distance;

            double toReturn = distance;
            double searchedDistance = 0;

            while(searchedDistance < Constants.SearchRange)
            {
                for(int i=0; i < route.Count; i++)
                {
                    if(route[i].endNode.tLight != null)
                    {
                        if (route[i].endNode.tLight.trafficLightState == TrafficLight.State.RED)
                        {
                            toReturn = route[i].Length + searchedDistance - this.FrontPos;// (this.distance + this.length / 2);
                            return toReturn;
                        }
                            
                    }else
                    {
                        searchedDistance += route[i].Length;
                    }
                }

            }

            return toReturn;
        }


        public void Accelerate(double newAcceleration)
        {
            _physics.acceleration = newAcceleration;
        }

        public string logAcc  = "";
        public string logVel  = "";
        public string logDist = "";
        public string logTime = "";
        double totalDist = 0;
        double roadDist = 0;
        double prevDist = 0;

        public void LogVeh(double totalTime)
        {
            logTime += totalTime + ";";
            logAcc += physics.acceleration + ";";
            logVel += physics.velocity + ";";


            if (distance + roadDist < totalDist)
            {
                logTime += "";
            }

            totalDist = distance + roadDist;

            logDist += totalDist + ";";
            prevDist = distance;

        }


        #region move
        public void Move(double tickLength)
        {
            if (!alreadyMoved)
            {
                _physics.velocity += _physics.acceleration * tickLength;

                if (_physics.velocity < 0)
                    _physics.velocity = 0;

                distance += _physics.velocity * tickLength;

                #region do lane change
                //if (inProcessOfLC)
                //{
                //    if (tLaneChangeDelay == 0)
                //    {
                //        doLaneChange(tickLength);
                //        //currentLane(state.targetLaneIdx - state.laneIdx).AddVehicle(this);
                //        //updateLaneChangeDelay(tickLength);
                //    }
                //    else
                //    {
                //        updateLaneChangeDelay(tickLength);
                //    }
                //}
                //else
                //{
                //    if (lastTimeLc <= 5)
                //    {
                //        lastTimeLCTick(tickLength);
                //    }
                //}
                #endregion

                //Debug.WriteLine(hashcode + " - " + distance);

                #region vehicle change segment
                /// Check if vehicles move past the segment
                if (distance >= state.currentSegment.Length)
                {
                    double newDistance = distance - currentSegment.Length;
                    if (Routing.Route.Count > 0)
                    {
                        Routing.Route.RemoveFirst();
                        if (Routing.Route.Count > 0)
                        {
                            /// test log
                            roadDist += currentLane().Length;

                            if (newDistance > Routing.Route.First.Value.Length)
                            {
                                roadDist += Routing.Route.First.Value.Length;
                                newDistance = newDistance - Routing.Route.First.Value.Length;
                                Routing.Route.RemoveFirst();
                                RemoveFromCurrentSegment(Routing.Route.First.Value, newDistance);
                            }
                            else
                            {
                                RemoveFromCurrentSegment(Routing.Route.First.Value, newDistance);
                            }
                        }
                        else
                        {
                            RemoveFromCurrentSegment(null, 0);
                        }
                    }
                    else
                    {
                        RemoveFromCurrentSegment(null, 0);
                    }
                }else
                {
                    newCoord();
                }
                #endregion
               

                alreadyMoved = true;
            }
        }

        public void Reset()
        {
            alreadyMoved = false;
        }
        #endregion

        #endregion

        private void RemoveFromCurrentSegment(RoadSegment nextSegment, double startPosition)
        {
            currentLane().vehToRemove.Add(this);

            if ((nextSegment != null) && (nextSegment.lanes.Count > state.laneIdx))
            {
                distance = startPosition;
                nextSegment.lanes[state.laneIdx].vehicles.Add(this);                
                _state.currentSegment = nextSegment;
                newCoord();
                RotateVehicle(currentSegment.startNode, currentSegment.endNode);
            }
            else
            {
                // Vehicle died
            }
        }

        public void newCoord()
        {
            Vector2 difference = currentLane().endNode.Position - currentLane().startNode.Position;

            PointF toReturn = new PointF();

            //float segmentLength = (float) Vector2.GetDistance(start, end);


            if (inProcessOfLC)
            {

                PointF offset = new PointF();
                int direction = state.laneIdx - state.prevLaneIdx;
                double angle = Vector2.AngleBetween(getLane(state.prevLaneIdx).endNode.Position, getLane(state.prevLaneIdx).startNode.Position);

                // TODO : Make it based on current / new lane, not prev lane. The lane shift is based on substraction
                toReturn.X = (float)difference.X * (float)(this.distance / getLane(state.laneIdx).Length) +
                                (float)getLane(state.laneIdx).startNode.Position.X;
                toReturn.Y = (float)difference.Y * (float)(this.distance / getLane(state.laneIdx).Length) +
                                (float)getLane(state.laneIdx).startNode.Position.Y;

                if (angle >= 0)
                {
                    offset.X = ((float)currentSegment.laneWidth * (float)(1 - tLaneChangeDelay) * direction) * (float)Math.Sin(angle);
                    offset.Y = ((float)currentSegment.laneWidth * (float)(1 - tLaneChangeDelay) * direction) * (float)Math.Cos(angle);

                }else
                {
                    offset.X = ((float)currentSegment.laneWidth * (float)(1 - tLaneChangeDelay) * -direction) * (float)Math.Sin(angle);
                    offset.Y = ((float)currentSegment.laneWidth * (float)(1 - tLaneChangeDelay) * -direction) * (float)Math.Cos(angle);
                }

                toReturn.X += offset.X;
                toReturn.Y += offset.Y;
            }else
            {
                toReturn.X = (float) difference.X * (float) (this.distance / currentLane().Length) +
                    (float) currentLane().startNode.Position.X;
                toReturn.Y = (float) difference.Y * (float) (this.distance / currentLane().Length) + 
                    (float) currentLane().startNode.Position.Y;
            }


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



        public SegmentLane currentLane(int direction = 0)
        {
            return currentSegment.lanes[state.laneIdx + direction];
        }

        public SegmentLane getLane(int lane)
        {
            if (currentSegment.lanes.Count > lane)
            {
                return currentSegment.lanes[lane];
            }else
            {
                return null;
            }
        }

    }
}
