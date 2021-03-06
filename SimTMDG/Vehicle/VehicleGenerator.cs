﻿using SimTMDG.Road;
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
        protected double q_in = 0;
        protected List<double> q_outs;
        protected RoadSegment startSegment;
        List<RoadSegment> endSegments;
        protected double inVehBuffer = 0;
        //List<RoadSegment> route;// = new List<RoadSegment>();
        protected static Random rnd = new Random();
        
        /// Temp
        public List<RoadSegment> segments;
        public List<List<RoadSegment>> routes = new List<List<RoadSegment>>();
        #endregion




        #region constructor
        public VehicleGenerator()
        {

        }

        public VehicleGenerator(List<RoadSegment> _segments, RoadSegment _startSegment, double _q_in, List<RoadSegment> _endSegments, List<double> _q_outs)
        {
            this.q_in = _q_in;
            this.segments = _segments;
            this.startSegment = _startSegment;
            this.endSegments = _endSegments;

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
            if ((nc.ActiveVehicles > 3000) && (nc.unusedVehicles.Count > 0))
            {
                v = nc.unusedVehicles[0];
                nc.unusedVehicles.RemoveAt(0);

                v.Reuse(startSegment, laneidx, routes[destination]);

                //v.color = System.Drawing.Color.Pink;

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
            
            #region AStar route
            AStar newAStar = new AStar(this.segments, start, end);
            if (newAStar.goalIsFound)
            {
                toReturn = generatePath(newAStar, start, end);
            }
            #endregion            

            return toReturn;
        }
        #endregion




        #region Tools
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="array">Array to shuffle.</param>
        protected static void Shuffle<T>(List<T> array)
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
