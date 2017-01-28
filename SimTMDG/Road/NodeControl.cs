using System;
using SimTMDG.Tools;
using System.Drawing;
using System.Collections.Generic;
using SimTMDG.Vehicle;
using System.Diagnostics;

namespace SimTMDG.Road
{
    [Serializable]
    public class NodeControl : ITickable
    {
        #region TEMP
        Rectangle mapBound;
        #endregion

        //private Routing _route;
        public List<Node> _nodes = new List<Node>();
        public List<RoadSegment> segments = new List<RoadSegment>();
        private List<VehicleGenerator> vehGenerators = new List<VehicleGenerator>();
        public List<IVehicle> unusedVehicles = new List<IVehicle>();
        public int ActiveVehicles = 0;

        internal List<RoadSegment> Segments
        {
            get
            {
                return segments;
            }

            set
            {
                segments = value;
            }
        }

        public NodeControl()
        {
            
        }


        // TODO temp for testing
        public void Load()
        {

            Clear();
        }


        public void Clear()
        {
            _nodes.Clear();

            foreach (RoadSegment ws in Segments)
            {
                //ws.vehicles.Clear();

                foreach(SegmentLane lane in ws.lanes)
                {
                    lane.vehicles.Clear();
                }
            }

            Segments.Clear();
        }


        public void Reset()
        {
            //_nodes.Clear();
            
            //foreach(WaySegment ws in Segments)
            //{
            //    ws.vehicles.Clear();
            //}

            //Segments.Clear();

            foreach (RoadSegment ws in segments)
            {
                ws.Reset();
            }

        }

        

        public void setBounds(Rectangle bounds)
        {
            mapBound = bounds;
        }


