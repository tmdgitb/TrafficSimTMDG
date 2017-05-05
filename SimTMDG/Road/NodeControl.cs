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
        private List<AngkotGenerator> angGenerators = new List<AngkotGenerator>();
        public List<IVehicle> unusedVehicles = new List<IVehicle>();
        public int ActiveVehicles = 0;

        internal List<RoadSegment> Segments
        {
            get { return segments; }
            set { segments = value; }
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

        #region draw perf test
        #endregion
        public int objDraw;
        Pen pen = new Pen(Color.Gray, 1);

        public void Draw(Graphics g, int zoomLvl)
        {
            objDraw = 0;
            /// new

            if (zoomLvl < 5)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.MidCoord, 0)))
                    {
                        DrawRoadSegment(g, ws);
                    }
                }
            }
            else if (zoomLvl < 8)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, 0)) || (IsInBound(ws.endNode.Position, 0)))
                    {
                        DrawSegmentLane(g, ws);
                    }
                }
            }
            else if (zoomLvl < 10)
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, (int)mapBound.Width / 2)) || (IsInBound(ws.endNode.Position, (int)mapBound.Width / 2)))
                    {
                        DrawSegmentLane(g, ws);
                    }
                }
            }
            else
            {
                foreach (RoadSegment ws in Segments)
                {
                    if ((IsInBound(ws.startNode.Position, mapBound.Width)) || (IsInBound(ws.endNode.Position, mapBound.Width)))
                    {
                        DrawSegmentLane(g, ws);
                    }
                }
            }

            Rectangle rec = new Rectangle(0, 0, 2, 2);
            Color penColor = Color.Gray;

            foreach (RoadSegment ws in Segments)
            {
                //ws.startNode.Draw(g);
                //ws.endNode.Draw(g);
                objDraw += 2;

                /// start node

                if (ws.startNode.tLight != null)
                {
                    if (ws.startNode.tLight.trafficLightState == TrafficLight.State.GREEN)
                    {
                        penColor = Color.Green;
                    }
                    else if (ws.startNode.tLight.trafficLightState == TrafficLight.State.RED)
                    {
                        penColor = Color.Red;
                    }

                    pen.Color = penColor;
                    rec.X = (int)ws.startNode.Position.X - 1;
                    rec.Y = (int)ws.startNode.Position.Y - 1;
                    g.DrawEllipse(pen, rec);
                }

                /// end node
                if (ws.endNode.tLight != null)
                {
                    if (ws.endNode.tLight.trafficLightState == TrafficLight.State.GREEN)
                    {
                        penColor = Color.Green;
                    }
                    else if (ws.endNode.tLight.trafficLightState == TrafficLight.State.RED)
                    {
                        penColor = Color.Red;
                    }

                    pen.Color = penColor;
                    rec.X = (int)ws.endNode.Position.X - 1;
                    rec.Y = (int)ws.endNode.Position.Y - 1;
                    g.DrawEllipse(pen, rec);
                }
            }

            ///if Debug
            foreach (RoadSegment ws in Segments)
            {
                ws.DrawRoadID(g);
            }

            foreach (RoadSegment ws in Segments)
            {
                foreach (SegmentLane lane in ws.lanes)
                {
                    foreach (IVehicle v in lane.vehicles)
                    {
                        if (IsInBound(v.absCoord, 0))
                        {
                            v.Draw(g);
                            objDraw++;
                        }
                    }
                }
            }

        }

        public void DrawSegmentLane(Graphics g, RoadSegment segment)
        {
            //ws.Draw(g);
            objDraw++;

            foreach (SegmentLane lane in segment.lanes)
            {
                //g.DrawLine(pen, (Point)lane.startNode.Position, (Point)lane.endNode.Position);
                //pen.Color = lane.debugColor;
                pen.Width = 1;
                pen.Color = Color.Gray;
                lane.Draw(g, pen);
            }
        }

        public void DrawRoadSegment(Graphics g, RoadSegment segment)
        {
            objDraw++;
            pen.Width = 2;
            pen.Color = segment.debugColor;
            segment.Draw(g, pen);

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
            foreach (RoadSegment ws in Segments)
            {
                ws.Tick(tickLength, this);
            }

            //System.Threading.Tasks.Parallel.ForEach(segments, ws =>
            //{
            //    ws.Tick(tickLength, this);
            //});

            //generateVehicles();
            foreach (VehicleGenerator vehGen in vehGenerators)
            {
                ActiveVehicles += vehGen.generate(tickLength, this);
            }

            foreach (AngkotGenerator angGen in angGenerators)
            {
                ActiveVehicles += angGen.generate(tickLength, this);
            }

            //foreach (WaySegment ws in Segments)
            //{
            //    foreach (IVehicle v in ws.vehicles)
            //    {
            //        v.newCoord();
            //    }
            //}
        }



        //private List<RoadSegment> _route;
        //private List<RoadSegment> _route2;
        //private List<RoadSegment> _route3;
        //private List<RoadSegment> _route4;
        //private List<RoadSegment> _route5;
        //private List<RoadSegment> _route6;


        Random rnd = new Random();
        int vehCount = 0;
        //int activeVehicles = 0;
        Double timeMod = 0.0;

        private void generateVehicles()
        {
            #region tempVehGenerate
            //if ((timeMod % 36) == 0.0)
            //{
            //    if ((vehCount % 2) == 0)
            //    {
            //        int laneidx = rnd.Next(0, _route[0].lanes.Count);
            //        int vehType = rnd.Next(0, 2);
            //        IVehicle v = null;

            //        if ((_route[0].lanes[laneidx].vehicles.Count == 0) ||
            //            (_route[0].lanes[laneidx].vehicles[_route[0].lanes[laneidx].vehicles.Count - 1].RearPos <
            //            _route[0].lanes[laneidx].Length - 20))
            //        {

            //            if (vehType == 0)
            //            {
            //                v = new Car(_route[0], laneidx, _route);
            //            }
            //            else if (vehType == 1)
            //            {
            //                v = new Bus(_route[0], laneidx, _route);
            //            }
            //            else
            //            {
            //                v = new Truck(_route[0], laneidx, _route);
            //            }

            //            _route[0].lanes[laneidx].vehicles.Add(v);
            //            ActiveVehicles++;
            //        }


            //        laneidx = rnd.Next(0, _route2[0].lanes.Count);
            //        vehType = rnd.Next(0, 2);
            //        v = null;

            //        if ((_route2[0].lanes[laneidx].vehicles.Count == 0) ||
            //            (_route2[0].lanes[laneidx].vehicles[_route2[0].lanes[laneidx].vehicles.Count - 1].RearPos <
            //            _route2[0].lanes[laneidx].Length - 20))
            //        {
            //            if (vehType == 0)
            //            {
            //                v = new Car(_route2[0], laneidx, _route2);
            //            }
            //            else if (vehType == 1)
            //            {
            //                v = new Bus(_route2[0], laneidx, _route2);
            //            }
            //            else
            //            {
            //                v = new Truck(_route2[0], laneidx, _route2);
            //            }

            //            _route2[0].lanes[laneidx].vehicles.Add(v);
            //            ActiveVehicles++;
            //        }

            //    }
            //    else
            //    {

            //        int laneidx = rnd.Next(0, _route3[0].lanes.Count);
            //        int vehType = rnd.Next(0, 2);
            //        IVehicle v = null;

            //        if ((_route3[0].lanes[laneidx].vehicles.Count == 0) ||
            //            (_route3[0].lanes[laneidx].vehicles[_route3[0].lanes[laneidx].vehicles.Count - 1].RearPos <
            //            _route3[0].lanes[laneidx].Length - 20))
            //        {

            //            if (vehType == 0)
            //            {
            //                v = new Car(_route3[0], laneidx, _route3);
            //            }
            //            else if (vehType == 1)
            //            {
            //                v = new Bus(_route3[0], laneidx, _route3);
            //            }
            //            else
            //            {
            //                v = new Truck(_route3[0], laneidx, _route3);
            //            }

            //            _route3[0].lanes[laneidx].vehicles.Add(v);
            //            ActiveVehicles++;
            //        }


            //        laneidx = rnd.Next(0, _route4[0].lanes.Count);
            //        vehType = rnd.Next(0, 2);
            //        v = null;

            //        if ((_route4[0].lanes[laneidx].vehicles.Count == 0) ||
            //            (_route4[0].lanes[laneidx].vehicles[_route4[0].lanes[laneidx].vehicles.Count - 1].RearPos <
            //            _route4[0].lanes[laneidx].Length - 20))
            //        {

            //            if (vehType == 0)
            //            {
            //                v = new Car(_route4[0], laneidx, _route4);
            //            }
            //            else if (vehType == 1)
            //            {
            //                v = new Bus(_route4[0], laneidx, _route4);
            //            }
            //            else
            //            {
            //                v = new Truck(_route4[0], laneidx, _route4);
            //            }

            //            _route4[0].lanes[laneidx].vehicles.Add(v);
            //            ActiveVehicles++;
            //        }
            //    }
            //    vehCount++;
            //}

            //timeMod++;
            #endregion
        }

        public void manuallyAddRoute()
        {
            #region manual
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
            #endregion



            #region temp test obstacle
            #endregion

            #region log Traffic Light
            //26962
            segments.Find(x => x.Id == 1).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 1).endNode.tLight.SwitchToGreen();
            //segments.Find(x => x.Id == 26964).endNode.tLight = new TrafficLight();
            //segments.Find(x => x.Id == 26964).endNode.tLight.SwitchToRed();
            #endregion


            segments.Find(x => x.Id == 2925).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28583).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28586).startNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28586).startNode.tLight.SwitchToGreen();
            //RSHS
            segments.Find(x => x.Id == 28606).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28606).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 28611).startNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 28611).startNode.tLight.SwitchToGreen();

            segments.Find(x => x.Id == 2919).MaxSpeed = 4;
            segments.Find(x => x.Id == 25163).MaxSpeed = 5;


            #region VehGen 1: #2908
            List<RoadSegment> origin_1__destinations = new List<RoadSegment>();
            List<double> origin_1__q_outs = new List<double>();
            origin_1__destinations.Add(segments.Find(x => x.Id == 430));        // 430));
            origin_1__destinations.Add(segments.Find(x => x.Id == 28735));      // 28598));

            origin_1__q_outs.Add(0.3);
            origin_1__q_outs.Add(0.7);

            vehGen1 = new VehicleGenerator(segments, segments.Find(x => x.Id == 2908), 0.8, origin_1__destinations, origin_1__q_outs);


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

            vehGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 1), .3, origin_2__destinations, origin_2__q_outs);


            //origin_2__destinations.Add(segments.Find(x => x.Id == 26964));
            //origin_2__q_outs.Add(1);
            //vehGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 353), .35, origin_2__destinations, origin_2__q_outs);
            vehGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 0), .35, origin_2__destinations, origin_2__q_outs);



            vehGenerators.Add(vehGen2);
            #endregion


            #region VehGen 3: #29103
            List<RoadSegment> origin_3__destinations = new List<RoadSegment>();
            List<double> origin_3__q_outs = new List<double>();

            origin_3__destinations.Add(segments.Find(x => x.Id == 988));
            origin_3__destinations.Add(segments.Find(x => x.Id == 8818));

            origin_3__q_outs.Add(.5);
            origin_3__q_outs.Add(.5);

            vehGen3 = new VehicleGenerator(segments, segments.Find(x => x.Id == 29103), .3, origin_3__destinations, origin_3__q_outs);


            vehGenerators.Add(vehGen3);
            #endregion


            #region VehGen 4: #20390
            List<RoadSegment> origin_4__destinations = new List<RoadSegment>();
            List<double> origin_4__q_outs = new List<double>();

            origin_4__destinations.Add(segments.Find(x => x.Id == 245));
            origin_4__destinations.Add(segments.Find(x => x.Id == 30022));

            origin_4__q_outs.Add(.5);
            origin_4__q_outs.Add(.5);

            vehGen4 = new VehicleGenerator(segments, segments.Find(x => x.Id == 20390), .3, origin_4__destinations, origin_4__q_outs);


            vehGenerators.Add(vehGen4);
            #endregion


            #region VehGen 5: #23059
            List<RoadSegment> origin_5__destinations = new List<RoadSegment>();
            List<double> origin_5__q_outs = new List<double>();

            //origin_5__destinations.Add(segments.Find(x => x.Id == 33646));
            origin_5__destinations.Add(segments.Find(x => x.Id == 33238));

            //origin_5__q_outs.Add(.5);
            origin_5__q_outs.Add(1);

            vehGen5 = new VehicleGenerator(segments, segments.Find(x => x.Id == 23059), .3, origin_5__destinations, origin_5__q_outs);


            vehGenerators.Add(vehGen5);
            #endregion


            #region VehGen 6: #286
            List<RoadSegment> origin_6__destinations = new List<RoadSegment>();
            List<double> origin_6__q_outs = new List<double>();

            origin_6__destinations.Add(segments.Find(x => x.Id == 33630));
            origin_6__destinations.Add(segments.Find(x => x.Id == 22168));

            origin_6__q_outs.Add(.5);
            origin_6__q_outs.Add(.5);

            vehGen6 = new VehicleGenerator(segments, segments.Find(x => x.Id == 286), .3, origin_6__destinations, origin_6__q_outs);


            vehGenerators.Add(vehGen6);
            #endregion


            #region VehGen 7: #286
            List<RoadSegment> origin_7__destinations = new List<RoadSegment>();
            List<double> origin_7__q_outs = new List<double>();
            
            origin_7__destinations.Add(segments.Find(x => x.Id == 30141));
            
            origin_7__q_outs.Add(1);

            vehGen7 = new VehicleGenerator(segments, segments.Find(x => x.Id == 20419), .3, origin_7__destinations, origin_7__q_outs);


            vehGenerators.Add(vehGen7);
            #endregion


            #region VehGen 8: #286
            List<RoadSegment> origin_8__destinations = new List<RoadSegment>();
            List<double> origin_8__q_outs = new List<double>();

            origin_8__destinations.Add(segments.Find(x => x.Id == 35013));

            origin_8__q_outs.Add(1);

            vehGen8 = new VehicleGenerator(segments, segments.Find(x => x.Id == 23195), .3, origin_8__destinations, origin_8__q_outs);


            vehGenerators.Add(vehGen8);
            #endregion



            #region Angkot 1: Cibaduyut - Kr Setra 529 22825
            List<AngkotGenerator.AngkotPoint> origin_1a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_1a__q_outs = new List<double>();

            origin_1a__destinations.Add(AngkotGenerator.AngkotPoint.KR_SETRA);
            origin_1a__q_outs.Add(1);

            anGen1 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.CIBADUYUT, 0.1, origin_1a__destinations, origin_1a__q_outs);


            angGenerators.Add(anGen1);
            #endregion

            #region Angkot Cicaheum Ledeng
            List<AngkotGenerator.AngkotPoint> origin_2a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_2a__q_outs = new List<double>();

            origin_2a__destinations.Add(AngkotGenerator.AngkotPoint.CICAHEUM);
            origin_2a__q_outs.Add(1);

            anGen2 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.LEDENG, 0.1, origin_2a__destinations, origin_2a__q_outs);


            angGenerators.Add(anGen2);
            #endregion

            #region Angkot Kelapa - Dago
            List<AngkotGenerator.AngkotPoint> origin_3a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_3a__q_outs = new List<double>();

            origin_3a__destinations.Add(AngkotGenerator.AngkotPoint.KALAPA);
            origin_3a__q_outs.Add(1);

            anGen3 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.1, origin_3a__destinations, origin_3a__q_outs);


            angGenerators.Add(anGen3);
            #endregion

            #region Angkot Stasiun - Dago
            List<AngkotGenerator.AngkotPoint> origin_4a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_4a__q_outs = new List<double>();

            origin_4a__destinations.Add(AngkotGenerator.AngkotPoint.STASIUN);
            origin_4a__q_outs.Add(1);
            anGen4 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.1, origin_4a__destinations, origin_4a__q_outs);


            angGenerators.Add(anGen4);
            #endregion

            #region coba
            //List<RoadSegment> origin_11__destinations = new List<RoadSegment>();
            //List<double> origin_11__q_outs = new List<double>();

            //origin_11__destinations.Add(segments.Find(x => x.Id == 1510));
            //origin_11__q_outs.Add(1);
            //anGen5 = new VehicleGenerator(segments, segments.Find(x => x.Id == 1533), 0.8, origin_11__destinations, origin_11__q_outs);


            //angGenerators.Add(anGen5);
            #endregion

        }

        VehicleGenerator vehGen1;
        VehicleGenerator vehGen2;
        VehicleGenerator vehGen3;
        VehicleGenerator vehGen4;
        VehicleGenerator vehGen5;
        VehicleGenerator vehGen6;
        VehicleGenerator vehGen7;
        VehicleGenerator vehGen8;
        AngkotGenerator anGen1;
        AngkotGenerator anGen2;
        AngkotGenerator anGen3;
        AngkotGenerator anGen4;
        //VehicleGenerator anGen5;

    }    
}
