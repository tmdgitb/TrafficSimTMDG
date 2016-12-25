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
        List<RoadSegment> route;// = new List<RoadSegment>();

        static Random rnd = new Random();


        /// Temp
        public List<RoadSegment> segments;
        List<AStar> astars;
        #endregion




        #region constructor
        public VehicleGenerator()
        {

        }

        public VehicleGenerator(RoadSegment _startSegment, double _q_in, List<RoadSegment> _endSegments, List<double> _q_outs)
        {
            this.q_in = _q_in;
            this.startSegment = _startSegment;
            this.endSegments = _endSegments;

            astars = new List<AStar>();
            for(int i=0; i<_endSegments.Count; i++)
            {
                astars.Add(new AStar(segments, startSegment, endSegments[i]));

                List<RoadSegment> test = new List<RoadSegment>(astars[i].costSoFar.Keys);
                foreach (RoadSegment segment in test)
                {
                    segment.debugColor = System.Drawing.Color.Red;
                }
            }

            this.q_outs = _q_outs;

        }
        #endregion




        #region tick
        /// <summary>
        /// The basic Idea is, we add a number to inVehBuffer (q_in which is veh/s * tickLength which is 1s/fps)
        /// Let's say we need to add 2 veh / s, with tickLength 0.25s ==> the inVehBuffer will adds 0.5 veh per tick
        /// Then we generate 1 vehicle every tick IF ONLY we have 1 or more inVehBuffer
        /// With this we generate a vehicle every 2 tick or 2 vehicles every 4 tick, with tickLength 0.25s it means 2 veh / s
        /// </summary>
        public int generate(double tickLength)
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

                            //route = new List<RoadSegment>(astars[destination].costSoFar.Keys);
                            route = findRoute(this.startSegment, this.endSegments[destination]);
                            vehGenerate(laneIdxList[i]);
                            toReturn = 1;
                            return toReturn;
                            //break;
                        }
                    }
                    
                }
            }

            return toReturn;
        }


        void vehGenerate(int laneidx)
        {
            /// Generate Vehicle
            int vehType = rnd.Next(0, 2);
            IVehicle v = null;

            if (vehType == 0)
            {
                v = new Car(startSegment, laneidx, route);
            }
            else if (vehType == 1)
            {
                v = new Bus(startSegment, laneidx, route);
            }
            else
            {
                v = new Truck(startSegment, laneidx, route);
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

            /// Manual Route
            /// Route 1: Pasteur
            if ((start.Id == 2908) && (end.Id == 430))
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

            //List<RoadSegment> toReturn = AStar astar = 
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
