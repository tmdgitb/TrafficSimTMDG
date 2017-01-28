using SimTMDG.Road;
using SimTMDG.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimTMDG.Vehicle
{
    public class VehicleGenerator
    {
        #region class attribute
        //int activeVehicle;
        double q_in = 0;
        List<double> q_outs;
        RoadSegment startSegment;
        List<RoadSegment> endSegments;
        double inVehBuffer = 0;
        //List<RoadSegment> route;// = new List<RoadSegment>();
        static Random rnd = new Random();

        public enum VehRouteType
        {
            /// <summary>
            /// default: Normal Car / Bus / Truck
            /// </summary>
            DEFAULT,

            /// <summary>
            /// rote Ampel
            /// </summary>
            FIXED
        }

        /// <summary>
        /// Type of vehicle route: normal or routed (e.g. Angkot / Damri)
        /// </summary>
        private VehRouteType _vehRouteType;
        /// <summary>
        /// Type of vehicle route: normal or routed (e.g. Angkot / Damri)
        /// </summary>
        public VehRouteType vehRouteType
        {
            get { return _vehRouteType; }
            set { _vehRouteType = value; }
        }


        /// Temp
        public List<RoadSegment> segments;
        public List<List<RoadSegment>> routes = new List<List<RoadSegment>>();
        List<AStar> astars;
        #endregion




        #region constructor
        public VehicleGenerator()
        {

        }

        public VehicleGenerator(List<RoadSegment> _segments, RoadSegment _startSegment, double _q_in, List<RoadSegment> _endSegments, List<double> _q_outs, VehRouteType vehRouteType)
        {
            this.q_in = _q_in;
            this.segments = _segments;
            this.startSegment = _startSegment;
            this.endSegments = _endSegments;

            this.vehRouteType = vehRouteType;

            for (int i = 0; i < _endSegments.Count; i++)
            {
                routes.Add(findRoute(startSegment, endSegments[i]));
            }

            this.q_outs = _q_outs;
        }
        #endregion

        public List<RoadSegment> generatePath(AStar astar, RoadSegment start, RoadSegment end)
        {
            List<RoadSegment> toReturn = new List<RoadSegment>();
            RoadSegment current = end;
            toReturn.Add(current);

            while (current != start)
            {
                RoadSegment value;
                astar.cameFrom.TryGetValue(current, out value);

                current = value;
                toReturn.Add(current);
            }

            toReturn.Reverse();


            return toReturn;
        }



        #region tick
        /// <summary>
        /// The basic Idea is, we add a number to inVehBuffer (q_in which is veh/s * tickLength which is 1s/fps)
        /// Let's say we need to add 2 veh / s, with tickLength 0.25s ==> the inVehBuffer will adds 0.5 veh per tick
        /// Then we generate 1 vehicle every tick IF ONLY we have 1 or more inVehBuffer
        /// With this we generate a vehicle every 2 tick or 2 vehicles every 4 tick, with tickLength 0.25s it means 2 veh / s
        /// </summary>
        public int generate(double tickLength, NodeControl nc)
        {
            int toReturn = 0;
            int destination = 0;
            /// TODO add maximum generation
            /// If vehicle generated from this function is exceeding the max_gen, stop to add this to inVehBuffer
            inVehBuffer += q_in * tickLength;

            if (inVehBuffer >= 1)
            {
                if (startSegment.lanes.Count > 1)
                {
                    //int laneidx = rnd.Next(0, startSegment.lanes.Count); /// TODO replace with shuffled list of lane
                    List<int> laneIdxList = new List<int>();

                    for (int i = 0; i < startSegment.lanes.Count; i++)
                    {
                        laneIdxList.Add(i);
                    }

                    Shuffle<int>(laneIdxList);

                    for (int i = 0; i < startSegment.lanes.Count; i++)
                    {
                        SegmentLane choosenLane = startSegment.lanes[laneIdxList[i]];
                        List<IVehicle> laneVehicles = choosenLane.vehicles;

                        /// If lane is empty OR there's a minimum 10m space between start node and last vehicle's rear position
                        if ((laneVehicles.Count == 0) ||
                            (laneVehicles[0].RearPos > laneVehicles[0].sstar + 7))
                        {
                            double destRnd = rnd.NextDouble();
                            double q_outSum = 0.0;

                            for (int dest = 0; dest < endSegments.Count; dest++)
                            {
                                q_outSum += q_outs[dest];

                                if ((dest == 0))
                                {
                                    if (destRnd < q_outs[0]){
                                        destination = dest;
                                    }
                                }
                                else
                                {
                                    if ((destRnd >= q_outs[dest - 1]) && (destRnd < q_outSum))
                                    {
                                        destination = dest;
                                    }
                                }                                
                            }

                            //success = true;

                            //route = routes[destination];
                            //route = findRoute(this.startSegment, this.endSegments[destination]);
                            vehGenerate(laneIdxList[i], destination, nc);
                            toReturn = 1;
                            return toReturn;
                            //break;
                        }
                    }
                    
                }
            }

            return toReturn;
        }


        void vehGenerate(int laneidx, int destination, NodeControl nc)
        {
            IVehicle v = null;
            if ((nc.ActiveVehicles > 30) && (nc.unusedVehicles.Count > 0))
            {
                v = nc.unusedVehicles[0];
                nc.unusedVehicles.RemoveAt(0);

                v.Reuse(startSegment, laneidx, routes[destination]);

                v.color = System.Drawing.Color.Pink;

            }
            else
            {
                /// Generate Vehicle
                int vehType = rnd.Next(0, 2);

                if (vehType == 0)
                {
                    v = new Car(startSegment, laneidx, routes[destination]);
                }
                else if (vehType == 1)
                {
                    v = new Bus(startSegment, laneidx, routes[destination]);
                }
                else
                {
                    v = new Truck(startSegment, laneidx, routes[destination]);
                }
            }


            startSegment.lanes[laneidx].vehicles.Add(v);
            //activeVehicle++;
            inVehBuffer--;
        }
        #endregion




        #region routing
        List<RoadSegment> findRoute(RoadSegment start, RoadSegment end)
        {
            List<RoadSegment> toReturn = new List<RoadSegment>();

            if (this.vehRouteType == VehRouteType.DEFAULT)
            {
                #region AStar route
                AStar newAStar = new AStar(this.segments, start, end);
                if (newAStar.goalIsFound)
                {
                    toReturn = generatePath(newAStar, start, end);
                }
                #endregion
            }else
            {
                #region manual route
                ///// Manual Route
                /// Route 5: coba
                if ((start.Id == 1533) && (end.Id == 1510))
                {
                    for (int i = 1533; i > 1509; i--)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    #region temp
                    //toReturn.Add(segments.Find(x => x.Id == ));

                    //for (int i = ; i < ; i++)
                    //{
                    //    toReturn.Add(segments.Find(x => x.Id == i));
                    //}
                    //for (int i = ; i < ; i++)
                    //{
                    //    toReturn.Add(segments.Find(x => x.Id == i));
                    //}
                    //for (int i = ; i < ; i++)
                    //{
                    //    toReturn.Add(segments.Find(x => x.Id == i));
                    //}
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));


                    //toReturn.Add(segments.Find(x => x.Id == ));
                    //toReturn.Add(segments.Find(x => x.Id == ));
                    #endregion
                }

                ///Route 4: Stasiun - Dago
                else if ((start.Id == 19684) && (end.Id == 28804))
                {
                    #region 03StasiunDAgo
                    for (int i = 19684; i < 19728; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 28034; i < 28038; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 33622; i < 33627; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 29103; i < 29117; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 29098; i < 29101; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 30018; i < 30022; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 353; i < 369; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 0));
                    toReturn.Add(segments.Find(x => x.Id == 1));
                    for (int i = 26962; i < 26965; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 2772; i < 2775; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 23273));
                    for (int i = 349; i < 353; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 989));
                    for (int i = 23264; i < 23273; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 21915));
                    toReturn.Add(segments.Find(x => x.Id == 34249));
                    toReturn.Add(segments.Find(x => x.Id == 34250));
                    toReturn.Add(segments.Find(x => x.Id == 987));
                    toReturn.Add(segments.Find(x => x.Id == 988));
                    toReturn.Add(segments.Find(x => x.Id == 975));
                    toReturn.Add(segments.Find(x => x.Id == 961));
                    toReturn.Add(segments.Find(x => x.Id == 964));
                    toReturn.Add(segments.Find(x => x.Id == 967));
                    toReturn.Add(segments.Find(x => x.Id == 970));
                    toReturn.Add(segments.Find(x => x.Id == 25225));
                    toReturn.Add(segments.Find(x => x.Id == 25231));
                    toReturn.Add(segments.Find(x => x.Id == 25234));
                    toReturn.Add(segments.Find(x => x.Id == 25237));
                    toReturn.Add(segments.Find(x => x.Id == 25341));
                    toReturn.Add(segments.Find(x => x.Id == 27742));
                    toReturn.Add(segments.Find(x => x.Id == 27745));
                    for (int i = 25261; i < 25264; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 237));
                    toReturn.Add(segments.Find(x => x.Id == 238));
                    for (int i = 250; i < 254; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 27901));
                    toReturn.Add(segments.Find(x => x.Id == 27731));
                    toReturn.Add(segments.Find(x => x.Id == 30022));
                    toReturn.Add(segments.Find(x => x.Id == 987));
                    toReturn.Add(segments.Find(x => x.Id == 988));
                    for (int i = 975; i < 987; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 28629));
                    toReturn.Add(segments.Find(x => x.Id == 681));
                    toReturn.Add(segments.Find(x => x.Id == 682));
                    for (int i = 2764; i < 2767; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 683; i < 691; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 29526));
                    for (int i = 28150; i < 28155; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 23283));
                    toReturn.Add(segments.Find(x => x.Id == 26115));
                    for (int i = 28089; i < 28094; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 35035));
                    for (int i = 23070; i < 23077; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 10));
                    toReturn.Add(segments.Find(x => x.Id == 11));
                    toReturn.Add(segments.Find(x => x.Id == 28045));

                    for (int i = 9311; i < 9323; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 9326; i < 9331; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 27121; i < 27126; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 28805; i < 28808; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 28038));
                    for (int i = 28724; i < 28804; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }


                    #endregion
                }

                /// Route 3: Cicaheum - Ledeng
                else if ((start.Id == 27149) && (end.Id == 33168))
                {
                    #region Cicaheum Ledeng
                    toReturn.Add(segments.Find(x => x.Id == 27149));
                    toReturn.Add(segments.Find(x => x.Id == 27151));
                    toReturn.Add(segments.Find(x => x.Id == 27155));
                    toReturn.Add(segments.Find(x => x.Id == 27157));
                    toReturn.Add(segments.Find(x => x.Id == 27169));
                    toReturn.Add(segments.Find(x => x.Id == 27172));
                    toReturn.Add(segments.Find(x => x.Id == 27178));
                    toReturn.Add(segments.Find(x => x.Id == 27181));
                    toReturn.Add(segments.Find(x => x.Id == 27188));
                    toReturn.Add(segments.Find(x => x.Id == 27191));
                    toReturn.Add(segments.Find(x => x.Id == 27194));
                    toReturn.Add(segments.Find(x => x.Id == 27197));
                    toReturn.Add(segments.Find(x => x.Id == 27203));
                    toReturn.Add(segments.Find(x => x.Id == 27206));
                    toReturn.Add(segments.Find(x => x.Id == 27209));
                    toReturn.Add(segments.Find(x => x.Id == 27212));
                    toReturn.Add(segments.Find(x => x.Id == 27215));
                    toReturn.Add(segments.Find(x => x.Id == 27218));
                    toReturn.Add(segments.Find(x => x.Id == 27221));
                    toReturn.Add(segments.Find(x => x.Id == 27224));
                    toReturn.Add(segments.Find(x => x.Id == 27227));
                    toReturn.Add(segments.Find(x => x.Id == 27230));
                    toReturn.Add(segments.Find(x => x.Id == 27236));
                    toReturn.Add(segments.Find(x => x.Id == 27238));
                    toReturn.Add(segments.Find(x => x.Id == 27241));
                    toReturn.Add(segments.Find(x => x.Id == 27244));
                    toReturn.Add(segments.Find(x => x.Id == 27250));
                    toReturn.Add(segments.Find(x => x.Id == 27253));
                    toReturn.Add(segments.Find(x => x.Id == 27259));
                    toReturn.Add(segments.Find(x => x.Id == 27262));
                    toReturn.Add(segments.Find(x => x.Id == 27265));
                    toReturn.Add(segments.Find(x => x.Id == 27268));
                    toReturn.Add(segments.Find(x => x.Id == 27271));
                    toReturn.Add(segments.Find(x => x.Id == 27274));
                    for (int i = 23183; i < 23193; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 9135));
                    toReturn.Add(segments.Find(x => x.Id == 9132));
                    toReturn.Add(segments.Find(x => x.Id == 9126));
                    toReturn.Add(segments.Find(x => x.Id == 9123));
                    toReturn.Add(segments.Find(x => x.Id == 9120));
                    toReturn.Add(segments.Find(x => x.Id == 9117));
                    toReturn.Add(segments.Find(x => x.Id == 9114));
                    toReturn.Add(segments.Find(x => x.Id == 1426));
                    toReturn.Add(segments.Find(x => x.Id == 1427));
                    for (int i = 27715; i < 27722; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    //pertigaan ciumbeuluit
                    for (int i = 9146; i < 9152; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 33262));
                    toReturn.Add(segments.Find(x => x.Id == 33260));
                    toReturn.Add(segments.Find(x => x.Id == 20768));
                    toReturn.Add(segments.Find(x => x.Id == 20765));
                    toReturn.Add(segments.Find(x => x.Id == 20762));
                    toReturn.Add(segments.Find(x => x.Id == 20759));
                    toReturn.Add(segments.Find(x => x.Id == 20756));
                    toReturn.Add(segments.Find(x => x.Id == 9221));
                    toReturn.Add(segments.Find(x => x.Id == 9218));
                    toReturn.Add(segments.Find(x => x.Id == 9215));
                    toReturn.Add(segments.Find(x => x.Id == 9212));
                    toReturn.Add(segments.Find(x => x.Id == 9209));
                    toReturn.Add(segments.Find(x => x.Id == 9206));
                    toReturn.Add(segments.Find(x => x.Id == 9203));
                    //pertigaan cisitu
                    for (int i = 1533; i > 1509; i--)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 643));
                    toReturn.Add(segments.Find(x => x.Id == 640));
                    toReturn.Add(segments.Find(x => x.Id == 637));
                    toReturn.Add(segments.Find(x => x.Id == 631));
                    toReturn.Add(segments.Find(x => x.Id == 628));
                    toReturn.Add(segments.Find(x => x.Id == 619));
                    toReturn.Add(segments.Find(x => x.Id == 616));
                    toReturn.Add(segments.Find(x => x.Id == 613));
                    toReturn.Add(segments.Find(x => x.Id == 610));
                    toReturn.Add(segments.Find(x => x.Id == 28822));
                    toReturn.Add(segments.Find(x => x.Id == 28819));
                    toReturn.Add(segments.Find(x => x.Id == 28816));
                    toReturn.Add(segments.Find(x => x.Id == 28812));
                    toReturn.Add(segments.Find(x => x.Id == 28809));
                    toReturn.Add(segments.Find(x => x.Id == 9187));
                    toReturn.Add(segments.Find(x => x.Id == 9183));
                    toReturn.Add(segments.Find(x => x.Id == 9180));
                    toReturn.Add(segments.Find(x => x.Id == 9177));
                    toReturn.Add(segments.Find(x => x.Id == 9174));
                    toReturn.Add(segments.Find(x => x.Id == 9171));
                    toReturn.Add(segments.Find(x => x.Id == 9168));
                    toReturn.Add(segments.Find(x => x.Id == 9165));
                    toReturn.Add(segments.Find(x => x.Id == 9188));
                    //baltos
                    toReturn.Add(segments.Find(x => x.Id == 664));
                    toReturn.Add(segments.Find(x => x.Id == 661));
                    toReturn.Add(segments.Find(x => x.Id == 658));
                    toReturn.Add(segments.Find(x => x.Id == 654));
                    toReturn.Add(segments.Find(x => x.Id == 651));
                    toReturn.Add(segments.Find(x => x.Id == 648));
                    toReturn.Add(segments.Find(x => x.Id == 645));
                    toReturn.Add(segments.Find(x => x.Id == 28));
                    toReturn.Add(segments.Find(x => x.Id == 31));
                    toReturn.Add(segments.Find(x => x.Id == 34));
                    toReturn.Add(segments.Find(x => x.Id == 37));
                    toReturn.Add(segments.Find(x => x.Id == 40));
                    toReturn.Add(segments.Find(x => x.Id == 43));
                    //dukomsel
                    toReturn.Add(segments.Find(x => x.Id == 377));
                    toReturn.Add(segments.Find(x => x.Id == 380));
                    toReturn.Add(segments.Find(x => x.Id == 383));
                    toReturn.Add(segments.Find(x => x.Id == 386));
                    toReturn.Add(segments.Find(x => x.Id == 389));
                    toReturn.Add(segments.Find(x => x.Id == 27119));
                    toReturn.Add(segments.Find(x => x.Id == 30273));
                    toReturn.Add(segments.Find(x => x.Id == 27110));
                    toReturn.Add(segments.Find(x => x.Id == 27113));
                    toReturn.Add(segments.Find(x => x.Id == 27116));
                    //pusdai
                    toReturn.Add(segments.Find(x => x.Id == 9463));
                    toReturn.Add(segments.Find(x => x.Id == 9464));
                    for (int i = 33046; i < 33055; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    //wr supratman
                    for (int i = 24522; i < 24530; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 23093));
                    toReturn.Add(segments.Find(x => x.Id == 23094));
                    toReturn.Add(segments.Find(x => x.Id == 23095));
                    toReturn.Add(segments.Find(x => x.Id == 30485));
                    toReturn.Add(segments.Find(x => x.Id == 30488));
                    toReturn.Add(segments.Find(x => x.Id == 30491));
                    toReturn.Add(segments.Find(x => x.Id == 30494));
                    toReturn.Add(segments.Find(x => x.Id == 30500));
                    toReturn.Add(segments.Find(x => x.Id == 30506));
                    toReturn.Add(segments.Find(x => x.Id == 30512));
                    toReturn.Add(segments.Find(x => x.Id == 30515));
                    toReturn.Add(segments.Find(x => x.Id == 26130));
                    toReturn.Add(segments.Find(x => x.Id == 26133));
                    toReturn.Add(segments.Find(x => x.Id == 26136));
                    toReturn.Add(segments.Find(x => x.Id == 26139));
                    toReturn.Add(segments.Find(x => x.Id == 26142));
                    toReturn.Add(segments.Find(x => x.Id == 26144));
                    toReturn.Add(segments.Find(x => x.Id == 26147));
                    toReturn.Add(segments.Find(x => x.Id == 26150));
                    toReturn.Add(segments.Find(x => x.Id == 26153));
                    toReturn.Add(segments.Find(x => x.Id == 1167));
                    toReturn.Add(segments.Find(x => x.Id == 1170));
                    toReturn.Add(segments.Find(x => x.Id == 1185));
                    toReturn.Add(segments.Find(x => x.Id == 1188));
                    toReturn.Add(segments.Find(x => x.Id == 1194));
                    toReturn.Add(segments.Find(x => x.Id == 28232));
                    toReturn.Add(segments.Find(x => x.Id == 28235));
                    toReturn.Add(segments.Find(x => x.Id == 2604));
                    toReturn.Add(segments.Find(x => x.Id == 30222));
                    toReturn.Add(segments.Find(x => x.Id == 30225));
                    toReturn.Add(segments.Find(x => x.Id == 30228));
                    toReturn.Add(segments.Find(x => x.Id == 30240));
                    toReturn.Add(segments.Find(x => x.Id == 30243));
                    toReturn.Add(segments.Find(x => x.Id == 38625));
                    toReturn.Add(segments.Find(x => x.Id == 38631));
                    toReturn.Add(segments.Find(x => x.Id == 38634));
                    toReturn.Add(segments.Find(x => x.Id == 38640));
                    toReturn.Add(segments.Find(x => x.Id == 38643));
                    toReturn.Add(segments.Find(x => x.Id == 38646));
                    toReturn.Add(segments.Find(x => x.Id == 24032));
                    toReturn.Add(segments.Find(x => x.Id == 27695));
                    toReturn.Add(segments.Find(x => x.Id == 33162));
                    toReturn.Add(segments.Find(x => x.Id == 33165));
                    toReturn.Add(segments.Find(x => x.Id == 33168));

                    #endregion
                }

                ///Route 2: Angkot Kelapa Dago
                else if ((start.Id == 19683) && (end.Id == 28803))
                {
                    #region 02KelapaDago
                    for (int i = 19683; i < 19728; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 28034; i < 28038; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 33622; i < 33627; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 29103; i < 29117; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 29098; i < 29101; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 30018; i < 30022; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 353; i < 369; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 0));
                    toReturn.Add(segments.Find(x => x.Id == 1));
                    toReturn.Add(segments.Find(x => x.Id == 26966));
                    toReturn.Add(segments.Find(x => x.Id == 26969));
                    toReturn.Add(segments.Find(x => x.Id == 27015));
                    toReturn.Add(segments.Find(x => x.Id == 21495));
                    toReturn.Add(segments.Find(x => x.Id == 21307));
                    toReturn.Add(segments.Find(x => x.Id == 21304));
                    toReturn.Add(segments.Find(x => x.Id == 26981));
                    for (int i = 21262; i < 21266; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 21119; i < 21126; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 20990));
                    toReturn.Add(segments.Find(x => x.Id == 21055));
                    for (int i = 84; i < 89; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 21162));
                    for (int i = 34279; i < 34285; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 1292));
                    for (int i = 34276; i < 34279; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 8815; i < 8819; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 29660; i < 29663; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 20308; i < 20315; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 7806; i < 7810; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 29659));
                    toReturn.Add(segments.Find(x => x.Id == 29656));
                    toReturn.Add(segments.Find(x => x.Id == 29653));
                    toReturn.Add(segments.Find(x => x.Id == 29650));
                    toReturn.Add(segments.Find(x => x.Id == 29647));
                    toReturn.Add(segments.Find(x => x.Id == 29644));
                    toReturn.Add(segments.Find(x => x.Id == 29641));
                    toReturn.Add(segments.Find(x => x.Id == 29638));
                    toReturn.Add(segments.Find(x => x.Id == 29635));
                    toReturn.Add(segments.Find(x => x.Id == 29632));
                    toReturn.Add(segments.Find(x => x.Id == 29629));
                    toReturn.Add(segments.Find(x => x.Id == 24873));
                    toReturn.Add(segments.Find(x => x.Id == 24870));
                    toReturn.Add(segments.Find(x => x.Id == 24864));
                    toReturn.Add(segments.Find(x => x.Id == 24861));
                    toReturn.Add(segments.Find(x => x.Id == 24858));
                    toReturn.Add(segments.Find(x => x.Id == 1505));
                    toReturn.Add(segments.Find(x => x.Id == 20861));
                    toReturn.Add(segments.Find(x => x.Id == 20864));
                    toReturn.Add(segments.Find(x => x.Id == 20867));
                    toReturn.Add(segments.Find(x => x.Id == 33244));
                    toReturn.Add(segments.Find(x => x.Id == 33245));
                    for (int i = 34297; i < 34299; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 34286; i < 34291; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 24874; i < 24877; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 34296));
                    toReturn.Add(segments.Find(x => x.Id == 30056));
                    toReturn.Add(segments.Find(x => x.Id == 30057));
                    for (int i = 29527; i < 29534; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 21410));
                    toReturn.Add(segments.Find(x => x.Id == 21412));
                    toReturn.Add(segments.Find(x => x.Id == 21415));
                    toReturn.Add(segments.Find(x => x.Id == 28144));
                    toReturn.Add(segments.Find(x => x.Id == 27006));
                    toReturn.Add(segments.Find(x => x.Id == 1862));
                    toReturn.Add(segments.Find(x => x.Id == 38350));
                    toReturn.Add(segments.Find(x => x.Id == 27036));
                    toReturn.Add(segments.Find(x => x.Id == 27039));
                    toReturn.Add(segments.Find(x => x.Id == 27042));
                    toReturn.Add(segments.Find(x => x.Id == 27045));
                    toReturn.Add(segments.Find(x => x.Id == 27048));
                    for (int i = 9311; i < 9323; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 9326; i < 9331; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 27121; i < 27126; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 28805; i < 28808; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 28038));
                    for (int i = 28724; i < 28805; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    #endregion
                }

                /// Route 1: Angkot Cibaduyut - Kr Setra
                else if ((start.Id == 22825) && (end.Id == 529))
                {
                    #region 01CibaduyutKarangsetra
                    for (int i = 22825; i < 22843; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 29606; i < 29612; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 20915));
                    for (int i = 23106; i < 23110; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 27930; i < 27935; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 27783));
                    toReturn.Add(segments.Find(x => x.Id == 27784));
                    toReturn.Add(segments.Find(x => x.Id == 27808));
                    toReturn.Add(segments.Find(x => x.Id == 27805));
                    toReturn.Add(segments.Find(x => x.Id == 27802));
                    toReturn.Add(segments.Find(x => x.Id == 27799));
                    toReturn.Add(segments.Find(x => x.Id == 27796));
                    toReturn.Add(segments.Find(x => x.Id == 27790));
                    toReturn.Add(segments.Find(x => x.Id == 27787));
                    toReturn.Add(segments.Find(x => x.Id == 27817));
                    toReturn.Add(segments.Find(x => x.Id == 27814));
                    toReturn.Add(segments.Find(x => x.Id == 27811));
                    toReturn.Add(segments.Find(x => x.Id == 27913));
                    toReturn.Add(segments.Find(x => x.Id == 27910));
                    toReturn.Add(segments.Find(x => x.Id == 27907));
                    toReturn.Add(segments.Find(x => x.Id == 283));
                    toReturn.Add(segments.Find(x => x.Id == 256));
                    toReturn.Add(segments.Find(x => x.Id == 258));
                    toReturn.Add(segments.Find(x => x.Id == 261));
                    for (int i = 23081; i < 23083; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 25257));
                    for (int i = 25253; i < 25257; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 33342));
                    toReturn.Add(segments.Find(x => x.Id == 25251));
                    for (int i = 33334; i < 33341; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    for (int i = 22164; i < 22169; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 22181));
                    toReturn.Add(segments.Find(x => x.Id == 34144));
                    toReturn.Add(segments.Find(x => x.Id == 34147));
                    toReturn.Add(segments.Find(x => x.Id == 34150));
                    toReturn.Add(segments.Find(x => x.Id == 34153));
                    toReturn.Add(segments.Find(x => x.Id == 33288));
                    toReturn.Add(segments.Find(x => x.Id == 33292));
                    toReturn.Add(segments.Find(x => x.Id == 33294));
                    toReturn.Add(segments.Find(x => x.Id == 28101));
                    toReturn.Add(segments.Find(x => x.Id == 28607));
                    toReturn.Add(segments.Find(x => x.Id == 28605));
                    toReturn.Add(segments.Find(x => x.Id == 34206));
                    toReturn.Add(segments.Find(x => x.Id == 34205));
                    toReturn.Add(segments.Find(x => x.Id == 23111));
                    toReturn.Add(segments.Find(x => x.Id == 24741));
                    toReturn.Add(segments.Find(x => x.Id == 24738));
                    toReturn.Add(segments.Find(x => x.Id == 575));
                    toReturn.Add(segments.Find(x => x.Id == 34106));
                    toReturn.Add(segments.Find(x => x.Id == 34103));
                    toReturn.Add(segments.Find(x => x.Id == 34100));
                    toReturn.Add(segments.Find(x => x.Id == 34097));
                    toReturn.Add(segments.Find(x => x.Id == 34094));
                    toReturn.Add(segments.Find(x => x.Id == 34124));
                    toReturn.Add(segments.Find(x => x.Id == 34121));
                    toReturn.Add(segments.Find(x => x.Id == 34118));
                    toReturn.Add(segments.Find(x => x.Id == 34115));
                    toReturn.Add(segments.Find(x => x.Id == 34112));
                    toReturn.Add(segments.Find(x => x.Id == 34109));
                    toReturn.Add(segments.Find(x => x.Id == 578));
                    for (int i = 28095; i < 28098; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                    toReturn.Add(segments.Find(x => x.Id == 34204));
                    toReturn.Add(segments.Find(x => x.Id == 28504));
                    toReturn.Add(segments.Find(x => x.Id == 28501));
                    toReturn.Add(segments.Find(x => x.Id == 8984));
                    toReturn.Add(segments.Find(x => x.Id == 8981));
                    toReturn.Add(segments.Find(x => x.Id == 8978));
                    toReturn.Add(segments.Find(x => x.Id == 8975));
                    toReturn.Add(segments.Find(x => x.Id == 8972));
                    toReturn.Add(segments.Find(x => x.Id == 8969));
                    toReturn.Add(segments.Find(x => x.Id == 23087));
                    toReturn.Add(segments.Find(x => x.Id == 550));
                    toReturn.Add(segments.Find(x => x.Id == 547));
                    toReturn.Add(segments.Find(x => x.Id == 544));
                    toReturn.Add(segments.Find(x => x.Id == 540));
                    toReturn.Add(segments.Find(x => x.Id == 538));
                    toReturn.Add(segments.Find(x => x.Id == 535));
                    toReturn.Add(segments.Find(x => x.Id == 532));
                    toReturn.Add(segments.Find(x => x.Id == 529));
                    #endregion
                }

                /// Route 0: Pasteur
                #region Pasteur
                else if ((start.Id == 2908) && (end.Id == 430))
                {
                    for (int i = 2908; i < 2926; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    toReturn.Add(segments.Find(x => x.Id == 421));

                    for (int i = 28456; i < 28469; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 422; i < 431; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                }
                else if ((start.Id == 2908) && (end.Id == 28600))
                {
                    for (int i = 2908; i < 2926; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    toReturn.Add(segments.Find(x => x.Id == 26040));
                    toReturn.Add(segments.Find(x => x.Id == 34069));

                    for (int i = 28587; i < 28601; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                }
                else if ((start.Id == 591) && (end.Id == 25176))
                {
                    for (int i = 591; i < 607; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 8986; i < 8997; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 28584; i < 28587; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    for (int i = 25153; i < 25177; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                }
                else if ((start.Id == 28483) && (end.Id == 25176))
                {
                    for (int i = 28483; i < 28493; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }

                    toReturn.Add(segments.Find(x => x.Id == 28455));
                    toReturn.Add(segments.Find(x => x.Id == 28583));
                    toReturn.Add(segments.Find(x => x.Id == 26062));

                    for (int i = 25153; i < 25177; i++)
                    {
                        toReturn.Add(segments.Find(x => x.Id == i));
                    }
                }
                #endregion

                #endregion
            }

            return toReturn;
        }
        #endregion




        #region Tools
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        static void Shuffle<T>(List<T> array)
        {
            int n = array.Count;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(rnd.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }
        #endregion




    }
}
