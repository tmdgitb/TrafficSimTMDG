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
        public List<Node> qHal = new List<Node>();
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

            foreach (AngkotGenerator angGen in angGenerators)
            {
                ActiveVehicles += angGen.generate(tickLength, this);
            }

            foreach (Halte h in haltes)
            {
                h.Tick(tickLength);
            }

            //List<Node> qh = new List<Node>();
            //foreach (Node a in qHal)
            //{
            //    a.tHalte.Timing(tickLength, a);
            //}
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

        Halte H1;
        Halte H2;
        Halte H3;
        Halte H4;

        List<Halte> haltes = new List<Halte>();


        public void manuallyAddRoute()
        {
            #region Halte
            segments.Find(x => x.Id == 20972).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 20972).endNode.tHalte);
            haltes[0].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 7774).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 7774).endNode.tHalte);
            haltes[1].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 1).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 1).endNode.tHalte);
            haltes[2].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 7).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 7).endNode.tHalte);
            haltes[3].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 10).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 10).endNode.tHalte);
            haltes[4].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 16548).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 16548).endNode.tHalte);
            haltes[5].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 600).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 600).endNode.tHalte);
            haltes[6].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 358).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 358).endNode.tHalte);
            haltes[7].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 379).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 379).endNode.tHalte);
            haltes[8].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 630).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 630).endNode.tHalte);
            haltes[9].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 6141).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 6141).endNode.tHalte);
            haltes[10].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 12).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 12).endNode.tHalte);
            haltes[11].HaltePlace = Halte.Place.BLUE;

            segments.Find(x => x.Id == 9682).endNode.tHalte = new Halte();
            haltes.Add(segments.Find(x => x.Id == 9682).endNode.tHalte);
            haltes[12].HaltePlace = Halte.Place.BLUE;

            #endregion

            #region Tlight
            //Simpang
            segments.Find(x => x.Id == 20973).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 20973).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 9687).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 9687).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 16472).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 15640).endNode.tLight = new TrafficLight();

            //DAGO SURAPATI
            segments.Find(x => x.Id == 17503).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 17503).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 1013).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 1013).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 1053).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 6231).endNode.tLight = new TrafficLight();

            //dukomsel
            segments.Find(x => x.Id == 16737).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 16737).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 376).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 376).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 49).endNode.tLight = new TrafficLight();

            //sebelum bip
            segments.Find(x => x.Id == 372).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 372).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 14831).endNode.tLight = new TrafficLight();
           
            //pajajaran
            segments.Find(x => x.Id == 15490).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 15490).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 9593).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 9593).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 246).endNode.tLight = new TrafficLight();
           

            //baltos
            segments.Find(x => x.Id == 6093).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 6093).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 635).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 635).endNode.tLight.SwitchToGreen();
            segments.Find(x => x.Id == 6144).endNode.tLight = new TrafficLight();
            segments.Find(x => x.Id == 13384).endNode.tLight = new TrafficLight();

            //segments.Find(x => x.Id == ).endNode.tLight = new TrafficLight();
            //segments.Find(x => x.Id == ).endNode.tLight.SwitchToGreen();
            //segments.Find(x => x.Id == ).endNode.tLight = new TrafficLight();
            //segments.Find(x => x.Id == ).endNode.tLight.SwitchToGreen();
            //segments.Find(x => x.Id == ).endNode.tLight = new TrafficLight();
            //segments.Find(x => x.Id == ).endNode.tLight = new TrafficLight();

            //segments.Find(x => x.Id == 2919).TargetSpeed = 4;
            //segments.Find(x => x.Id == 25163).TargetSpeed = 5;
            #endregion

            #region vehgen lama
            //#region VehGen 1: #2908
            //List<RoadSegment> origin_1__destinations = new List<RoadSegment>();
            //List<double> origin_1__q_outs = new List<double>();
            //origin_1__destinations.Add(segments.Find(x => x.Id == 430));        // 430));
            //origin_1__destinations.Add(segments.Find(x => x.Id == 28735));      // 28598));

            //origin_1__q_outs.Add(0.3);
            //origin_1__q_outs.Add(0.7);

            //vehGen1 = new VehicleGenerator(segments, segments.Find(x => x.Id == 2908), 0.8, origin_1__destinations, origin_1__q_outs);


            //vehGenerators.Add(vehGen1); 
            //#endregion


            //#region VehGen 2: #1
            //List<RoadSegment> origin_2__destinations = new List<RoadSegment>();
            //List<double> origin_2__q_outs = new List<double>();

            //origin_2__destinations.Add(segments.Find(x => x.Id == 25176));
            //origin_2__destinations.Add(segments.Find(x => x.Id == 266));
            //origin_2__destinations.Add(segments.Find(x => x.Id == 29103));

            //origin_2__q_outs.Add(.3);
            //origin_2__q_outs.Add(.3);
            //origin_2__q_outs.Add(.3);

            //vehGen2 = new VehicleGenerator(segments, segments.Find(x => x.Id == 1), .3, origin_2__destinations, origin_2__q_outs);


            //vehGenerators.Add(vehGen2);
            //#endregion


            //#region VehGen 3: #29103
            //List<RoadSegment> origin_3__destinations = new List<RoadSegment>();
            //List<double> origin_3__q_outs = new List<double>();

            //origin_3__destinations.Add(segments.Find(x => x.Id == 988));
            //origin_3__destinations.Add(segments.Find(x => x.Id == 8818));

            //origin_3__q_outs.Add(.5);
            //origin_3__q_outs.Add(.5);

            //vehGen3 = new VehicleGenerator(segments, segments.Find(x => x.Id == 29103), .3, origin_3__destinations, origin_3__q_outs);


            //vehGenerators.Add(vehGen3);
            //#endregion


            //#region VehGen 4: #20390
            //List<RoadSegment> origin_4__destinations = new List<RoadSegment>();
            //List<double> origin_4__q_outs = new List<double>();

            //origin_4__destinations.Add(segments.Find(x => x.Id == 245));
            //origin_4__destinations.Add(segments.Find(x => x.Id == 30022));

            //origin_4__q_outs.Add(.5);
            //origin_4__q_outs.Add(.5);

            //vehGen4 = new VehicleGenerator(segments, segments.Find(x => x.Id == 20390), .3, origin_4__destinations, origin_4__q_outs);


            //vehGenerators.Add(vehGen4);
            //#endregion


            #region VehGen 5: #23059
            List<RoadSegment> origin_5__destinations = new List<RoadSegment>();
            List<double> origin_5__q_outs = new List<double>();

            
            origin_5__destinations.Add(segments.Find(x => x.Id == 15894));

            //origin_5__q_outs.Add(.5);
            origin_5__q_outs.Add(1);

            vehGen5 = new VehicleGenerator(segments, segments.Find(x => x.Id == 7756), .3, origin_5__destinations, origin_5__q_outs);


            vehGenerators.Add(vehGen5);
            #endregion

            #region VehGen 5a: #23059
            List<RoadSegment> origin_5a__destinations = new List<RoadSegment>();
            List<double> origin_5a__q_outs = new List<double>();


            origin_5a__destinations.Add(segments.Find(x => x.Id == 6047));

            //origin_5__q_outs.Add(.5);
            origin_5a__q_outs.Add(1);

            vehGen5a = new VehicleGenerator(segments, segments.Find(x => x.Id == 236), .3, origin_5a__destinations, origin_5a__q_outs);


            vehGenerators.Add(vehGen5a);
            #endregion


            #region VehGen 6: #286
            List<RoadSegment> origin_6__destinations = new List<RoadSegment>();
            List<double> origin_6__q_outs = new List<double>();

            origin_6__destinations.Add(segments.Find(x => x.Id == 14840));
            origin_6__destinations.Add(segments.Find(x => x.Id == 239));

            origin_6__q_outs.Add(.5);
            origin_6__q_outs.Add(.5);

            vehGen6 = new VehicleGenerator(segments, segments.Find(x => x.Id == 286), .3, origin_6__destinations, origin_6__q_outs);


            vehGenerators.Add(vehGen6);

            #endregion

            #region VehGen 7:
            List<RoadSegment> origin_6a__destinations = new List<RoadSegment>();
            List<double> origin_6a__q_outs = new List<double>();

            origin_6a__destinations.Add(segments.Find(x => x.Id == 16540));
            origin_6a__destinations.Add(segments.Find(x => x.Id == 13433));

            origin_6a__q_outs.Add(.5);
            origin_6a__q_outs.Add(.5);

            vehGen6a = new VehicleGenerator(segments, segments.Find(x => x.Id == 6214), .3, origin_6a__destinations, origin_6a__q_outs);


            vehGenerators.Add(vehGen6a);

            #endregion

            #endregion

            #region Angkot
            #region Angkot 1: Cibaduyut - Kr Setra 
            //List<AngkotGenerator.AngkotPoint> origin_7__destinations = new List<AngkotGenerator.AngkotPoint>();
            //List<double> origin_7__q_outs = new List<double>();

            //origin_7__destinations.Add(AngkotGenerator.AngkotPoint.KR_SETRA);
            //origin_7__q_outs.Add(1);

            //anGen1 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.CIBADUYUT, 0.1, origin_7__destinations, origin_7__q_outs);


            //angGenerators.Add(anGen1);
            #endregion

            #region Angkot  Ledeng Cicaheum
            List<AngkotGenerator.AngkotPoint> origin_8__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_8__q_outs = new List<double>();

            origin_8__destinations.Add(AngkotGenerator.AngkotPoint.CICAHEUM);
            origin_8__q_outs.Add(1);

            anGen2 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.LEDENG, 0.1, origin_8__destinations, origin_8__q_outs);


            angGenerators.Add(anGen2);
            #endregion

            #region Angkot Sadang Saerang-SAdangsrgb
            List<AngkotGenerator.AngkotPoint> origin_14__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_14__q_outs = new List<double>();
            origin_14__destinations.Add(AngkotGenerator.AngkotPoint.SADANGSRGb);
            origin_14__q_outs.Add(1);
            anGen8 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.SADANGSRG, 0.04, origin_14__destinations, origin_14__q_outs);
            angGenerators.Add(anGen8);
            #endregion

            #region Angkot Sadang Saerang b -Caringin
            List<AngkotGenerator.AngkotPoint> origin_14a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_14a__q_outs = new List<double>();
            origin_14a__destinations.Add(AngkotGenerator.AngkotPoint.CARINGIN);
            origin_14a__q_outs.Add(1);
            anGen8a = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.SADANGSRGb, 0.03, origin_14a__destinations, origin_14a__q_outs);
            angGenerators.Add(anGen8a);
            #endregion

            #region Angkot Caringin - Sadang Saerang b
            List<AngkotGenerator.AngkotPoint> origin_14b__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_14b__q_outs = new List<double>();
            origin_14b__destinations.Add(AngkotGenerator.AngkotPoint.SADANGSRG);
            origin_14b__q_outs.Add(1);
            anGen8b = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.CARINGIN, 0.1, origin_14b__destinations, origin_14b__q_outs);
            angGenerators.Add(anGen8b);
            #endregion

            #region Angkot Dago - Kelapa
            List<AngkotGenerator.AngkotPoint> origin_9__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_9__q_outs = new List<double>();
            origin_9__destinations.Add(AngkotGenerator.AngkotPoint.KALAPA);
            origin_9__q_outs.Add(1);
            anGen3 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.15, origin_9__destinations, origin_9__q_outs);
            angGenerators.Add(anGen3);
            #endregion

            #region Angkot Kelapa Dago 
            List<AngkotGenerator.AngkotPoint> origin_9a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_9a__q_outs = new List<double>();
            origin_9a__destinations.Add(AngkotGenerator.AngkotPoint.DAGO);
            origin_9a__q_outs.Add(1);
            anGen3a = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.KALAPA, 0.15, origin_9a__destinations, origin_9a__q_outs);
            angGenerators.Add(anGen3a);
            #endregion

            #region Angkot Dago-Stasiun
            List<AngkotGenerator.AngkotPoint> origin_10__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_10__q_outs = new List<double>();
            origin_10__destinations.Add(AngkotGenerator.AngkotPoint.STASIUN);
            origin_10__q_outs.Add(1);
            anGen4 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.1, origin_10__destinations, origin_10__q_outs);
            angGenerators.Add(anGen4);
            #endregion

            #region Angkot Stasiun - Stasiunb
            List<AngkotGenerator.AngkotPoint> origin_10a__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_10a__q_outs = new List<double>();
            origin_10a__destinations.Add(AngkotGenerator.AngkotPoint.STASIUNb);
            origin_10a__q_outs.Add(1);
            anGen4a = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.STASIUN, 0.1, origin_10a__destinations, origin_10a__q_outs);
            angGenerators.Add(anGen4a);
            #endregion

            #region Angkot Stasiunb -Dago
            List<AngkotGenerator.AngkotPoint> origin_10b__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_10b__q_outs = new List<double>();
            origin_10b__destinations.Add(AngkotGenerator.AngkotPoint.DAGO);
            origin_10b__q_outs.Add(1);
            anGen4b = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.STASIUNb, 0.1, origin_10b__destinations, origin_10b__q_outs);
            angGenerators.Add(anGen4b);
            #endregion

            ////Angkot Buah Batu Kelapa
            //List<AngkotGenerator.AngkotPoint> origin_11__destinations = new List<AngkotGenerator.AngkotPoint>();
            //List<double> origin_11__q_outs = new List<double>();
            //origin_11__destinations.Add(AngkotGenerator.AngkotPoint.KALAPA);
            //origin_11__q_outs.Add(1);
            //anGen5 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.BUAHBATU, 0.1, origin_11__destinations, origin_11__q_outs);
            //angGenerators.Add(anGen5);

            #region Angkot Margahayu Ledeng
            List<AngkotGenerator.AngkotPoint> origin_12__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_12__q_outs = new List<double>();
            origin_12__destinations.Add(AngkotGenerator.AngkotPoint.LEDENG);
            origin_12__q_outs.Add(1);
            anGen6 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.MARGAHAYU, 0.1, origin_12__destinations, origin_12__q_outs);
            angGenerators.Add(anGen6);
            #endregion

            ////Angkot Ledeng Kelapa
            //List<AngkotGenerator.AngkotPoint> origin_13__destinations = new List<AngkotGenerator.AngkotPoint>();
            //List<double> origin_13__q_outs = new List<double>();
            //origin_13__destinations.Add(AngkotGenerator.AngkotPoint.LEDENG);
            //origin_13__q_outs.Add(1);
            //anGen7 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.KALAPA, 0.1, origin_13__destinations, origin_13__q_outs);
            //angGenerators.Add(anGen7);

            //Angkot Dago Riung
            List<AngkotGenerator.AngkotPoint> origin_15z__destinations = new List<AngkotGenerator.AngkotPoint>();
            List<double> origin_15z__q_outs = new List<double>();
            origin_15z__destinations.Add(AngkotGenerator.AngkotPoint.RIUNG);
            origin_15z__q_outs.Add(1);
            anGen9 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.1, origin_15z__destinations, origin_15z__q_outs);
            angGenerators.Add(anGen9);
            
            #endregion
            

            #region coba tertiary
            //List<AngkotGenerator.AngkotPoint> origin_10__destinations = new List<AngkotGenerator.AngkotPoint>();
            //List<double> origin_10__q_outs = new List<double>();

            //origin_10__destinations.Add(AngkotGenerator.AngkotPoint.STASIUN);
            //origin_10__q_outs.Add(1);
            //anGen4 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.05, origin_10__destinations, origin_10__q_outs);
            //angGenerators.Add(anGen4);

            //List<AngkotGenerator.AngkotPoint> origin_9__destinations = new List<AngkotGenerator.AngkotPoint>();
            //List<double> origin_9__q_outs = new List<double>();

            //origin_9__destinations.Add(AngkotGenerator.AngkotPoint.KALAPA);
            //origin_9__q_outs.Add(1);

            //anGen3 = new AngkotGenerator(segments, AngkotGenerator.AngkotPoint.DAGO, 0.1, origin_9__destinations, origin_9__q_outs);


            //angGenerators.Add(anGen3);

            //List<RoadSegment> origin_11__destinations = new List<RoadSegment>();
            //List<double> origin_11__q_outs = new List<double>();
            //origin_11__destinations.Add(segments.Find(x => x.Id == 2));
            //origin_11__q_outs.Add(1);
            //vehGen5coba = new VehicleGenerator(segments, segments.Find(x => x.Id == 16750), 0.8, origin_11__destinations, origin_11__q_outs);
            //vehGenerators.Add(vehGen5coba);

            #endregion



        }

        VehicleGenerator vehGen1;
        VehicleGenerator vehGen2;
        VehicleGenerator vehGen3;
        VehicleGenerator vehGen4;
        VehicleGenerator vehGen5;
        VehicleGenerator vehGen5a;
        VehicleGenerator vehGen5coba;
        VehicleGenerator vehGen6;
        VehicleGenerator vehGen6a;
        AngkotGenerator anGen1;
        AngkotGenerator anGen2;
        AngkotGenerator anGen3;
        AngkotGenerator anGen3a;
        AngkotGenerator anGen4;
        AngkotGenerator anGen4a;
        AngkotGenerator anGen4b;
        AngkotGenerator anGen5;
        AngkotGenerator anGen6;
        AngkotGenerator anGen7;
        AngkotGenerator anGen8;
        AngkotGenerator anGen8a;
        AngkotGenerator anGen8b;
        AngkotGenerator anGen9;
        

    }    
}