        #region draw
        public void Draw(Graphics g, int zoomLvl)
        {
            if (zoomLvl < 5)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.MidCoord, 0)))
                        ws.Draw(g);
                }
            }else if (zoomLvl < 8)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, 0)) || (IsInBound(ws.endNode.Position, 0)))
                        ws.Draw(g);
                }
            }else if (zoomLvl < 10)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, (int)mapBound.Width / 2)) || (IsInBound(ws.endNode.Position, (int)mapBound.Width / 2)))
                        ws.Draw(g);
                }
            }else
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, mapBound.Width)) || (IsInBound(ws.endNode.Position, mapBound.Width)))
                        ws.Draw(g);
                }
            }


            foreach (RoadSegment ws in Segments)
            {
                ws.startNode.Draw(g);
                ws.endNode.Draw(g);
            }

             //if Debug
            foreach (RoadSegment ws in Segments)
            {
                ws.DrawRoadID(g);
            }

            foreach (RoadSegment ws in Segments)
            {
                //foreach (IVehicle v in ws.vehicles)
                //{
                //    //if (v.distance <= v.CurrentSegment.Length)
                //    if (IsInBound(v.absCoord))
                //        v.Draw(g);
                //}

                foreach (SegmentLane lane in ws.lanes)
                {
                    foreach (IVehicle v in lane.vehicles)
                    {
                        if (IsInBound(v.absCoord, 0))
                            v.Draw(g);
                    }
                }
            }

            //Pen pen = new Pen(Color.OrangeRed, 1);

            //g.DrawRectangle(pen, (float)minBound.X, (float)minBound.Y, (float)maxBound.X, (float)maxBound.Y);
        }
        #endregion

        public bool IsInBound(Vector2 coord, int extension)
        {
            if  ((coord.X < mapBound.X - extension)||(coord.Y < mapBound.Y - extension) ||
                 (coord.X > mapBound.X + mapBound.Width + extension) ||
                 (coord.Y > mapBound.Y + mapBound.Height + extension)){
                return false;
            }else{
                return true;
            }
        }

        public void Tick(double tickLength, NodeControl nc = null)
        {
            foreach(RoadSegment ws in Segments)
            {
                ws.Tick(tickLength, this);
            }

            //generateVehicles();
            foreach(VehicleGenerator vehGen in vehGenerators)
            {
                ActiveVehicles += vehGen.generate(tickLength, this);
            }

            //foreach (WaySegment ws in Segments)
            //{
            //    foreach (IVehicle v in ws.vehicles)
            //    {
            //        v.newCoord();
            //    }
            //}
        }



        private List<RoadSegment> _route;
        private List<RoadSegment> _route2;
        private List<RoadSegment> _route3;
        private List<RoadSegment> _route4;
        private List<RoadSegment> _route5;
        private List<RoadSegment> _route6;


        Random rnd = new Random();
        int vehCount = 0;
        //int activeVehicles = 0;
        Double timeMod = 0.0;

        private void generateVehicles()
        {
            #region tempVehGenerate
            if ((timeMod % 36) == 0.0)
            {
                if ((vehCount % 2) == 0)
                {
                    int laneidx = rnd.Next(0, _route[0].lanes.Count);
                    int vehType = rnd.Next(0, 2);
                    IVehicle v = null;

                    if ((_route[0].lanes[laneidx].vehicles.Count == 0) ||
                        (_route[0].lanes[laneidx].vehicles[_route[0].lanes[laneidx].vehicles.Count - 1].RearPos <
                        _route[0].lanes[laneidx].Length - 20))
                    {

                        if (vehType == 0)
                        {
                            v = new Car(_route[0], laneidx, _route);
                        }
                        else if (vehType == 1)
                        {
                            v = new Bus(_route[0], laneidx, _route);
                        }
                        else
                        {
                            v = new Truck(_route[0], laneidx, _route);
                        }

                        _route[0].lanes[laneidx].vehicles.Add(v);
                        ActiveVehicles++;
                    }


                    laneidx = rnd.Next(0, _route2[0].lanes.Count);
                    vehType = rnd.Next(0, 2);
                    v = null;

                    if ((_route2[0].lanes[laneidx].vehicles.Count == 0) ||
                        (_route2[0].lanes[laneidx].vehicles[_route2[0].lanes[laneidx].vehicles.Count - 1].RearPos <
                        _route2[0].lanes[laneidx].Length - 20))
                    {
                        if (vehType == 0)
                        {
                            v = new Car(_route2[0], laneidx, _route2);
                        }
                        else if (vehType == 1)
                        {
                            v = new Bus(_route2[0], laneidx, _route2);
                        }
                        else
                        {
                            v = new Truck(_route2[0], laneidx, _route2);
                        }

                        _route2[0].lanes[laneidx].vehicles.Add(v);
                        ActiveVehicles++;
                    }

                }
                else
                {

                    int laneidx = rnd.Next(0, _route3[0].lanes.Count);
                    int vehType = rnd.Next(0, 2);
                    IVehicle v = null;

                    if ((_route3[0].lanes[laneidx].vehicles.Count == 0) ||
                        (_route3[0].lanes[laneidx].vehicles[_route3[0].lanes[laneidx].vehicles.Count - 1].RearPos <
                        _route3[0].lanes[laneidx].Length - 20))
                    {

                        if (vehType == 0)
                        {
                            v = new Car(_route3[0], laneidx, _route3);
                        }
                        else if (vehType == 1)
                        {
                            v = new Bus(_route3[0], laneidx, _route3);
                        }
                        else
                        {
                            v = new Truck(_route3[0], laneidx, _route3);
                        }

                        _route3[0].lanes[laneidx].vehicles.Add(v);
                        ActiveVehicles++;
                    }


                    laneidx = rnd.Next(0, _route4[0].lanes.Count);
                    vehType = rnd.Next(0, 2);
                    v = null;

                    if ((_route4[0].lanes[laneidx].vehicles.Count == 0) ||
                        (_route4[0].lanes[laneidx].vehicles[_route4[0].lanes[laneidx].vehicles.Count - 1].RearPos <
                        _route4[0].lanes[laneidx].Length - 20))
                    {

                        if (vehType == 0)
                        {
                            v = new Car(_route4[0], laneidx, _route4);
                        }
                        else if (vehType == 1)
                        {
                            v = new Bus(_route4[0], laneidx, _route4);
                        }
                        else
                        {
                            v = new Truck(_route4[0], laneidx, _route4);
                        }

                        _route4[0].lanes[laneidx].vehicles.Add(v);
                        ActiveVehicles++;
                    }
                }
                vehCount++;
            }

            timeMod++;
            #endregion
        }

        public void manuallyAddRoute()
        {
            //_route = new List<RoadSegment>();
            //_route2 = new List<RoadSegment>();
            //_route3 = new List<RoadSegment>();
            //_route4 = new List<RoadSegment>();
            //_route5 = new List<RoadSegment>();
            //_route6 = new List<RoadSegment>();

            //// Route 1 : Pasteur
            //for (int i = 2908; i < 2926; i++)
            //{
            //    _route.Add(segments.Find(x => x.Id == i));
            //}

            //_route.Add(segments.Find(x => x.Id == 421));

            //for (int i = 28456; i < 28469; i++)
            //{
            //    _route.Add(segments.Find(x => x.Id == i));
            //}

            //for (int i = 422; i < 431; i++)
            //{
            //    _route.Add(segments.Find(x => x.Id == i));
            //}

            ////nc.segments.Find(x => x.Id == 2918).endNode.tLight = new TrafficLight();

            //// Route 2 : Pasteur
            //for (int i = 591; i < 607; i++)
            //{
            //    _route2.Add(segments.Find(x => x.Id == i));
            //}

            //for (int i = 8986; i < 8997; i++)
            //{
            //    _route2.Add(segments.Find(x => x.Id == i));
            //}



            //for (int i = 28584; i < 28587; i++)
            //{
            //    _route2.Add(segments.Find(x => x.Id == i));
            //}

            ////_route2.Add(nc.segments.Find(x => x.Id == 34087));
            ////_route2.Add(nc.segments.Find(x => x.Id == 34084));

            //for (int i = 25153; i < 25177; i++)
            //{
            //    _route2.Add(segments.Find(x => x.Id == i));
            //}
            ////nc.segments.Find(x => x.Id == 25163).endNode.tLight = new TrafficLight();


            //// Route 3 : Pasteur
            //for (int i = 2908; i < 2926; i++)
            //{
            //    _route3.Add(segments.Find(x => x.Id == i));
            //}

            //_route3.Add(segments.Find(x => x.Id == 26040));
            //_route3.Add(segments.Find(x => x.Id == 34069));

            //for (int i = 28587; i < 28601; i++)
            //{
            //    _route3.Add(segments.Find(x => x.Id == i));
            //}


            //// Route 4 : Pasteur
            //for (int i = 28483; i < 28493; i++)
            //{
            //    _route4.Add(segments.Find(x => x.Id == i));
            //}

            //_route4.Add(segments.Find(x => x.Id == 28455));
            //_route4.Add(segments.Find(x => x.Id == 28583));
            //_route4.Add(segments.Find(x => x.Id == 26062));

            //for (int i = 25153; i < 25177; i++)
            //{
            //    _route4.Add(segments.Find(x => x.Id == i));
            //}



            #region temp test obstacle
            #endregion
            


            segments.Find(x => x.Id == 2925).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28583).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28586).startNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28586).startNode.tLight.SwitchToGreen();
            //RSHS
            segments.Find(x => x.Id == 28606).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28606).startNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 28611).startNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28611).startNode.tLight.SwitchToGreen();

            segments.Find(x => x.Id == 2919).TargetSpeed = 4;
            segments.Find(x => x.Id == 25163).TargetSpeed = 5;


            #region VehGen 1: #2908
            List<RoadSegment> origin_1__destinations = new List<RoadSegment>();
            List<double> origin_1__q_outs = new List<double>();
            origin_1__destinations.Add(segments.Find(x => x.Id == 430));        // 430));
            origin_1__destinations.Add(segments.Find(x => x.Id == 28735));      // 28598));

            origin_1__q_outs.Add(0.3);
            origin_1__q_outs.Add(0.7);

            vehGen1 = new VehicleGenerator(segments, segments.Find(x => x.Id == 2908), 0.8, origin_1__destinations, origin_1__q_outs, VehicleGenerator.VehRouteType.DEFAULT);


            vehGenerators.Add(vehGen1); 
            #endregion


            #region VehGen 2: #1
            List<RoadSegment> origin_2__destinations = new List<RoadSegment>();
            List<double> origin_2__q_outs = new List<double>();

            origin_2__destinations.Add(segments.Find(x => x.Id == 25176));
            origin_2__destinations.Add(segments.Find(x => x.Id == 266));
            origin_2__destinations.Add(segments.Find(x => x.Id == 29103));

            origin_2__q_outs.Add(.3);
            origin_2__q_outs.Add(.3);
            origin_2__q_outs.Add(.3);

            vehGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 1), .3, origin_2__destinations, origin_2__q_outs, VehicleGenerator.VehRouteType.DEFAULT);


            vehGenerators.Add(vehGen2);
            #endregion


            #region VehGen 3: #29103
            List<RoadSegment> origin_3__destinations = new List<RoadSegment>();
            List<double> origin_3__q_outs = new List<double>();

            origin_3__destinations.Add(segments.Find(x => x.Id == 988));
            origin_3__destinations.Add(segments.Find(x => x.Id == 8818));

            origin_3__q_outs.Add(.5);
            origin_3__q_outs.Add(.5);

            vehGen3 = new VehicleGenerator(segments, segments.Find(x => x.Id == 29103), .3, origin_3__destinations, origin_3__q_outs, VehicleGenerator.VehRouteType.DEFAULT);
            
            
            vehGenerators.Add(vehGen3);
            #endregion


            #region VehGen 4: #20390
            List<RoadSegment> origin_4__destinations = new List<RoadSegment>();
            List<double> origin_4__q_outs = new List<double>();

            origin_4__destinations.Add(segments.Find(x => x.Id == 245));
            origin_4__destinations.Add(segments.Find(x => x.Id == 30022));

            origin_4__q_outs.Add(.5);
            origin_4__q_outs.Add(.5);

            vehGen4 = new VehicleGenerator(segments, segments.Find(x => x.Id == 20390), .3, origin_4__destinations, origin_4__q_outs, VehicleGenerator.VehRouteType.DEFAULT);


            vehGenerators.Add(vehGen4);
            #endregion


            #region VehGen 5: #23059
            List<RoadSegment> origin_5__destinations = new List<RoadSegment>();
            List<double> origin_5__q_outs = new List<double>();

            //origin_5__destinations.Add(segments.Find(x => x.Id == 33646));
            origin_5__destinations.Add(segments.Find(x => x.Id == 33238));

            //origin_5__q_outs.Add(.5);
            origin_5__q_outs.Add(1);

            vehGen5 = new VehicleGenerator(segments, segments.Find(x => x.Id == 23059), .3, origin_5__destinations, origin_5__q_outs, VehicleGenerator.VehRouteType.DEFAULT);


            vehGenerators.Add(vehGen5);
            #endregion


            #region VehGen 6: #286
            List<RoadSegment> origin_6__destinations = new List<RoadSegment>();
            List<double> origin_6__q_outs = new List<double>();

            origin_6__destinations.Add(segments.Find(x => x.Id == 33630));
            origin_6__destinations.Add(segments.Find(x => x.Id == 22168));

            origin_6__q_outs.Add(.5);
            origin_6__q_outs.Add(.5);

            vehGen6 = new VehicleGenerator(segments, segments.Find(x => x.Id == 286), .3, origin_6__destinations, origin_6__q_outs, VehicleGenerator.VehRouteType.DEFAULT);


            vehGenerators.Add(vehGen6);
            #endregion


            #region Angkot 1: Cibaduyut - Kr Setra 529 22825
            List<RoadSegment> origin_7__destinations = new List<RoadSegment>();
            List<double> origin_7__q_outs = new List<double>();

            origin_7__destinations.Add(segments.Find(x => x.Id == 529));
            origin_7__q_outs.Add(1);
            anGen1 = new VehicleGenerator(segments, segments.Find(x => x.Id == 22825), 0.8, origin_7__destinations, origin_7__q_outs, VehicleGenerator.VehRouteType.FIXED);


            vehGenerators.Add(anGen1);
            #endregion

            #region Angkot Cicaheum Ledeng
            List<RoadSegment> origin_8__destinations = new List<RoadSegment>();
            List<double> origin_8__q_outs = new List<double>();

            origin_8__destinations.Add(segments.Find(x => x.Id == 33168));
            origin_8__q_outs.Add(1);
            anGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 27149), 0.8, origin_8__destinations, origin_8__q_outs, VehicleGenerator.VehRouteType.FIXED);


            vehGenerators.Add(anGen2);
            #endregion

            #region Angkot Kelapa - Dago
            List<RoadSegment> origin_9__destinations = new List<RoadSegment>();
            List<double> origin_9__q_outs = new List<double>();

            origin_9__destinations.Add(segments.Find(x => x.Id == 28803));
            origin_9__q_outs.Add(1);
            anGen3 = new VehicleGenerator(segments, segments.Find(x => x.Id == 19683), 0.8, origin_9__destinations, origin_9__q_outs, VehicleGenerator.VehRouteType.FIXED);


            vehGenerators.Add(anGen3);
            #endregion

            #region Angkot Stasiun - Dago
            List<RoadSegment> origin_10__destinations = new List<RoadSegment>();
            List<double> origin_10__q_outs = new List<double>();

            origin_10__destinations.Add(segments.Find(x => x.Id == 28804));
            origin_10__q_outs.Add(1);
            anGen4 = new VehicleGenerator(segments, segments.Find(x => x.Id == 19684), 0.8, origin_10__destinations, origin_10__q_outs, VehicleGenerator.VehRouteType.FIXED);


            vehGenerators.Add(anGen4);
            #endregion

            #region coba
            List<RoadSegment> origin_11__destinations = new List<RoadSegment>();
            List<double> origin_11__q_outs = new List<double>();

            origin_11__destinations.Add(segments.Find(x => x.Id == 1510));
            origin_11__q_outs.Add(1);
            anGen5 = new VehicleGenerator(segments, segments.Find(x => x.Id == 1533), 0.8, origin_11__destinations, origin_11__q_outs, VehicleGenerator.VehRouteType.FIXED);


            vehGenerators.Add(anGen5);
            #endregion


        }

        VehicleGenerator vehGen1;
        VehicleGenerator vehGen2;
        VehicleGenerator vehGen3;
        VehicleGenerator vehGen4;
        VehicleGenerator vehGen5;
        VehicleGenerator vehGen6;
        VehicleGenerator anGen1;
        VehicleGenerator anGen2;
        VehicleGenerator anGen3;
        VehicleGenerator anGen4;
        VehicleGenerator anGen5;

    }    
}
